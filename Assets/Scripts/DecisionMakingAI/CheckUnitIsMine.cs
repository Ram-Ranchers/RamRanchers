namespace DecisionMakingAI
{
    public class CheckUnitIsMine : Node
    {
        private bool _unitIsMine;

        //public CheckUnitIsMine(UnitManager manager) : base()
        //{
        //    _unitIsMine = manager.Unit.Owner == GameManagerDependencyInfo.instance.gamePlayersParameters.myPlayerId;
        //}

        public override NodeState Evaluate()
        {
            state = _unitIsMine ? NodeState.Success : NodeState.Failure;
            return state;
        }
        
    }
}
