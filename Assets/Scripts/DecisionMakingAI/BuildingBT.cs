using System.Collections.Generic;

namespace DecisionMakingAI
{
    //[UnityEngine.RequireComponent(typeof(BuildingManager))]
    public class BuildingBT //: //BehaviourTree
    {
    //   private BuildingManager _manager;

    //   private void Awake()
    //   {
    //       _manager = GetComponent<BuildingManager>();
    //   }

    //   protected override Node SetupTree()
    //   {
    //       Node root;

    //       root = new Sequence(new List<Node>()
    //       {
    //           new CheckUnitIsMine(_manager);
    //           new Timer(GameManager.instance.producingRate, new List<Node>()
    //           {
    //            new TaskProduceResources(_manager);   
    //           }, 
    //           delegate 
    //           {
    //           EventManager.TriggerEvent("UpdateResourceTexts");
    //           }
    //           )
    //       });
    //       
    //       return root;
    //   }
    }
}
