using UnityEngine;

namespace DecisionMakingAI
{
    public class CheckHasDestination : Node
    {
        public override NodeState Evaluate()
        {
            object destinationPoint = GetData("destinationPoint");
            if (destinationPoint == null)
            {
                state = NodeState.Failure;
                return state;
            }

            state = NodeState.Success;
            return state;
        }
    }
}
