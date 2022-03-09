using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2 : MonoBehaviour
{
    private int width;
    private int height;
    private int length;
    [SerializeField] private float cellSize = 1f;
    private int[,,] gridArray;

    public Grid2(int width, int height, int length, float cellSize)
    {
        this.width = width;
        this.length = length;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, length, height];
    }
}
