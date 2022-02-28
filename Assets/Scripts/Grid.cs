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

    // https://www.youtube.com/watch?v=waEsGu--9P8&t=0s
    //[SerializeField] private float gridWidth = 1f;
    //[SerializeField] private float gridLength = 1f;
    //[SerializeField] private float gridHeight = 1f;
    //[SerializeField] private int scale = 2;

    //private List<Vector3> cells;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    for(int i = 0; i < gridWidth; i++)
    //    {
    //        for(int j = 0; j < gridLength; j++)
    //        {
    //            for(int k = 0; k < gridHeight; k++)
    //            {
    //                cells.x = i * scale;
    //                cells.y = k * scale;
    //                cells.z = j * scale;

    //                print(cells);
    //            }
    //        }
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
