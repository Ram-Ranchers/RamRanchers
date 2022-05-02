using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFlocking2 : MonoBehaviour
{
    [SerializeField] private float speedMax = 1.0f;
    [SerializeField] private float alignVal = 1.0f;
    [SerializeField] private float separVal = 1.0f;
    [SerializeField] private float cohesVal = 1.0f;
    [SerializeField] private float alignRange = 2.0f;
    [SerializeField] private float separRange = 2.0f;
    [SerializeField] private float cohesRange = 2.0f;


    private GameObject[] birds;
    // Start is called before the first frame update
    void Start()
    {
        birds = GameObject.FindGameObjectsWithTag("Bird");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
