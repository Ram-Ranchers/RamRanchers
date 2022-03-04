using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DecisionMakingAI
{
    public class BuildingPlacer : MonoBehaviour
    {
        private Building _placedBuilding = null;
        private UiManager _uiManager;
        
        private Ray _ray;
        private RaycastHit _raycastHit;
        private Vector3 _lastPlacementPosition;

        private void Awake()
        {
            _uiManager = GetComponent<UiManager>();
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

                if (_placedBuilding.HasValidPlacement && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
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
                _placedBuilding = null;
            }
            _uiManager.UpdateResourceTexts();
            _uiManager.CheckBuildingButtons();
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
