using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdGen2 : MonoBehaviour
{
    [SerializeField] private GameObject[] landings;
    [SerializeField] private GameObject birdPref;
    [SerializeField] private int numBirds = 20;
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
            newBirdPos.x = spawnLanding.x + spawnDist * Random.Range(-numBirds/4, numBirds/4);
            newBirdPos.z = spawnLanding.z + spawnDist * Random.Range(-numBirds/4, numBirds/4);
            birds[i] = Instantiate(birdPref, newBirdPos, new Quaternion(0, 0, 0, 1), this.transform);
            birds[i].tag = "bird";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
