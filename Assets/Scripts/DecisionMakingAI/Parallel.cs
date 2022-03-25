using System.Collections.Generic;

namespace DecisionMakingAI
{
    public class Parallel : Node
    {
        public Parallel() : base() {}
        public Parallel(List<Node> children) : base(children) {}

        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;
            int nFailedChildren = 0;

            foreach (Node node in children)
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
                        state = NodeState.Success;
                        return state;
                }
            }

            if (nFailedChildren == children.Count)
            {
                state = NodeState.Failure;
            }
            else
            {
                state = anyChildIsRunning ? NodeState.Running : NodeState.Success;
            }
            return state;
        }
    }
}
