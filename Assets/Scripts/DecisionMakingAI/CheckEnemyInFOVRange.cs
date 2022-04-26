using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace DecisionMakingAI
{
    public class CheckEnemyInFOVRange : Node
    {
        private UnitManager _manager;
        private float _fovRadius;
        private int _unitOwner;

        private Vector3 _pos;

        public CheckEnemyInFOVRange(UnitManager manager) : base()
        {
            _manager = manager;
            _fovRadius = _manager.Unit.Data.fieldOfView;
            _unitOwner = _manager.Unit.Owner;
        }

        public override NodeState Evaluate()
        {
            _pos = _manager.transform.position;
            IEnumerable<Collider> enemiesInRange = Physics.OverlapSphere(_pos, _fovRadius, Globals.Unit_Mask).Where(
                delegate(Collider c)
                {
                    UnitManager um = c.GetComponent<UnitManager>();
                    if (um == null)
                    {
                        return false;
                    }

                    return um.Unit.Owner != _unitOwner;
                });

            if (enemiesInRange.Any())
            {
                _parent.SetData("currentTarget",
                    enemiesInRange.OrderBy(x => (x.transform.position - _pos).sqrMagnitude).First().transform);
                _state = NodeState.Success;
                return _state;
            }

            IEnumerable<Collider> buildingsInRange = Physics.OverlapSphere(_pos, _fovRadius, Globals.Building_Mask).Where(
                delegate (Collider c)
                {
                    UnitManager um = c.GetComponent<UnitManager>();
                    if (um == null)
                    {
                        return false;
                    }

                    return um.Unit.Owner != _unitOwner;
                });

            if (buildingsInRange.Any())
            {
                _parent.SetData("currentTarget",
                    buildingsInRange.OrderBy(x => (x.transform.position - _pos).sqrMagnitude).First().transform);
                _state = NodeState.Success;
                return _state;
            }

            _state = NodeState.Failure;
            return _state;
        }
    }
}