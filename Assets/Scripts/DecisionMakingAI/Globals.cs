using System.Collections.Generic;

namespace DecisionMakingAI
{
    public class Globals
    {
        public static int Terrain_Layer_Mask = 1 << 7;

        public static BuildingData[] Building_Data;

        public static Dictionary<string, GameResource> Game_Resources =
            new Dictionary<string, GameResource>()
            {
                { "gold", new GameResource("Gold", 300) },
                { "wood", new GameResource("Wood", 300) },
                { "stone", new GameResource("Stone", 300) }
            };

        public static List<UnitManager> Selected_Units = new List<UnitManager>();
    }
}
