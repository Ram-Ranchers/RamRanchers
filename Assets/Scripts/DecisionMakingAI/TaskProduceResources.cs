namespace DecisionMakingAI
{
    public class TaskProduceResources : Node
    {
        private Unit _unit;

        public TaskProduceResources(UnitManager manager) : base()
        {
            _unit = manager.Unit;
        }

        public override NodeState Evaluate()
        {
            _unit.ProduceResources();
            _state = NodeState.Success;
            return _state;
        }
    }
}
