using UnityEngine;

namespace DecisionMakingAI
{
    public class CheckEnemyInAttackRange : Node
    {
        private Transform _transform;

       //private UnitManager _manager;
       //private float _attackRange;

       //public CheckEnemyInAttackRange(UnitManager manager) : base()
       //{
       //    _manager = manager;
       //    _attackRange = _manager.Unit.Data.attackRange;
       //}
       //
       //public override NodeState Evaluate()
       //{
       //    object currentTarget = _parent.GetData("currentTarget");
       //    if (currentTarget == null)
       //    {
       //        state = NodeState.Failure;
       //        return state;
       //    }

       //    Transform target = (Transform)currentTarget;
       //    if (!target)
       //    {
       //        _parent.ClearData("currentTarget");
       //        state = NodeState.Failure;
       //        return state;
       //    }

       //    Vector3 s = target.Find("Mesh").localScale;
       //    float targetSize = Mathf.Max(s.x, s.z);

       //    float d = Vector3.Distance(_manager.transform.position, target.position);
       //    bool isInRange = (d - targetSize) <= _attackRange;
       //    state = isInRange ? NodeState.Success : NodeState.Failure;
       //    return state;
       //}
        
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
