using UnityEngine;

namespace DecisionMakingAI
{
    [CreateAssetMenu(fileName = "Global Parameters", menuName = "Scriptable Objects/Game Global Parameters", order = 10)]
    public class GameGlobalParameters : ScriptableObject
    {
        public delegate int ResourceProductionFunc(float distance);
        
        public int baseGoldProduction;
        public int bonusGoldProductionPerBuilding;
        public float goldBonusRange;
        public float woodProductionRange;
        public float stoneProductionRange;
        
        [Header("Day and Night")]
        public bool enableDayAndNightCycle;
        public float dayLengthInSeconds;
        public float dayInitialRatio;
        
        [Header("Initialisation")]
        public BuildingData initialBuilding;
        
        [Header("FOV")]
        public bool enableFOV;

        [HideInInspector] 
        public ResourceProductionFunc WoodProductionFunc = (float distance) =>
        {
            return Mathf.CeilToInt(10 * 1f / distance);
        };

        [HideInInspector] 
        public ResourceProductionFunc stoneProductionFunc = (float distance) =>
        {
            return Mathf.CeilToInt(2 * 1f / distance);
        };
    }
}
