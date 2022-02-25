using UnityEngine;

namespace DecisionMakingAI
{
    public class CheckEnemyInFOVRange : Node
    {
        private static int _enemyLayerMask = 1 << 6;

        private Transform _transform;

        public CheckEnemyInFOVRange(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                Collider[] colliders =
                    Physics.OverlapSphere(_transform.position, GuardBT.fovRange, _enemyLayerMask);
                if (colliders.Length > 0)
                {
                    _parent._parent.SetData("target", colliders[0].transform);
                    
                    state = NodeState.Success;
                    return state;
                }
                
                state = NodeState.Failure;
                return state;
            }

            state = NodeState.Success;
            return state;
        }
    }
}
