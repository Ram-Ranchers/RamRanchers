using System;
using UnityEngine;
using UnityEngine.AI;

namespace DecisionMakingAI
{
    public class CharacterManager : UnitManager
    {
        public PathfindingUnit _agent;
        
        private Character _character;

        public override Unit Unit
        {
            get { return _character; }
            set { _character = value is Character ? (Character)value : null; }
        }

        private void Start()
        {
            _character.Place();
        }

        public bool MoveTo(Vector3 targetPosition, bool playSound = true)
        {
            _agent.target = GameObject.Find("target").transform;

            _agent.target.transform.position = targetPosition;

            return true;
        }
    }
}
