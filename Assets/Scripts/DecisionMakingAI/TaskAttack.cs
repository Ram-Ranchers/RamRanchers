using UnityEngine;

namespace DecisionMakingAI
{
    public class TaskAttack : Node
    {
        private UnitManager _manager;

        public TaskAttack(UnitManager manager) : base()
        {
            _manager = manager;
        }

        public override NodeState Evaluate()
        {
            object currentTarget = GetData("currentTarget");
            _manager.Attack((Transform)currentTarget);
            _state = NodeState.Success;
            return _state;
        }
    }
}