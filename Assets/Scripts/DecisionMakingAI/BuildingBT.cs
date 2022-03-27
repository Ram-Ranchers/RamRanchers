using System.Collections.Generic;

namespace DecisionMakingAI
{
    [UnityEngine.RequireComponent(typeof(BuildingManager))]
    public class BuildingBT : BehaviourTree
    {
        private BuildingManager _manager;

        private void Awake()
        {
            _manager = GetComponent<BuildingManager>();
        }

        protected override Node SetupTree()
        {
            Node _root;

            _root = new Parallel();
            if (_manager.Unit.Data.attackDamage > 0)
            {
                Sequence attackSequence = new Sequence(new List<Node>
                {
                    new CheckEnemyInAttackRange(_manager),
                    new Timer(_manager.Unit.Data.attackRate,
                        new List<Node>()
                        {
                            new TaskAttack(_manager),
                        }
                    ),
                });

                _root.Attach(attackSequence);
                _root.Attach(new CheckEnemyInFOVRange(_manager));
            }

            _root.Attach(new Sequence(new List<Node>
            {
                new CheckUnitIsMine(_manager),
                new Timer(GameManager.instance._producingRate,
                    new List<Node>()
                    {
                        new TaskProduceResources(_manager)
                    },
                delegate
                {
                    EventManager.TriggerEvent("UpdateResourceTexts");
                }
                )
            }));

            return _root;
        }
    }
}