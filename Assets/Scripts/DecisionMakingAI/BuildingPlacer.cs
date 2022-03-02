using System;
using UnityEngine;

namespace DecisionMakingAI
{
    public class BuildingPlacer : MonoBehaviour
    {
        private Building _placedBuilding = null;

        private Ray _ray;
        private RaycastHit _raycastHit;
        private Vector3 _lastPlacementPosition;
        
        private void Start()
        {
            PreparePlacedBuilding(0);
        }

        private void Update()
        {
            if (_placedBuilding != null)
            {
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    CancelPlaceBuilding();
                    return;
                }
            }
        }

        void PreparePlacedBuilding(int buildingDataIndex)
        {
            if (_placedBuilding !=  null)
            {
                Destroy(_placedBuilding.Transform.gameObject);
            }

            Building building = new Building(Globals.Building_Data[buildingDataIndex]);
            _placedBuilding = building;
            _lastPlacementPosition = Vector3.zero;
        }

        void CancelPlaceBuilding()
        {
            Destroy(_placedBuilding.Transform.gameObject);
            _placedBuilding = null;
        }
        
        //private void Start()
        //{
        //    SpawnBuilding(GameManager.instance.gameGlobalParameters.initialBuilding,
        //        GameManager.instance.gamePlayersParameters.myPlayerId, GameManager.instance.startPosition);
//
        //    SpawnBuilding(GameManager.instance.gameGlobalParameters.initialBuilding,
        //        1 - GameManager.instance.gamePlayersParameters.myPlayerId,
        //        GameManager.instance.startPosition + new Vector3(-32f, 0f, 0f));
        //}
    }
}
