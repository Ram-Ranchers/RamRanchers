using System;
using UnityEngine;
using UnityEngine.EventSystems;

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
            _placedBuilding = new Building(GameManager.instance.gameGlobalParameters.initialBuilding);
            _placedBuilding.SetPosition(GameManager.instance.startPosition);
            
            _placedBuilding.Transform.GetComponent<BuildingManager>().Initialise(_placedBuilding);
            PlaceBuilding();
            
            CancelPlaceBuilding();
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

                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out _raycastHit, 1000f, Globals.Terrain_Layer_Mask))
                {
                    _placedBuilding.SetPosition(_raycastHit.point);
                    if (_lastPlacementPosition != _raycastHit.point)
                    {
                        _placedBuilding.CheckValidPlacement();
                    }

                    _lastPlacementPosition = _raycastHit.point;
                }

                if (_placedBuilding.HasValidPlacement && Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    PlaceBuilding();
                }
            }
        }

        void PreparePlacedBuilding(int buildingDataIndex)
        {
            if (_placedBuilding !=  null && !_placedBuilding.IsFixed)
            {
                Destroy(_placedBuilding.Transform.gameObject);
            }

            Building building = new Building(Globals.Building_Data[buildingDataIndex]);
            building.Transform.GetComponent<BuildingManager>().Initialise(building);
            _placedBuilding = building;
            _lastPlacementPosition = Vector3.zero;
        }

        public void SelectPlacedBuilding(int buildingDataIndex)
        {
            PreparePlacedBuilding(buildingDataIndex);
        }
        
        void PlaceBuilding()
        {
            _placedBuilding.Place();
            if (_placedBuilding.CanBuy())
            {
                PreparePlacedBuilding(_placedBuilding.DataIndex);
            }
            else
            {
				EventManager.TriggerEvent("PlaceBuildingOff");
                _placedBuilding = null;
            }
            EventManager.TriggerEvent("UpdateResourceTexts");
            EventManager.TriggerEvent("CheckBuildingButtons");
			
			// Update the dynamic nav mesh
			Globals.UpdateNavMeshSurface();
            
            EventManager.TriggerEvent("PlaySoundByName", "onBuildingPlacedSound");
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
