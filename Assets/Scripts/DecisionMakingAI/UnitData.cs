using System.Collections.Generic;
using UnityEngine;

namespace DecisionMakingAI
{
    [CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Objects/Unit", order = 1)]
    public class UnitData : ScriptableObject
    {
        public string code;
        public string unitname;
        public string description;
        public int healthpoints;
        public GameObject prefab;
        public List<ResourceValue> cost;
        public List<SkillData> skills = new List<SkillData>();
        public float fieldOfView;
            
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
