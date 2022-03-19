using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;

namespace DecisionMakingAI
{
    public class UiManager : MonoBehaviour
    {
        private BuildingPlacer _buildingPlacer;
        
        public Transform buildingMenu;
        public GameObject buildingButtonPrefab;
        public Transform resourcesUIParent;
        public GameObject gameResourcesDisplayPrefab;
        public GameObject gameResourceCostPrefab;
        public GameObject infoPanel;
        public Color invalidTextColour;
        public Transform selectedUnitsListParent;
        public GameObject selectedUnitDisplayPrefab;
        public Transform selectionGroupsParent;
        public GameObject selectedUnitMenu;
        public GameObject unitSkillButtonPrefab;
        public GameObject gameSettingsPanel;
        public Transform gameSettingsMenusParent;
        public Text gameSettingsContentName;
        public Transform gameSettingsContentParent;
        public GameObject gameSettingsMenuButtonPrefab;
        public GameObject gameSettingsParameterPrefab;
        public GameObject sliderPrefab;
        public GameObject togglePrefab;

        private Text _infoPanelTitleText;
        private Text _infoPanelDescriptionText;
        private Transform _infoPanelResourcesCostParent;
        private Dictionary<string, Text> _resourceTexts;
        private Dictionary<string, Button> _buildingButtons;
        private RectTransform _selectedUnitContentRectTransform;
        private RectTransform _selectedUnitButtonsRectTransform;
        private Text _selectedUnitTitleText;
        private Text _selectedUnitLevelText;
        private Transform _selectedUnitResourcesProductionParent;
        private Transform _selectedUnitActionButtonsParent;
        private Unit _selectedUnit;
        private Dictionary<string, GameParameters> _gameParameters;

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
                BuildingData data = Globals.Building_Data[i];
                GameObject button = Instantiate(buildingButtonPrefab, buildingMenu);
                button.name = data.unitname;
                button.transform.Find("Text").GetComponent<Text>().text = data.unitname;
                Button b = button.GetComponent<Button>();
                _buildingButtons[data.code] = b;
                AddBuildingButtonListener(b, i);
                button.GetComponent<BuildingButton>().Initialise(Globals.Building_Data[i]);
                
                if (!Globals.Building_Data[i].CanBuy())
                {
                    b.interactable = false;
                }
            }

            Transform infoPanelTransfrom = infoPanel.transform;
            _infoPanelTitleText = infoPanelTransfrom.Find("Content/Title").GetComponent<Text>();
            _infoPanelDescriptionText = infoPanelTransfrom.Find("Content/Description").GetComponent<Text>();
            _infoPanelResourcesCostParent = infoPanelTransfrom.Find("Content/ResourcesCost");
            ShowInfoPanel(false);

            for (int i = 1; i <= 9; i++)
            {
                ToggleSelectionGroupButton(i, false);
            }

            Transform selectedUnitMenuTransform = selectedUnitMenu.transform;
            _selectedUnitContentRectTransform = selectedUnitMenuTransform.Find("Content").GetComponent<RectTransform>();
            _selectedUnitButtonsRectTransform = selectedUnitMenuTransform.Find("Buttons").GetComponent<RectTransform>();
            _selectedUnitTitleText = selectedUnitMenuTransform.Find("Content/Title").GetComponent<Text>();
            _selectedUnitLevelText = selectedUnitMenuTransform.Find("Content/Level").GetComponent<Text>();
            _selectedUnitResourcesProductionParent = selectedUnitMenuTransform.Find("Content/ResourcesProduction");
            _selectedUnitActionButtonsParent = selectedUnitMenuTransform.Find("Buttons/SpecificActions");

            ShowSelectedUnitMenu(false);
            
