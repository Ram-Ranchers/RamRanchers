using System;
using System.Collections.Generic;

namespace DecisionMakingAI
{
    //[UnityEngine.RequireComponent(typeof(CharacterManager))]
    public class CharacterBT //: BehaviourTree
    {
      // private CharacterManager _manager;

      // private void Awake()
      // {
      //     _manager = GetComponent<CharacterManager>();
      // }

      // protected override Node SetupTree()
      // {
      //     Node root;

      //     root = new Parallel(new List<Node>
      //     {
      //         new Sequence(new List<Node>
      //         {
      //             new CheckUnitIsMine(_manager),
      //             new TaskTrySetDestination(_manager),
      //         }),
      //         new Sequence(new List<Node>
      //         {
      //             new CheckHasDestination(),
      //             new TaskMoveToDestination(_manager),
      //         })
      //     });
      //     
      //     return root;
      // }
    }
}
