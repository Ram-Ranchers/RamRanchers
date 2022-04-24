using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace DecisionMakingAI
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public GameGlobalParameters gameGlobalParameters;
        public GameObject fov;
        public GamePlayersParameters gamePlayersParameters;

        [HideInInspector]
        public bool gameIsPaused;
        public Vector3 startPosition;
        public float _producingRate = 3f;

        private void Awake()
        {
            // load building data
            Globals.Building_Data =
                Resources.LoadAll<BuildingData>("ScriptableObjects/Units/Buildings") as BuildingData[];
            GetComponent<DayAndNightCycler>().enabled = gameGlobalParameters.enableDayAndNightCycle;
			Globals.Nav_Mesh_Surface = GameObject.Find("Plane").GetComponent<NavMeshSurface>();
			Globals.UpdateNavMeshSurface();
            fov.SetActive(gameGlobalParameters.enableFOV);
            GetStartPosition();
            gameIsPaused = false;
        }

        public void Start()
        {
            instance = this;
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

        private void OnApplicationQuit()
        {
            #if !UNITY_EDITOR
            DataHandler.SaveGameData();
            #endif
        }
    }
}
