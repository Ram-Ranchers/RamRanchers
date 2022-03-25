using UnityEngine;

namespace DecisionMakingAI
{
    public class TaskTrySetDestinationOrTarget : Node
    {
      // private CharacterManager _manager;
      // 
      // private Ray _ray;
      // private RaycastHit _raycastHit;
      // 
      // public TaskTrySetDestination(CharacterManager manager) : base()
      // {
      //     _manager = manager;
      // }
      // 
      // public override NodeState Evaluate()
      // {
      //     if (_manager.IsSelected && Input.GetMouseButtonUp(1))
      //     {
      //         _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      //         if (Physics.Raycast(_ray, out _raycastHit, 1000f, Globals.Unit_Mask))
      //         {
      //             UnitManager um = _raycastHit.collider.GetComponent<UnitManager>();
      //             if (um != null)
      //             {
      //                 _parent._parent.SetData("currentTarget", _raycastHit.transform);
      //                 ClearData("destinationPoint");
      //                 state = NodeState.Success;
      //                 return state;
      //             }
      //         }
      //         else if (Physics.Raycast(_ray, out _raycastHit, 1000f, Globals.Terrain_Layer_Mask))
      //         {
      //             ClearData("currentTarget");
      //             _parent._parent.SetData("destinationPoint", _raycastHit.point);
      //             state = NodeState.Success;
      //             return state;
      //         }
      //     }
      // 
      //     state = NodeState.Failure;
      //     return state;
      // }
    }
}
