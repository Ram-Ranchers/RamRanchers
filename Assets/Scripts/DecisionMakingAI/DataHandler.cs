using UnityEngine;

namespace DecisionMakingAI
{
    public static class DataHandler
    {
        public static void LoadGameData()
        {
            // load building data
            Globals.Building_Data =
                Resources.LoadAll<BuildingData>("ScriptableObjects/Units/Buildings") as BuildingData[];
            
            // load game parameters
            GameParameters[] gameParametersList = Resources.LoadAll<GameParameters>("ScriptableObjects/Parameters");
            foreach (GameParameters parameters in gameParametersList)
            {
                parameters.LoadFromFile();
            }
        }

        public static void SaveGameData()
        {
            // save game parameters
            GameParameters[] gameParametersList = Resources.LoadAll<GameParameters>("ScriptableObjects/Parameters");
            foreach (GameParameters parameters in gameParametersList)
            {
                parameters.SaveToFile();
            }
        }
    }
}
