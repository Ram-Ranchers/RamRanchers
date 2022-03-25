using System.Collections.Generic;
using Unity.AI.Navigation;

namespace DecisionMakingAI
{
    public static class Globals
    {
        public static int Terrain_Layer_Mask = 1 << 7;

        public static int Flat_Terrain_Layer_Mask = 1 << 9;

        public static BuildingData[] Building_Data;

		public static NavMeshSurface Nav_Mesh_Surface;

		public static void UpdateNavMeshSurface()
		{
			Nav_Mesh_Surface.UpdateNavMesh(Nav_Mesh_Surface.navMeshData);
		}

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
