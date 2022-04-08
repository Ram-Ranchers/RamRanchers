using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFlocking : MonoBehaviour
{
    [SerializeField] private float neighbourRange = 10.0f;
    [SerializeField] private float alignVal = 1.0f;
    [SerializeField] private float separVal = 1.0f;
    [SerializeField] private float cohesVal = 1.0f;

    private GameObject[] rams;
    // Start is called before the first frame update
    void Start()
    {
        rams = GameObject.FindGameObjectsWithTag("Ram");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newVel = Vector3.zero;
        for (int i = 0; i < rams.Length; i++)
        {
            newVel += (alignVal * Alignment(rams[i]) + cohesVal * Cohesion(rams[i]) + separVal * Separation(rams[i]) + Vector3.one * 0.3f * Random.Range(-1.0f, 1.0f));
            newVel.y = 0;
            if (newVel.sqrMagnitude > 49)
                newVel = newVel.normalized * 7;
            rams[i].GetComponent<Rigidbody>().velocity = newVel;
        }
    }

    Vector3 Alignment(GameObject ram)
    {
        Vector3 align = Vector3.zero;
        int neighbours = 0;

        foreach (GameObject r in rams)
        {
            if (r != ram && (ram.transform.position - r.transform.position).sqrMagnitude < neighbourRange * neighbourRange)
            {
                align.x += r.GetComponent<Rigidbody>().velocity.x;
                align.z += r.GetComponent<Rigidbody>().velocity.z;
                neighbours++;
            }
        }
        if (neighbours > 0)
        {
            align.x /= neighbours;
            align.z /= neighbours;
            align.y = ram.GetComponent<Rigidbody>().velocity.y;
            return align;
        }
        else
            return align;
    }
    Vector3 Cohesion(GameObject ram)
    {
        Vector3 cohesion = Vector3.zero;
        int neighbours = 0;

        foreach (GameObject r in rams)
        {
            if (r != ram && (ram.transform.position - r.transform.position).sqrMagnitude < neighbourRange * neighbourRange)
            {
                cohesion.x += r.transform.position.x;
                cohesion.z += r.transform.position.z;
                neighbours++;
            }
        }
        if (neighbours > 0)
        {
            cohesion.x /= neighbours;
            cohesion.z /= neighbours;
            cohesion.y = ram.transform.position.y;
            cohesion -= ram.transform.position;
            return cohesion;
        }
        else
            return ram.transform.position;
    }
    Vector3 Separation(GameObject ram)
    {
        Vector3 separate = Vector3.zero;
        int neighbours = 0;

        foreach (GameObject r in rams)
        {
            if (r != ram && (ram.transform.position - r.transform.position).sqrMagnitude < neighbourRange * neighbourRange)
            {
                separate.x += r.transform.position.x - ram.transform.position.x;
                separate.z += r.transform.position.z - ram.transform.position.z;
                neighbours++;
            }
        }
        if (neighbours > 0)
        {
            separate.x /= -neighbours;
            separate.z /= -neighbours;
            separate.y = ram.transform.position.y;
            return separate;
        }
        else
            return ram.transform.position;
    }
}
