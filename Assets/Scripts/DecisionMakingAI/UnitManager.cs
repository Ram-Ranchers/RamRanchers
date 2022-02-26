using UnityEngine;

namespace DecisionMakingAI
{
    public class UnitManager : MonoBehaviour
    {
        private bool _selected = false;
        public bool IsSelected => _selected;

        public void Deselect()
        {
            _selected = false;
        }

        private void SelectUtil()
        {
            _selected = true;
        }
    }
}
