using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdVel : MonoBehaviour
{

    public Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position += velocity * Time.deltaTime;
    }
}
