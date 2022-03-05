using UnityEngine;
using UnityEngine.EventSystems;

namespace DecisionMakingAI
{
    public class BuildingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private BuildingData _buildingData;

        public void Initialise(BuildingData buildingData)
        {
            _buildingData = buildingData;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EventManager.TriggerTypedEvent("HoverBuildingButton", new CustomEventData(_buildingData));
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            EventManager.TriggerEvent("UnhoverBuildingButton");
        }
    }
}
