using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private Renderer lineRenderer;

    private float gridSizeX;
    private float gridSizeZ;

    [SerializeField] private float size = 10;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        gridSizeX = gameObject.transform.localScale.x / size;
        gridSizeZ = gameObject.transform.localScale.z / size;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeZ; j++)
            {
                Vector3 pos0 = new Vector3(mSquare[i, j].getX(), zOffset, mSquare[i, j].getY());
                Vector3 pos1 = new Vector3(mSquare[i, j].getXPlus(), zOffset, mSquare[i, j].getY());
                Vector3 pos2 = new Vector3(mSquare[i, j].getXPlus(), zOffset, mSquare[i, j].getYPlus());
                Vector3 pos3 = new Vector3(mSquare[i, j].getX(), zOffset, mSquare[i, j].getYPlus());

                lineRenderer.SetWidth(10, 10);
                lineRenderer.SetColors(Color.red, Color.red);

                lineRenderer.SetPosition(0, pos0);
                lineRenderer.SetPosition(1, pos1);
                lineRenderer.SetPosition(2, pos2);
                lineRenderer.SetPosition(3, pos3);
                lineRenderer.SetPosition(4, pos0);
            }
        }
    }
}   
