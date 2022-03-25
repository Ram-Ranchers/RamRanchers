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
                state = NodeState.Failure;
                return state;
            }

            if (!(Transform) currentTarget)
            {
                _parent.ClearData("currentTarget");
                state = NodeState.Failure;
                return state;
            }

            state = NodeState.Success;
            return state;
        }
    }
}
