using RTS;
using UnityEngine;

namespace DecisionMakingAI
{
    public class TaskAttack : Node
    {
        private Transform _lastTarget;
        private EnemyManager _enemyManager;

        private float _attackTime = 1f;
        private float _attackCoumter = 0f;
        
        public TaskAttack(Transform transform)
        {
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            if (target != _lastTarget)
            {
               _enemyManager = target.GetComponent<EnemyManager>();
               _lastTarget = target;
            }

            _attackCoumter += Time.deltaTime;
            if (_attackCoumter >= _attackTime)
            {
               bool enemyIsDead = _enemyManager.TakeHit();
               if (enemyIsDead)
               {
                   ClearData("target");
               }
               else
               {
                   _attackCoumter = 0f;   
               }
            }
            
            state = NodeState.Running;
            return state;
        }
    }
}
