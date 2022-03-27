namespace DecisionMakingAI
{
    public class CheckHasDestination : Node
    {
        public override NodeState Evaluate()
        {
            object destinationPoint = GetData("destinationPoint");
            if (destinationPoint == null)
            {
                _state = NodeState.Failure;
                return _state;
            }

            _state = NodeState.Success;
            return _state;
        }
    }
}
