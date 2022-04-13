using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid3 : MonoBehaviour
{
    public bool displayGridGizmos;
    public Transform player;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public LayerMask unwalkableMask;
    Node[,] grid;
    public TerrainType[] walkableRegions;
    public int obstacleProximityPenalty = 10;

    LayerMask walkableMask;
    Dictionary<int, int> walkableRegionDictionary = new Dictionary<int, int>();

    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;

    float nodeDiameter;
    int gridSizeX;
    int gridSizeY;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach(TerrainType region in walkableRegions)
        {
            walkableMask.value = walkableMask |= region.terrainMask.value;
            walkableRegionDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }

        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                int movementPenalty = 0;

                Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, walkableMask))
                {
                    walkableRegionDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                }

                if(!walkable)
                {
                    movementPenalty += obstacleProximityPenalty;
                }

                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }
        }

        BlurPenaltyMap(5);
    }

    void BlurPenaltyMap(int blurSize)
    {
        int kernalSize = blurSize * 2 + 1;
        int kernalExtents = (kernalSize - 1) / 2;

        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        for(int y = 0; y < gridSizeY; y++)
        {
            for(int x = -kernalExtents; x <= kernalExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernalExtents);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }

            for(int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernalExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernalExtents, 0, gridSizeX - 1);

                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = -kernalExtents; y <= kernalExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernalExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernalSize * kernalSize));

            grid[x, 0].movementPenalty = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernalExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(y + kernalExtents, 0, gridSizeX - 1);

                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];

                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernalSize * kernalSize));

                grid[x, y].movementPenalty = blurredPenalty;

                if(blurredPenalty > penaltyMax)
                {
                    penaltyMax = blurredPenalty;
                }
                if(blurredPenalty < penaltyMin)
                {
                    penaltyMin = blurredPenalty;
                }
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        float percentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if(grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));

                Gizmos.color = (n.walkable) ? Gizmos.color : Color.red;       
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter));
            }
        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}
