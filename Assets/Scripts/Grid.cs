using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private float width;
    private float height;
    private float length;
    [SerializeField] private float cellSize = 1f;

    [SerializeField] private List<Vector3> cellPos;

    private void Start()
    {
        width = gameObject.transform.lossyScale.x;
        height = gameObject.transform.lossyScale.y;
        length = gameObject.transform.lossyScale.z;
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
