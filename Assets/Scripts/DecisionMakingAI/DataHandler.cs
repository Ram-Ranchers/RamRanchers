using UnityEngine;

namespace DecisionMakingAI
{
    public static class DataHandler
    {
        public static void LoadGameData()
        {
            Globals.Building_Data =
                Resources.LoadAll<BuildingData>("ScriptableObjects/Units/Buildings") as BuildingData[];
        }
    }
}
