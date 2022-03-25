using UnityEngine;

namespace DecisionMakingAI
{
    [CreateAssetMenu(fileName = "Building", menuName = "Scriptable Objects/Building", order = 2)]
    public class BuildingData : UnitData
    {
        [Header("Building Sounds")] 
        public AudioClip ambientSound;
    }
}
