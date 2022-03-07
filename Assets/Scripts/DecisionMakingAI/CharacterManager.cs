using UnityEngine;
using UnityEngine.AI;

namespace DecisionMakingAI
{
    public class CharacterManager : UnitManager
    {
        public NavMeshAgent agent;
        
        private Character _character;

        public override Unit Unit
        {
            get { return _character; }
            set { _character = value is Character ? (Character)value : null; }
        }

        public void MoveTo(Vector3 targetPosition)
        {
            agent.destination = targetPosition;
        }
    }
}
