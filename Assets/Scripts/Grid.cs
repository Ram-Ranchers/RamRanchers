using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    struct cell
    {
        private float xPos;
        private float yPos;
        private float zPos;
        private bool isWalkable = true;

        [SerializeField] private float cellSize = 1f;
    }

    private float width;
    private float height;
    private float length;

    [SerializeField] private cell[] cells;

   // [SerializeField] private List<Vector3> cellPos;

    private void Start()
    {
        width = gameObject.GetComponent<Renderer>().bounds.size.x;
        height = gameObject.GetComponent<Renderer>().bounds.size.y;
        length = gameObject.GetComponent<Renderer>().bounds.size.z;
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < length; z++)
            {
                for(int y = 0; y < height; y++)
                {
                    cellPos.Add(new Vector3(x * cellSize, y * cellSize, z * cellSize));
                }
            }
        }
    }
}
