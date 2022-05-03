using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdGen : MonoBehaviour
{
    [SerializeField] private GameObject[] landings;
    [SerializeField] private GameObject birdPref;
    [SerializeField] private int numBirds;
    [SerializeField] private float spawnDist;
    public Vector3 spawnLanding;
    private GameObject[] birds;
    // Start is called before the first frame update
    void Start()
    {
        birds = new GameObject[numBirds];
        int spawnId = Random.Range(0, landings.Length);
        spawnLanding = landings[spawnId].transform.position;
        Vector3 newBirdPos = spawnLanding;
        for (int i = 0; i < numBirds; i++)
        {
            newBirdPos.x = spawnLanding.x + .05f * Random.Range(-numBirds, numBirds);
            newBirdPos.z = spawnLanding.z + .05f * Random.Range(-numBirds, numBirds);
            birds[i] = Instantiate(birdPref, newBirdPos, new Quaternion(0, 0, 0, 1), this.transform);
            birds[i].GetComponent<Rigidbody>().velocity = new Vector3(1, 1, 1) * Random.Range(-0.1f, 0.1f);
            birds[i].tag = "bird";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
