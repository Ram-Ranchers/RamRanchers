using UnityEngine;

namespace DecisionMakingAI
{
    public class CheckHasTarget : Node
    {
        public override NodeState Evaluate()
        {
            object currentTarget = _parent.GetData("currentTarget");
            if (currentTarget == null)
            {
                _state = NodeState.Failure;
                return _state;
            }

            if (!(Transform) currentTarget)
            {
                _parent.ClearData("currentTarget");
                _state = NodeState.Failure;
                return _state;
            }

            _state = NodeState.Success;
            return _state;
        }
    }
}
