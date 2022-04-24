using System.Collections.Generic;
using Unity.AI.Navigation;

namespace DecisionMakingAI
{
    public enum InGameResource
    {
        Gold,
        Wood,
        Stone
    }

    public static class Globals
    {
        public static int Terrain_Layer_Mask = 1 << 7;
        public static int Flat_Terrain_Layer_Mask = 1 << 9;
        public static int Unit_Mask = 1 << 11;
        public static int Tree_Mask = 1 << 12;
        public static int Rock_Mask = 1 << 13;
        
        public static BuildingData[] Building_Data;
        public static NavMeshSurface Nav_Mesh_Surface;
        public static Dictionary<string, SkillData> Skill_Data = new Dictionary<string, SkillData>();

		public static void UpdateNavMeshSurface()
		{
			//Nav_Mesh_Surface.UpdateNavMesh(Nav_Mesh_Surface.navMeshData);
		}

        public static Dictionary<InGameResource, GameResource> Game_Resources =
            new Dictionary<InGameResource, GameResource>()
            {
                { InGameResource.Gold, new GameResource("Gold", 1000) },
                { InGameResource.Wood, new GameResource("Wood", 1000) },
                { InGameResource.Stone, new GameResource("Stone", 1000) }
            };

        public static List<UnitManager> Selected_Units = new List<UnitManager>();
    }
}
