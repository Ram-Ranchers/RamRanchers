using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace DecisionMakingAI
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public Vector3 startPosition;
        public GameGlobalParameters gameGlobalParameters;
        public GameObject fov;
        public GamePlayersParameters gamePlayersParameters;

        [HideInInspector] 
        public List<Unit> ownedProducingUnits = new List<Unit>();
        public bool gameIsPaused;
        private float _producingRate = 3f;
        private Coroutine _producingResourcesCoroutine = null;

        
        private Ray _ray;
        private RaycastHit _raycastHit;
        
        private void Awake()
        {
            DataHandler.LoadGameData();
			Globals.Nav_Mesh_Surface = GameObject.Find("Plane").GetComponent<NavMeshSurface>();
			Globals.UpdateNavMeshSurface();
            GetComponent<DayAndNightCycler>().enabled = gameGlobalParameters.enableDayAndNightCycle;
            GameObject.Find("FogOfWar").SetActive(gameGlobalParameters.enableFOV);
            GetStartPosition();
            gameIsPaused = false;
            fov.SetActive(gameGlobalParameters.enableFOV);
        }

        public void Start()
        {
            instance = this;
            _producingResourcesCoroutine = StartCoroutine("ProducingResources");
            
            GameParameters[] gameParametersList = Resources.LoadAll<GameParameters>("ScriptableObjects/Parameters");

            foreach (GameParameters parameters in gameParametersList)
            {
                Debug.Log(parameters.GetParametersName());
                Debug.Log("> Fields shown in-game:");
                foreach (string fieldName in parameters.FieldsToShowInGame)
                {
                    Debug.Log($"     {fieldName}");
                }
            }
        }

        private void Update()
        {
            if (gameIsPaused)
            {
                return;
            }
            
            CheckUnitsNavigation();
        }

        private void CheckUnitsNavigation()
        {
            if (Globals.Selected_Units.Count > 0 && Input.GetMouseButtonUp(1))
            {
                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out _raycastHit, 1000f, Globals.Terrain_Layer_Mask))
                {
                    foreach (UnitManager um in Globals.Selected_Units)
                    {
                        if (um.GetType() == typeof(CharacterManager))
                        {
                            ((CharacterManager)um).MoveTo(_raycastHit.point);
                        }
                    }
                }
            }
        }

        private void GetStartPosition()
        {
            startPosition = Utils.MiddleOfScreenPointToWorld();
        }

        private void OnEnable()
        {
            EventManager.AddListener("PauseGame", OnPauseGame);
            EventManager.AddListener("ResumeGame", OnResumeGame);
            EventManager.AddListener("UpdateGameParameter:enableDayAndNightCycle", OnUpdateDayAndNightCycle);
            EventManager.AddListener("UpdateGameParameter:enableFOV", OnUpdateFOV);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener("PauseGame", OnPauseGame);
            EventManager.RemoveListener("ResumeGame", OnResumeGame);
            EventManager.RemoveListener("UpdateGameParameter:enableDayAndNightCycle", OnUpdateDayAndNightCycle);
            EventManager.RemoveListener("UpdateGameParameter:enableFOV", OnUpdateFOV);
        }

        private void OnPauseGame()
        {
            gameIsPaused = true;
            if (_producingResourcesCoroutine != null)
            {
                StopCoroutine(_producingResourcesCoroutine);
                _producingResourcesCoroutine = null;
            }
        }

        private void OnResumeGame()
        {
            gameIsPaused = false;
            if (_producingResourcesCoroutine == null)
            {
                _producingResourcesCoroutine = StartCoroutine("ProducingResources");
            }
        }

        private void OnUpdateDayAndNightCycle(object data)
        {
            bool dayAndNightIsOn = (bool)data;
            GetComponent<DayAndNightCycler>().enabled = dayAndNightIsOn;
        }

        private void OnUpdateFOV(object data)
        {
            bool fovIsOn = (bool)data;
            fov.SetActive(fovIsOn);
        }

        private void OnApplicationQuit()
        {
            #if !UNITY_EDITOR
            DataHandler.SaveGameData();
            #endif
        }

        private IEnumerator ProducingResources()
        {
            while(true)
            {
                foreach (Unit unit in ownedProducingUnits)
                {
                    unit.ProduceResources();
                    EventManager.TriggerEvent("UpdateResourceTexts");
                    yield return new WaitForSeconds(_producingRate);
                }
            }
        }
        
        //public float producingRate = 3f;

       //public void Start()
       //{
       //    instance = this;
       //}

       //private void OnPauseGame()
       //{
       //    gameIsPaused = true;
       //}

       //private void OnResumeGame()
       //{
       //    gameIsPaused = false;
       //}
    }
}
