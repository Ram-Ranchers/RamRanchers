using UnityEngine;

namespace DecisionMakingAI
{
    public class TaskMoveToDestination : Node
    {
        private CharacterManager _manager;
        
        public TaskMoveToDestination(CharacterManager manager) : base()
        {
            _manager = manager;
        }

        public override NodeState Evaluate()
        {
            object destinationPoint = GetData("destinationPoint");
            Vector3 destination = (Vector3)destinationPoint;

            if (Vector3.Distance(destination, _manager._agent.target) > 0.2f)
            { 
                bool canMove = _manager.MoveTo(destination);
                _state = canMove ? NodeState.Success : NodeState.Failure;
                return _state;
            }

            float d = Vector3.Distance(_manager.transform.position, _manager._agent.target);
            if (d <= _manager._agent.speed - 5)
            {
                ClearData("destinationPoint");
                _state = NodeState.Success;
                return _state;
            }

            _state = NodeState.Running;
            return _state;
        }
    }
}
