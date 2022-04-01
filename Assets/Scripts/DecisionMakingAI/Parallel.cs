using System.Collections.Generic;

namespace DecisionMakingAI
{
    public class Parallel : Node
    {
        public Parallel() : base() {}
        public Parallel(List<Node> children) : base(children) {}

        public override bool IsFlowNode => true;
        
        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;
            int nFailedChildren = 0;

            foreach (Node node in _children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        nFailedChildren++;
                        continue;
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        _state = NodeState.Success;
                        return _state;
                }
            }

            if (nFailedChildren == _children.Count)
            {
                _state = NodeState.Failure;
            }
            else
            {
                _state = anyChildIsRunning ? NodeState.Running : NodeState.Success;
            }
            return _state;
        }
    }
}
