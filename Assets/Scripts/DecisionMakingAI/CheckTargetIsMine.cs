using UnityEngine;

namespace DecisionMakingAI
{
    public class CheckTargetIsMine : Node
    {
        //private int _myPlayerId;
//
        //public CheckTargetIsMine(UnitManager manager) : base()
        //{
        //    _myPlayerId = GameManager.instance.gamePlayersParameters.myPlayerId;
        //}
//
        //public override NodeState Evaluate()
        //{
        //    object currentTraget = _parent.GetData("currentTarget");
        //    UnitManager um = ((Transform)currentTraget).GetComponent<UnitManager>();
        //    if (um == null)
        //    {
        //        state = NodeState.Failure;
        //        return state;
        //    }
//
        //    state = um.Unit.Owner == _myPlayerId ? NodeState.Success : NodeState.Failure;
        //    return state;
        //}
    }
}
