using UnityEngine;

namespace DecisionMakingAI
{
    [CreateAssetMenu(fileName = "Global Parameters", menuName = "Scriptable Objects/Game Global Parameters", order = 10)]
    public class GameGlobalParameters : GameParameters
    {
        public override string GetParametersName() => "Global";
        
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
