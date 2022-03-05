using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace DecisionMakingAI
{
    public class UiManager : MonoBehaviour
    {
        private BuildingPlacer _buildingPlacer;
        
        public Transform buildingMenu;
        public GameObject buildingButtonPrefab;
        public Transform resourcesUIParent;
        public GameObject gameResourcesDisplayPrefab;

        private Dictionary<string, Text> _resourceTexts;
        private Dictionary<string, Button> _buildingButtons;

        private void Awake()
        {
            _resourceTexts = new Dictionary<string, Text>();
            foreach (KeyValuePair<string, GameResource> pair in Globals.Game_Resources)
            {
                GameObject display = Instantiate(gameResourcesDisplayPrefab, resourcesUIParent);
                display.name = pair.Key;
                _resourceTexts[pair.Key] = display.transform.Find("Text").GetComponent<Text>();
                SetResourceText(pair.Key, pair.Value.Amount);
            }

            _buildingButtons = new Dictionary<string, Button>();
            _buildingPlacer = GetComponent<BuildingPlacer>();

            for (int i = 0; i < Globals.Building_Data.Length; i++)
            {
                GameObject button = Instantiate(buildingButtonPrefab, buildingMenu);
                string code = Globals.Building_Data[i].Code;
                button.name = code;
                button.transform.Find("Text").GetComponent<Text>().text = code;
                Button b = button.GetComponent<Button>();
                _buildingButtons[code] = b;
                if (!Globals.Building_Data[i].CanBuy())
                {
                    b.interactable = false;
                }
                AddBuildingButtonListener(b, i);
            }
        }

        private void SetResourceText(string resource, int value)
        {
            _resourceTexts[resource].text = value.ToString();
        }

        public void OnUpdateResourceTexts()
        {
            foreach (KeyValuePair<string, GameResource> pair in Globals.Game_Resources)
            {
                SetResourceText(pair.Key, pair.Value.Amount);
            }
        }
        
        private void AddBuildingButtonListener(Button b, int i)
        {
            b.onClick.AddListener(() => _buildingPlacer.SelectPlacedBuilding(i));
        }

        public void OnCheckBuildingButtons()
        {
            foreach (BuildingData data in Globals.Building_Data)
            {
                _buildingButtons[data.Code].interactable = data.CanBuy();
            }
        }

        private void OnEnable()
        {
            EventManager.AddListener("UpdateResourceTexts", OnUpdateResourceTexts);
            EventManager.AddListener("CheckBuildingButtons", OnCheckBuildingButtons);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener("UpdateResourceTexts", OnUpdateResourceTexts);
            EventManager.RemoveListener("CheckBuildingButtons", OnCheckBuildingButtons);
        }

        //[Header("Unit Selection")] 
        //public GameObject selecteUnitMenuUpgradeButton;
        //public GameObject selecteUnitMenuDestroyButton;

        //public void SetSelectedUnitMenu(Unit unit)
        //{
        //    _selectedUnit = unit;

        //    bool unitIsMine = unit.Owner == GameManager.instance.gamePlayersParameters.myPlayerId;

        //    int contentHeight = unitIsMine ? 60 + unit.Production.count * 16 : 60;
        //    _selectedUnitContentRectTransform.sizeDelta = new Vector2(64, contentHeight);
        //    _selectedUnitButtonsRectTransform.anchordPosition = new Vector2(0, -contentHeight - 20);
        //    _selectedUnitButtonsRectTransform.sizeDelta = new Vector2(70, Screen.height - contentHeight - 20);

        //    _selectedUnitTitleText.text = unit.Data.unitName;
        //    _selectedUnitLevelText.text = $"Level {unit.Level}";

        //    foreach (Transform child in _selectedUnitResourcesProductionParent)
        //    {
        //        Destroy(child.gameObject);
        //    }

        //    if (unitIsMine && unit.Production.count > 0)
        //    {
        //        GameObject g;
        //        Transform t;
        //        foreach (KeyValuePair<InGameREsource, int> resource in unit.Production)
        //        {
        //            g = Instantiate(gameResourceCostPrefab);
        //            t = g.transform;
        //            t.Find("Text").GetComponent<Text>().text = $"+{resource.Value}";
        //            t.Find("Icon").GetComponent<Image>().sprite =
        //                Resources.Load<Sprite>($"Textures/GameResources/{resource.Key}");
        //            t.SetParent(_selectedUnitActionButtonsParent);
        //            AddUnitSkillButtonListener(b, i);
        //        }
        //    }

        //    selecteUnitMenuUpgradeButton.SetActive(unitIsMine);
        //    selecteUnitMenuDestroyButton.SetActive(unitIsMine);
        // }
    }
}
