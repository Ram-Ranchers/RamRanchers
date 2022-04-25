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
            if (_agent.target == null)
            {
                return false;
            }
        
            _agent.target = GameObject.Find("target").transform;
            
            Debug.Log("Move");

            
            //if ()
            //{
            //    if (playSound)
            //    {
            //        contextualSource.PlayOneShot(((CharacterData)Unit.Data).onMoveInvalidSound); 
            //    }
//
            //    return false;
            //}
            
            _agent.target.transform.position = targetPosition;

            //if (playSound)
            //{
            //    contextualSource.PlayOneShot(((CharacterData)Unit.Data).onMoveValidSound);
            //}

            return true;
        }
    }
}
