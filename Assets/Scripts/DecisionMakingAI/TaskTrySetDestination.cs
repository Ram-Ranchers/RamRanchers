using UnityEngine;

namespace DecisionMakingAI
{
    public class TaskTrySetDestination : Node
    {
        //private CharacterManager _manager;
//
        //private Ray _ray;
        //private RaycastHit _raycastHit;
//
        //public TaskTrySetDestination(CharacterManager manager) : base()
        //{
        //    _manager = manager;
        //}
//
        //public override NodeState Evaluate()
        //{
        //    if (_manager.IsSelected && Input.GetMouseButtonUp(1))
        //    {
        //        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//
        //        if (Physics.Raycast(_ray, out _raycastHit, 1000f, Globals.Terrain_Layer_Mask))
        //        {
        //            _parent._parent.SetData("destinationPoint", _raycastHit.point);
        //            state = NodeState.Success;
        //            return state;
        //        }
        //    }
//
        //    state = NodeState.Failure;
        //    return state;
        //}
    }
}
