using UnityEngine;

namespace DecisionMakingAI
{
    public class CheckEnemyInAttackRange : Node
    {
        private UnitManager _manager;
        private float _attackRange;

       public CheckEnemyInAttackRange(UnitManager manager) : base()
       {
           _manager = manager;
           _attackRange = _manager.Unit.Data.attackRange;
       }
       
       public override NodeState Evaluate()
       {
           object currentTarget = _parent.GetData("currentTarget");
           if (currentTarget == null)
           {
               _state = NodeState.Failure;
               return _state;
           }

           Transform target = (Transform)currentTarget;
           if (!target)
           {
               _parent.ClearData("currentTarget");
               _state = NodeState.Failure;
               return _state;
           }

           Vector3 s = target.Find("Mesh").localScale;
           float targetSize = Mathf.Max(s.x, s.z);

           float d = Vector3.Distance(_manager.transform.position, target.position);
           bool isInRange = (d - targetSize) < _attackRange + 2;
           _state = isInRange ? NodeState.Success : NodeState.Failure;
           return _state;
       }
    }
}
