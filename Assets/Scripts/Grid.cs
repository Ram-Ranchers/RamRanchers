using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private float gridSizeX;
    private float gridSizeZ;

    // Start is called before the first frame update
    void Start()
    {
        gridSizeX = gameObject.bounds.size.x;
        gridSizeZ = gameObject.bounds.size.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