            gameSettingsPanel.SetActive(false);
            GameParameters[] gameParametersList = Resources.LoadAll<GameParameters>("ScriptableObjects/Parameters");
            _gameParameters = new Dictionary<string, GameParameters>();
            foreach (GameParameters p in gameParametersList)
            {
                _gameParameters[p.GetParametersName()] = p;
                SetupGameSettingsPanel();
            }
        }

        private void SetupGameSettingsPanel()
        {
            GameObject g;
            string n;
            List<string> availableMenus = new List<string>();
            foreach (GameParameters parameters in _gameParameters.Values)
            {
                if (parameters.FieldsToShowInGame.Count == 0)
                {
                    continue;
                }

                g = GameObject.Instantiate(gameSettingsMenuButtonPrefab, gameSettingsMenusParent);
                n = parameters.GetParametersName();
                g.transform.Find("Text").GetComponent<Text>().text = n;
                AddGameSettingsPanelMenuListener(g.GetComponent<Button>(), n);
                availableMenus.Add(n);
            }

            if (availableMenus.Count > 0)
            {
                SetGameSettingsContent(availableMenus[0]);
            }
        }

        private void AddGameSettingsPanelMenuListener(Button b, string menu)
        {
            b.onClick.AddListener(() => SetGameSettingsContent(menu));
        }

        private void SetGameSettingsContent(string menu)
        {
            gameSettingsContentName.text = menu;

            foreach (Transform child in gameSettingsContentParent)
            {
                Destroy(child.gameObject);
            }

            GameParameters parameters = _gameParameters[menu];
            System.Type ParametersType = parameters.GetType();
            GameObject gWrapper, gEditor;
            RectTransform rtWrapper, rtEditor;
            int i = 0;
            float contentWidth = 534f;
            float parameterNameWidth = 200f;
            float fieldHeight = 32f;

            foreach (string fieldName in parameters.FieldsToShowInGame)
            {
                gWrapper = GameObject.Instantiate(gameSettingsParameterPrefab, gameSettingsContentParent);

                gEditor = null;
                FieldInfo field = ParametersType.GetField(fieldName);
                if (field.FieldType == typeof(bool))
                {
                    gEditor = Instantiate(togglePrefab);
                    Toggle t = gEditor.GetComponent<Toggle>();
                    t.isOn = (bool)field.GetValue(parameters);
                    t.onValueChanged.AddListener(delegate
                    {
                        OnGameSettingsToggleValueChanged(parameters, field, fieldName, t);
                    });
                }
                else if (field.FieldType == typeof(int) || field.FieldType == typeof(float))
                {
                    bool isRange = System.Attribute.IsDefined(field, typeof(RangeAttribute), false);
                    if (isRange)
                    {
                        RangeAttribute attr =
                            (RangeAttribute)System.Attribute.GetCustomAttribute(field, typeof(RangeAttribute));
                        gEditor = Instantiate(sliderPrefab);
                        Slider s = gEditor.GetComponent<Slider>();
                        s.minValue = attr.min;
                        s.maxValue = attr.max;
                        s.wholeNumbers = field.FieldType == typeof(int);
                        s.value = field.FieldType == typeof(int)
                            ? (int)field.GetValue(parameters)
                            : (float)field.GetValue(parameters);
                        s.onValueChanged.AddListener(delegate
                        {
                            OnGameSettingsSliderValueChanged(parameters, field, fieldName, s);
                        });
                    }
                }

                rtWrapper = gWrapper.GetComponent<RectTransform>();
                rtWrapper.anchoredPosition = new Vector2(0f, -i * fieldHeight);
                rtWrapper.sizeDelta = new Vector2(contentWidth, fieldHeight);

                if (gEditor != null)
                {
                    gEditor.transform.SetParent(gWrapper.transform);
                    rtEditor = gEditor.GetComponent<RectTransform>();
                    rtEditor.anchoredPosition = new Vector2((parameterNameWidth + 16f), 0f);
                    rtEditor.sizeDelta = new Vector2(rtWrapper.sizeDelta.x - (parameterNameWidth + 16f), fieldHeight);
                }

                i++;
            }

            RectTransform rt = gameSettingsContentParent.GetComponent<RectTransform>();
            Vector2 size = rt.sizeDelta;
            size.y = i * fieldHeight;
            rt.sizeDelta = size;
        }

        private void OnGameSettingsToggleValueChanged(GameParameters parameters, FieldInfo field, string gameParameter,
            Toggle change)
        {
            field.SetValue(parameters, change.isOn);
            EventManager.TriggerEvent($"UpdateGameParameter:{gameParameter}", change.isOn);
        }

        private void OnGameSettingsSliderValueChanged(GameParameters parameters, FieldInfo field, string gameParameter,
            Slider change)
        {
            if (field.FieldType == typeof(int))
            {
                field.SetValue(parameters, (int)change.value);
            }
            else
            {
                field.SetValue(parameters, change.value);
            }
            
            EventManager.TriggerEvent($"UpdateGameParameter:{gameParameter}", change.value);
        }
        
        public void ToggleSelectionGroupButton(int groupIndex, bool on)
        {
            selectionGroupsParent.Find(groupIndex.ToString()).gameObject.SetActive(on);
        }

        public void ToggleGameSettingsPanel()
        {
            bool showGameSettingsPanel = !gameSettingsPanel.activeSelf;
            gameSettingsPanel.SetActive(showGameSettingsPanel);
            EventManager.TriggerEvent(showGameSettingsPanel ? "PauseGame" : "ResumeGame");
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
                _buildingButtons[data.code].interactable = data.CanBuy();
            }
        }

        private void OnEnable()
        {
            EventManager.AddListener("UpdateResourceTexts", OnUpdateResourceTexts);
            EventManager.AddListener("CheckBuildingButtons", OnCheckBuildingButtons);
            EventManager.AddListener("HoverBuildingButton", OnHoverBuildingButton);
            EventManager.AddListener("UnhoverBuildingButton", OnUnhoverBuildingButton);
            EventManager.AddListener("SelectUnit", OnSelectUnit);
            EventManager.AddListener("DeselectUnit", OnDeselectUnit);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener("UpdateResourceTexts", OnUpdateResourceTexts);
            EventManager.RemoveListener("CheckBuildingButtons", OnCheckBuildingButtons);
            EventManager.RemoveListener("HoverBuildingButton", OnHoverBuildingButton);
            EventManager.RemoveListener("UnhoverBuildingButton", OnUnhoverBuildingButton);
            EventManager.RemoveListener("SelectUnit", OnSelectUnit);
            EventManager.RemoveListener("DeselectUnit", OnDeselectUnit);
        }

        private void OnSelectUnit(object data)
        {
            Unit unit = (Unit)data;
            AddSelectedUnitToUIList(unit);
            SetSelectedUnitMenu(unit);
            ShowSelectedUnitMenu(true);
        }
        
        private void OnDeselectUnit(object data)
        {
            Unit unit = (Unit)data;
            RemoveSelectedUnitFromUIList(unit.Code);
            if (Globals.Selected_Units.Count == 0)
            {
                ShowSelectedUnitMenu(false);
            }
            else
            {
                SetSelectedUnitMenu(Globals.Selected_Units[Globals.Selected_Units.Count - 1].Unit);
            }
        }

        private void SetSelectedUnitMenu(Unit unit)
        {
            // Adapt content panel heights to match info to display
            int contentHeight = 60 + unit.Production.Count * 16;
            _selectedUnitContentRectTransform.sizeDelta = new Vector2(64, contentHeight);
            _selectedUnitButtonsRectTransform.anchoredPosition = new Vector2(0, -contentHeight - 20);
            _selectedUnitButtonsRectTransform.sizeDelta = new Vector2(70, Screen.height - contentHeight - 20);
            
            // Update texts
            _selectedUnitTitleText.text = unit.Data.unitname;
            _selectedUnitLevelText.text = $"Level {unit.Level}";
            
            // Clear resource production and reinstantiate new one
            foreach (Transform child in _selectedUnitResourcesProductionParent)
            {
                Destroy(child.gameObject);
            }

            if (unit.Production.Count > 0)
            {
                GameObject g;
                Transform t;
                foreach (ResourceValue resource in unit.Production)
                {
                    g = GameObject.Instantiate(gameResourceCostPrefab, _selectedUnitResourcesProductionParent);
                    t = g.transform;
                    t.Find("Text").GetComponent<Text>().text = $"+{resource.amount}";
                    t.Find("Icon").GetComponent<Image>().sprite =
                        Resources.Load<Sprite>($"Textures/GameResources/{resource.code}");
                }
            }

            _selectedUnit = unit;
            
            // Clear skills and reinstantiate new ones
            foreach (Transform child in _selectedUnitActionButtonsParent)
            {
                Destroy(child.gameObject);
            }

            if (unit.SkillManagers.Count > 0)
            {
                GameObject g;
                Transform t;
                Button b;
                for (int i = 0; i < unit.SkillManagers.Count; i++)
                {
                    g = GameObject.Instantiate(unitSkillButtonPrefab, _selectedUnitActionButtonsParent);
                    t = g.transform;
                    b = g.GetComponent<Button>();
                    unit.SkillManagers[i].SetButton(b);
                    t.Find("Text").GetComponent<Text>().text = unit.SkillManagers[i].skill.skillName;
                    AddUnitSkillButtonListener(b, i);
                }
            }
        }

        private void AddUnitSkillButtonListener(Button b, int i)
        {
            b.onClick.AddListener(() => _selectedUnit.TriggerSkill(i));
        }
        
        private void ShowSelectedUnitMenu(bool show)
        {
            selectedUnitMenu.SetActive(show);
        }
        
        public void AddSelectedUnitToUIList(Unit unit)
        {
            Transform alreadyInstantiatedChild = selectedUnitsListParent.Find(unit.Code);
            if (alreadyInstantiatedChild != null)
            {
                Text t = alreadyInstantiatedChild.Find("Count").GetComponent<Text>();
                int count = int.Parse(t.text);
                t.text = (count + 1).ToString();
            }
            else
            {
                GameObject g = GameObject.Instantiate(selectedUnitDisplayPrefab, selectedUnitsListParent);
                g.name = unit.Code;
                Transform t = g.transform;
                t.Find("Count").GetComponent<Text>().text = "1";
                t.Find("Name").GetComponent<Text>().text = unit.Data.unitname;
            }
        }

        public void RemoveSelectedUnitFromUIList(string code)
        {
            Transform listItem = selectedUnitsListParent.Find(code);
            if (listItem == null)
            {
                return;
            }

            Text t = listItem.Find("Count").GetComponent<Text>();
            int count = int.Parse(t.text);
            count -= 1;

            if (count == 0)
            {
                DestroyImmediate(listItem.gameObject);
            }
            else
            {
                t.text = count.ToString();
            }
        }
        
        private void OnHoverBuildingButton(object data)
        {
            SetInfoPanel((UnitData)data);
            ShowInfoPanel(true);
        }

        private void OnUnhoverBuildingButton()
        {
            ShowInfoPanel(false);
        }

        public void SetInfoPanel(UnitData data)
        {
            // Update texts
            if (data.code != "")
            {
                _infoPanelTitleText.text = data.code;
            }

            if (data.description != "")
            {
                _infoPanelDescriptionText.text = data.description;
            }
            
            // Clear resource costs and reinstantiate new ones
            foreach (Transform child in _infoPanelResourcesCostParent)
            {
                Destroy(child.gameObject);
            }

            if (data.cost.Count > 0)
            {
                GameObject g; Transform t;
                foreach (ResourceValue resource in data.cost)
                {
                    g = GameObject.Instantiate(gameResourceCostPrefab, _infoPanelResourcesCostParent);
                    t = g.transform;
                    t.Find("Text").GetComponent<Text>().text = resource.amount.ToString();
                    t.Find("Icon").GetComponent<Image>().sprite =
                        Resources.Load<Sprite>($"Textures/GameResources/{resource.code}");
                    if (Globals.Game_Resources[resource.code].Amount < resource.amount)
                    {
                        t.Find("Text").GetComponent<Text>().color = invalidTextColour;
                    }
                }
            }
        }

        public void ShowInfoPanel(bool show)
        {
            infoPanel.SetActive(show);
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
