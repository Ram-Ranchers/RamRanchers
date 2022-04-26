using UnityEngine;

namespace DecisionMakingAI
{
    public class TaskFollow : Node
    {
      private CharacterManager _manager;
      private Vector3 _lastTargetPosition;
      private PathfindingUnit _agent;
      
      public TaskFollow(CharacterManager manager) : base()
      {
          _manager = manager;
          _lastTargetPosition = Vector3.zero;
      }

      public override NodeState Evaluate()
      {
          object currentTarget = GetData("currentTarget");
          Vector3 targetPosition = GetTargetPosition((Transform)currentTarget);

          if (targetPosition != _lastTargetPosition)
          {
              _manager.MoveTo(targetPosition, false);
              _lastTargetPosition = targetPosition;
          }

          float d = Vector3.Distance(_manager.transform.position,  _manager._agent.target);
          if (d <= _manager._agent.speed - 5)
          {
              ClearData("currentTarget");
              _state = NodeState.Success;
              return _state;
          }

          _state = NodeState.Running;
          return _state;
      }

      private Vector3 GetTargetPosition(Transform target)
      {
          Vector3 s = target.Find("Mesh").localScale;
          float targetSize = Mathf.Max(s.x, s.z);

          Vector3 p = _manager.transform.position;
          Vector3 t = target.position - p;
          
          float d = targetSize + _manager.Unit.Data.attackRange - 0.2f;
          float r = d / t.magnitude;
          return p + t * (1 - r);
      }
    }
}
