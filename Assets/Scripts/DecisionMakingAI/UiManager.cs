using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DecisionMakingAI
{
    public class UiManager : MonoBehaviour
    {
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
