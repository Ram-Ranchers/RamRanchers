using UnityEngine;

namespace DecisionMakingAI
{
    public class CheckEnemyInAttackRange : Node
    {
        private Transform _transform;

        public CheckEnemyInAttackRange(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                state = NodeState.Failure;
                return state;
            }

            Transform target = (Transform)t;
            if (Vector3.Distance(_transform.position, target.position) <= GuardBT.attackRange)
            {
                state = NodeState.Success;
                return state;
            }
            
            state = NodeState.Failure;
            return state;
        }
    }
}
