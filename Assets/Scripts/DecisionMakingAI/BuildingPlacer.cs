using System;
using System.Collections.Generic;
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

        private Grid3 grid;

        private void Start()
        {
            SpawnBuilding(GameManager.instance.gameGlobalParameters.initialBuilding,
                GameManager.instance.gamePlayersParameters.myPlayerId, new Vector3(40, 0, -40));

            SpawnBuilding(GameManager.instance.gameGlobalParameters.initialBuilding,
                0, new Vector3(-40, 0, 40));
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

                if (_lastPlacementPosition != _raycastHit.point)
                {
                    _placedBuilding.CheckValidPlacement();
                    Dictionary<InGameResource, int> prod = _placedBuilding.ComputeProduction();
                    EventManager.TriggerEvent("UpdatePlacedBuildingProduction",
                        new object[] { prod, _raycastHit.point });
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

            Building building = new Building(Globals.Building_Data[buildingDataIndex],
                GameManager.instance.gamePlayersParameters.myPlayerId);
            _placedBuilding = building;
            _lastPlacementPosition = Vector3.zero;
            EventManager.TriggerEvent("PlaceBuildingOn");
        }

        public void SelectPlacedBuilding(int buildingDataIndex)
        {
            PreparePlacedBuilding(buildingDataIndex);
        }
        
        void PlaceBuilding(bool canChain = true)
        {
            grid = GameObject.Find("Plane").GetComponent<Grid3>();

            _placedBuilding.ComputeProduction();
            _placedBuilding.Place();
            if (canChain)
            {
                if (_placedBuilding.CanBuy())
                {
                    PreparePlacedBuilding(_placedBuilding.DataIndex);
                }
                else
                {
                    EventManager.TriggerEvent("PlaceBuildingOff");
                    _placedBuilding = null;
                } 
            }

            grid.UpdateGrid();

            EventManager.TriggerEvent("UpdateResourceTexts");
            EventManager.TriggerEvent("CheckBuildingButtons");

            EventManager.TriggerEvent("PlaySoundByName", "onBuildingPlacedSound");
        }
        
        void CancelPlaceBuilding()
        {
            Destroy(_placedBuilding.Transform.gameObject);
            _placedBuilding = null;
        }

        public void SpawnBuilding(BuildingData data, int owner, Vector3 position)
        {
            SpawnBuilding(data, owner, position, new List<ResourceValue>() {});
        }

        public void SpawnBuilding(BuildingData data, int owner, Vector3 position, List<ResourceValue> production)
        {
            Building prevPlacedBuilding = _placedBuilding;

            _placedBuilding = new Building(data, owner, production);
            _placedBuilding.SetPosition(position);
            PlaceBuilding(false);

            _placedBuilding = prevPlacedBuilding;
        }
    }
}
