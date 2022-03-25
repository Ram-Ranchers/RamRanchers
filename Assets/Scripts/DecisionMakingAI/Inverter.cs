using System.Collections.Generic;

namespace DecisionMakingAI
{
    public class Inverter : Node
    {
        public Inverter() : base() { }
        public Inverter(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            if (!HasChildren)
            {
                return NodeState.Failure;
            }

            switch (children[0].Evaluate())
            {
                case NodeState.Failure:
                    state = NodeState.Success;
                    return state;
                case NodeState.Success:
                    state = NodeState.Failure;
                    return state;
                case NodeState.Running:
                    state = NodeState.Running;
                    return state;
                default:
                    state = NodeState.Failure;
                    return state;
            }
        }
    }
}
