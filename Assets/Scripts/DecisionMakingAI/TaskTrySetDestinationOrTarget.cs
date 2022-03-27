using UnityEngine;

namespace DecisionMakingAI
{
    public class TaskTrySetDestinationOrTarget : Node
    {
      private CharacterManager _manager;
      
      private Ray _ray;
      private RaycastHit _raycastHit;
      
      public TaskTrySetDestinationOrTarget(CharacterManager manager) : base()
      {
          _manager = manager;
      }
      
      public override NodeState Evaluate()
      {
          if (_manager.IsSelected && Input.GetMouseButtonUp(1))
          {
              _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

              if (Physics.Raycast(_ray, out _raycastHit, 1000f, Globals.Unit_Mask))
              {
                  UnitManager um = _raycastHit.collider.GetComponent<UnitManager>();
                  if (um != null)
                  {
                      _parent._parent.SetData("currentTarget", _raycastHit.transform);
                      ClearData("destinationPoint");
                      _state = NodeState.Success;
                      return _state;
                  }
              }
              else if (Physics.Raycast(_ray, out _raycastHit, 1000f, Globals.Terrain_Layer_Mask))
              {
                  ClearData("currentTarget");
                  _parent._parent.SetData("destinationPoint", _raycastHit.point);
                  _state = NodeState.Success;
                  return _state;
              }
          }
      
          _state = NodeState.Failure;
          return _state;
      }
    }
}
