using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DecisionMakingAI
{
    public class UnitSelection : MonoBehaviour
    {
        private bool _isDraggingMouseBox = false;
        private Vector3 _dragStartPosition;
        private Ray _ray;
        private RaycastHit _raycastHit;
        private Dictionary<int, List<UnitManager>> _selectionGroups = new Dictionary<int, List<UnitManager>>();

        public UiManager UiManager;
        
        private void Update()
        {
            if (GameManager.instance.gameIsPaused)
            {
                return;
            }
            
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                _isDraggingMouseBox = true;
                _dragStartPosition = Input.mousePosition;
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                _isDraggingMouseBox = false;
            }
            
            if (_isDraggingMouseBox && _dragStartPosition != Input.mousePosition)
            {
                SelectUnitsInDraggingBox();
            }
            
            if (Globals.Selected_Units.Count > 0)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    DeselectAllUnits();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(_ray, out _raycastHit, 1000f))
                    {
                        if (_raycastHit.transform.tag == "Terrain")
                        {
                            DeselectAllUnits();
                        }
                    }
                }
            }

            if (Input.anyKeyDown)
            {
                int alphaKey = Utils.GetAlphaKeyValue(Input.inputString);
                if (alphaKey != -1)
                {
                    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftApple) || Input.GetKey(KeyCode.RightApple) || Input.GetKey(KeyCode.G))
                    {
                        CreatSelectionGroup(alphaKey);
                    }
                    else
                    {
                        ReselectGroup(alphaKey);
                    }
                }
            }
        }

        public void SelectUnitsGroup(int groupIndex)
        {
            ReselectGroup(groupIndex);
        }

        private void CreatSelectionGroup(int groupIndex)
        {
            if (Globals.Selected_Units.Count == 0)
            {
                if (_selectionGroups.ContainsKey(groupIndex))
                {
                    RemoveSelectionGroup(groupIndex);
                }

                return;
            }

            List<UnitManager> groupUnits = new List<UnitManager>(Globals.Selected_Units);
            _selectionGroups[groupIndex] = groupUnits;
            UiManager.ToggleSelectionGroupButton(groupIndex, true);
        }

        private void RemoveSelectionGroup(int groupIndex)
        {
            _selectionGroups.Remove(groupIndex);
            UiManager.ToggleSelectionGroupButton(groupIndex, false);
        }

        private void ReselectGroup(int groupIndex)
        {
            if (!_selectionGroups.ContainsKey(groupIndex))
            {
                return;
            }
            
            DeselectAllUnits();
            
            foreach (UnitManager um in _selectionGroups[groupIndex])
            {
                um.Select();
            }
        }

        private void DeselectAllUnits()
        {
            List<UnitManager> selectedUnits = new List<UnitManager>(Globals.Selected_Units);
            foreach (UnitManager um in selectedUnits)
            {
                um.Deselect();
            }
        }
        
        private void SelectUnitsInDraggingBox()
        {
            Bounds selectionBounds = Utils.GetViewportBounds(Camera.main, _dragStartPosition, Input.mousePosition);
            GameObject[] selectedUnits = GameObject.FindGameObjectsWithTag("Unit");
            bool inBounds;
            foreach (GameObject unit in selectedUnits)
            {
                inBounds = selectionBounds.Contains(Camera.main.WorldToViewportPoint(unit.transform.position));
                if (inBounds)
                {
                    unit.GetComponent<UnitManager>().Select();
                }
                else
                {
                    unit.GetComponent<UnitManager>().Deselect();
                }
            }
        }
        
        private void OnGUI()
        {
            if (_isDraggingMouseBox)
            {
                // Create a rect from both mouse positions
                var rect = Utils.GetScreenRect(_dragStartPosition, Input.mousePosition);
                Utils.DrawScreenRect(rect, new Color(0.5f, 1f, 0.4f, 0.2f));
                Utils.DrawScreenRectBorder(rect, 1, new Color(0.5f, 1f, 0.4f));
            }
        }
    }
}
