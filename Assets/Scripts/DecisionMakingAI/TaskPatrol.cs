using System.Collections.Generic;
using UnityEngine;

namespace DecisionMakingAI
{
    public class TaskPatrol : Node
    {
        //private int _currentWaypointIndex = 0;

        //private float _waitTime = 1f;
        //private float _waitCounter = 0f;
        //private bool _waiting = false;

        //private Transform _transform;
        //private Transform[] _waypoints;
        
        //public TaskPatrol(Transform transform, Transform[] waypoints)
        //{
        //    _transform = transform;
        //    _waypoints = waypoints;
        //}
        
        //public override NodeState Evaluate()
        //{
        //    if (_waiting)
        //    {
        //        _waitCounter += _waitTime.deltaTime;
        //        if (_waitCounter >= _waitTime )
        //        {
        //            _waiting = false;
        //        }
        //    }
        //    else
        //    {
        //        _transform wp = _waypoints[_currentWaypointIndex];
        //        if (Vector3.Distance(_transform.position, wp.position) < 0.01f)
        //        {
        //            _transform.position = wp.position;
        //            _waitCounter = 0f;
        //            _waiting = true;

        //            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        //        }
        //        else
        //        {
        //            _transform.position = Vector3.MoveTowards(_transform.position, wp.position, GuardBT.speed * Time.deltaTime);
        //            _transform.LookAt(wp.position);
        //        }
        //    }
        //     state = NodeState.Running;
        //     return state;
        //}
    }
}