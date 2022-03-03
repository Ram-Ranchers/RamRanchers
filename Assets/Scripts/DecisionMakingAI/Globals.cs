namespace DecisionMakingAI
{
    public class Globals
    {
        public static int Terrain_Layer_Mask = 1 << 7;
        
        public static BuildingData[] Building_Data = new BuildingData[]
        {
            new BuildingData("House", 100),
            new BuildingData("Tower", 50)
        };
    }
}
