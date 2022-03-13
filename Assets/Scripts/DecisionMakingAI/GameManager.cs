using Unity.AI.Navigation;
using UnityEngine;

namespace DecisionMakingAI
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public Vector3 startPosition;
        public GameParameters gameParameters;
        
        private Ray _ray;
        private RaycastHit _raycastHit;
        
        private void Awake()
        {
            DataHandler.LoadGameData();
			Globals.Nav_Mesh_Surface = GameObject.Find("Plane").GetComponent<NavMeshSurface>();
			Globals.UpdateNavMeshSurface();
            GetComponent<DayAndNightCycler>().enabled = gameParameters.enableDayAndNightCycle;
            GetStartPosition();
            GameObject.Find("FogOfWar").SetActive(gameParameters.enableFOV);
        }

        public void Start()
        {
            instance = this;
        }

        private void Update()
        {
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
