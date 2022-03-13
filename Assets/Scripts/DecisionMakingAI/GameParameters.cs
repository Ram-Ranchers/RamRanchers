using UnityEngine;

namespace DecisionMakingAI
{
    [CreateAssetMenu(fileName = "Parameters", menuName = "Scriptable Objects/Game Parameters", order = 10)]
    public class GameParameters : ScriptableObject
    {
        [Header("Day and Night")]
        public bool enableDayAndNightCycle;
        public float dayLengthInSeconds;
        public float dayInitialRatio;
        
        [Header("Initialisation")]
        public BuildingData initialBuilding;
        
        [Header("FOV")]
        public bool enableFOV;
    }
}
