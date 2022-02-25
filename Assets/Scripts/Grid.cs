using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;

    public Grid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];

        for(int i = 0; i < gridArray.GetLength(0); i++)
        {
            for(int j = 0; j < gridArray.GetLength(1); j++)
            {
                Debug.Log(i + ", " + j);
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
