using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdGen : MonoBehaviour
{
    [SerializeField] private Vector3 herdSpawn;
    [SerializeField] private int numRams;
    [SerializeField] private GameObject ramPref;

    private GameObject[] rams;

    // Start is called before the first frame update
    void Start()
    {
        rams = new GameObject[numRams];
        Vector3 newRamPos = herdSpawn;
        for (int i = 0; i < numRams; i++)
        {
            newRamPos.x = herdSpawn.x + .5f * Random.Range(-numRams, numRams);
            newRamPos.z = herdSpawn.z + .5f * Random.Range(-numRams, numRams);
            rams[i] = Instantiate(ramPref, newRamPos, new Quaternion(0, 0, 0, 1), this.transform);
            rams[i].GetComponent<Rigidbody>().velocity = new Vector3(1,0,1) * Random.Range(-1.0f, 1.0f);
            rams[i].tag = "Ram";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
