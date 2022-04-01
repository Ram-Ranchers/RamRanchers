using System.Collections.Generic;

namespace DecisionMakingAI
{
    [UnityEngine.RequireComponent(typeof(CharacterManager))]
    public class CharacterBT : BehaviourTree
    {
        private CharacterManager _manager;

        private void Awake()
        {
            _manager = GetComponent<CharacterManager>();
        }

        protected override Node SetupTree()
        {
            Node _root;

            Sequence trySetDestinationOrTargetSequence = new Sequence(new List<Node>
            {
                new CheckUnitIsMine(_manager),
                new TaskTrySetDestinationOrTarget(_manager),
            });

            Sequence moveToDestinationSequence = new Sequence(new List<Node>
            {
                new CheckHasDestination(),
                new TaskMoveToDestination(_manager),
            });

            Sequence attackSequence = new Sequence(new List<Node>
            {
                new Inverter(new List<Node>
                {
                    new CheckTargetIsMine(_manager),
                }),
                new CheckEnemyInAttackRange(_manager),
                new Timer(_manager.Unit.Data.attackRate, new List<Node>
                {
                    new TaskAttack(_manager)
                }),
            });

            Sequence moveToTargetSequence = new Sequence(new List<Node>
            {
                new CheckHasTarget()
            });
            if (_manager.Unit.Data.attackDamage > 0)
            {
                moveToTargetSequence.Attach(new Selector(new List<Node>
                {
                    attackSequence,
                    new TaskFollow(_manager),
                }));
            }
            else
            {
                moveToTargetSequence.Attach(new TaskFollow(_manager));
            }

            _root = new Selector(new List<Node>
            {
                new Parallel(new List<Node>
                {
                    trySetDestinationOrTargetSequence,
                    new Selector(new List<Node>
                    {
                        moveToDestinationSequence,
                        moveToTargetSequence,
                    }),
                }),
                new CheckEnemyInFOVRange(_manager),
            });

            return _root;
        }
    }
}