using UnityEngine;

namespace DecisionMakingAI
{
    public class CharacterManager : UnitManager
    {
        public PathfindingUnit _agent;
        
        private Character _character;

        public override Unit Unit
        {
            get { return _character; }
            set { _character = value is Character ? (Character)value : null; }
        }

        private void Start()
        {
            _character.Place();
        }

        public bool MoveTo(Vector3 targetPosition, bool playSound = true)
        {
            _agent.target = targetPosition;

            return true;
        }
    }
}
