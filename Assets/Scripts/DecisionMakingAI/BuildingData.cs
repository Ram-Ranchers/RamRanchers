using System.Collections.Generic;
using UnityEngine;

namespace DecisionMakingAI
{
    [CreateAssetMenu(fileName = "Building", menuName = "Scriptable Objects/Building", order = 1)]
    public class BuildingData : ScriptableObject
    {
        public string code;
        public string unitname;
        public string description;
        public int healthpoints;
        public GameObject prefab;
        public List<ResourceValue> cost;
        
        public bool CanBuy()
        {
            foreach (ResourceValue resource in cost)
            {
                if (Globals.Game_Resources[resource.code].Amount < resource.amount)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
