using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private float gridWidth = 1f;
    [SerializeField] private float gridLength = 1f;
    [SerializeField] private float gridHeight = 1f;
    [SerializeField] private int scale = 2;

    private Vector3 cells;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < gridWidth; i++)
        {
            for(int j = 0; j < gridLength; j++)
            {
                for(int k = 0; k < gridHeight; k++)
                {
                    cells.x = i * scale;
                    cells.y = k * scale;
                    cells.z = j * scale;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
