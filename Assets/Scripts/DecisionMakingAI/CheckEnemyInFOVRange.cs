using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace DecisionMakingAI
{
    public class CheckEnemyInFOVRange : Node
    {
        private static int _enemyLayerMask = 1 << 6;
        private Transform _transform;

        //private UnitManager _manager;
        //private float _fovRadius;
        //private int _unitOwner;
//
        //private Vector3 _pos;
//
        //public CheckEnemyInFOVRange(UnitManager manager) : base()
        //{
        //    _manager = manager;
        //    _fovRadius = _manager.Unit.Data.fieldOfView;
        //    _unitOwner = _manager.Unit.Owner;
        //}
        //
        //public override NodeState Evaluate()
        //{
        //    _pos = _manager.transform.position;
        //    IEnumerable<Collider> enemiesInRange = Physics.OverlapSphere(_pos, _fovRadius, Globals.Unit_Mask).Where(
        //        delegate(Collider c)
        //        {
        //            UnitManager um = c.GetComponent<UnitManager>();
        //            if (um == null)
        //            {
        //                return false;
        //            }
//
        //            return um.Unit.Owner != _unitOwner;
        //        });
        //    if (enemiesInRange.Any())
        //    {
        //        _parent.SetData("currentTarget",
        //            enemiesInRange.OrderBy(x => (x.transform.position - _pos).sqrMagnitude).First().transform);
        //        state = NodeState.Success;
        //        return state;
        //    }
//
        //    state = NodeState.Failure;
        //    return state;
        //}
        
        public CheckEnemyInFOVRange(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                Collider[] colliders =
                    Physics.OverlapSphere(_transform.position, GuardBT.fovRange, _enemyLayerMask);
                if (colliders.Length > 0)
                {
                    _parent._parent.SetData("target", colliders[0].transform);
                    
                    state = NodeState.Success;
                    return state;
                }
                
                state = NodeState.Failure;
                return state;
            }

            state = NodeState.Success;
            return state;
        }
    }
}
