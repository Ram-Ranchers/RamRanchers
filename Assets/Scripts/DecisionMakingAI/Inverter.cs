using System.Collections.Generic;

namespace DecisionMakingAI
{
    public class Inverter : Node
    {
        public Inverter() : base() { }
        public Inverter(List<Node> children) : base(children) { }

        public override bool IsFlowNode => true;
        
        public override NodeState Evaluate()
        {
            if (!HasChildren)
            {
                return NodeState.Failure;
            }

            switch (_children[0].Evaluate())
            {
                case NodeState.Failure:
                    _state = NodeState.Success;
                    return _state;
                case NodeState.Success:
                    _state = NodeState.Failure;
                    return _state;
                case NodeState.Running:
                    _state = NodeState.Running;
                    return _state;
                default:
                    _state = NodeState.Failure;
                    return _state;
            }
        }
    }
}
