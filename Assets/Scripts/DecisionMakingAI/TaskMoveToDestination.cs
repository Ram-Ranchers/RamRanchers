using UnityEngine;

namespace DecisionMakingAI
{
    public class TaskMoveToDestination : Node
    {
    //   private CharacterManager _manager;

    //   public TaskMoveToDestination(CharacterManager manager) : base()
    //   {
    //       _manager = manager;
    //   }

    //   public override NodeState Evaluate()
    //   {
    //       object destinationPoint = GetData("destinationPoint");
    //       Vector3 destination = (Vector3)destinationPoint;

    //       if (Vector3.Distance(destination, _manager.agent.destination) > 0.2f)
    //       {
    //           bool canMove = _manager.MoveTo(destination);
    //           state = canMove ? NodeState.Success : NodeState.Failure;
    //           return state;
    //       }

    //       float d = Vector3.Distance(_manager.transform.position, _manager.agent.destination);
    //       if (d <= _manager.agent.stoppingDistance)
    //       {
    //           ClearData("destinationPoint");
    //           state = NodeState.Success;
    //           return state;
    //       }

    //       state = NodeState.Running;
    //       return state;
    //   }
    }
}
