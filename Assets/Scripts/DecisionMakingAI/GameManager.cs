using System;
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
        
        [HideInInspector] 
        public bool gameIsPaused;
        
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
        }

        private void OnResumeGame()
        {
            gameIsPaused = false;
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
