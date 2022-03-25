using System.Collections.Generic;

namespace DecisionMakingAI
{
    //[UnityEngine.RequireComponent(typeof(BuildingManager))]
    public class BuildingBT //: //BehaviourTree
    {
        // private BuildingManager _manager;
//
        // private void Awake()
        // {
        //     _manager = GetComponent<BuildingManager>();
        // }
//
        // protected override Node SetupTree()
        // {
        //     Node root;
//
        //     root = new Parallel()
        //     if (_manager.Unit.Data.attackDamage > 0)
        //     {
        //         Sequence attackSequence = new Sequence(new List<Node>
        //         {
        //             new CheckEnemyInAttackRange(_manager),
        //             new Timer(_manager.Unit.Data.attackRate,
        //                 new List<Node>()
        //                 {
        //                     new TaskAttack(_manager),
        //                 }
        //             ),
        //         });
        //         
        //         root.Attach(attackSequence);
        //         root.Attach(new CheckEnemyInFOVRange(_manager));
        //     }
        //     
        //     root.Attach(new Sequence(new List<Node>
        //     {
        //         new CheckUnitIsMine(_manager),
        //         new Timer(GameManager.instance.producingRate, 
        //             new List<Node>()
        //         {
        //             new TaskProduceResources(_manager)
        //         }),
        //         delegate
        //         {
        //             EventManager.TriggerEvent("UpdateResourceTexts");
        //         }
        //     }));
//
        //     return root;
        // }
    }
}
