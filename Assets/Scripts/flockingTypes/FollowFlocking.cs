using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowFlocking : MonoBehaviour
{
    [SerializeField] private float speedMax = 5.0f;
    [SerializeField] private float alignVal = 1.0f;
    [SerializeField] private float separVal = 1.0f;
    [SerializeField] private float cohesVal = 1.0f;
    [SerializeField] private float folowVal = 1.0f;
    [SerializeField] private float alignRange = 2.0f;
    [SerializeField] private float separRange = 1.0f;
    [SerializeField] private float cohesRange = 2.0f;
    //[SerializeField] private float folowRange = 150.0f;


    private GameObject[] birds;
    private GameObject[] landings;
    private Vector3 targetLanding;
    private Vector3 currentLanding;
    private float landingCooldown = 5;
    private float landingTimer = 0;
    private float birdsLanded = 0;
    private bool isFlying = false;
    private BirdGen2 birdGen;
    public GameObject birdsContainer;

    private void Awake()
    {
        birdGen = birdsContainer.GetComponent<BirdGen2>();
    }

    void Start()
    {
        landings = GameObject.FindGameObjectsWithTag("Landing");
        birds = GameObject.FindGameObjectsWithTag("bird");
        Debug.Log(birds.Length);
        currentLanding = birdGen.spawnLanding;
        targetLanding = landings[Random.Range(0, landings.Length)].transform.position;
        birdsLanded = birds.Length;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 centerPos = Vector3.zero;
        for (int i = 0; i < birds.Length; i++)
        {
            centerPos += birds[i].transform.position;
        }
        centerPos /= birds.Length;
        Vector3 newVel = Vector3.zero;
        if(!isFlying)
        {
            Debug.Log(landingTimer);
            landingTimer += Time.deltaTime;
            if (landingTimer > landingCooldown)
            {
                isFlying = true;
                birdsLanded = 0;
                targetLanding = landings[Random.Range(0, landings.Length)].transform.position;
                while (targetLanding==currentLanding)
                {
                    targetLanding = landings[Random.Range(0, landings.Length)].transform.position;
                }
                Debug.Log(targetLanding);
            }
        }
        if(isFlying)
        {
            for (int i = 0; i < birds.Length; i++)
            {
                //if((birds[i].transform.position-centerPos).sqrMagnitude>4.0f)
                //{
                //    birds[i].transform.SetPositionAndRotation(centerPos, birds[i].transform.rotation);
                //}
                newVel = birds[i].GetComponent<BirdVel>().velocity;
                if((targetLanding - birds[i].transform.position).sqrMagnitude >0.8f)
                {
                    newVel += (alignVal * Align(birds[i]) + cohesVal * Cohesion(birds[i]) + separVal * Separation(birds[i]) + folowVal * Follow(birds[i]) + Vector3.one * 0.3f * Random.Range(-1.0f, 1.0f));
                }
                else
                {
                    newVel += folowVal * Follow(birds[i]);
                }
                if (newVel.sqrMagnitude > speedMax * speedMax)
                    newVel = newVel.normalized * speedMax;

                if((targetLanding - birds[i].transform.position).sqrMagnitude>(currentLanding - birds[i].transform.position).sqrMagnitude)
                {
                    newVel.y = 0.05f;
                }
                else
                {
                    newVel.y = -0.05f;
                }
                if(birds[i].transform.position.y<0.09f)
                {
                    birds[i].transform.SetPositionAndRotation(new Vector3(birds[i].transform.position.x, 0.1f, birds[i].transform.position.z), birds[i].transform.rotation);
                }
                birds[i].GetComponent<BirdVel>().velocity = newVel;
                if (Mathf.Abs(birds[i].transform.position.x - targetLanding.x) < 0.25f && Mathf.Abs(birds[i].transform.position.z - targetLanding.z) < 0.25f)
                {
                    birds[i].GetComponent<BirdVel>().velocity = Vector3.zero;
                    birds[i].transform.SetPositionAndRotation(new Vector3(birds[i].transform.position.x, 0.1f, birds[i].transform.position.z), birds[i].transform.rotation);
                    birdsLanded++;
                }
            }
            if (birdsLanded >= birds.Length)
            {
                Debug.Log(birdsLanded);
                isFlying = false;
                currentLanding = targetLanding;
                landingTimer = 0;
                Vector3 newBirdPos = currentLanding;
                for (int i = 0; i < birds.Length; i++)
                {
                    birds[i].GetComponent<BirdVel>().velocity = Vector3.zero;
                    newBirdPos.x = currentLanding.x + 0.07f * Random.Range(-birds.Length / 4, birds.Length / 4);
                    newBirdPos.z = currentLanding.z + 0.07f * Random.Range(-birds.Length / 4, birds.Length / 4);
                }
            }
        }
    }


    Vector3 Align(GameObject bird)
    {
        Vector3 outV = Vector3.zero;
        int s = 0;

        foreach (GameObject r in birds)
        {
            if (r != bird && (bird.transform.position - r.transform.position).sqrMagnitude < alignRange * alignRange)
            {
                outV.x += r.GetComponent<BirdVel>().velocity.x;
                outV.z += r.GetComponent<BirdVel>().velocity.z;
                s++;
            }
        }
        if (s > 0)
        {
            outV.x /= s;
            outV.z /= s;
            outV.y = bird.GetComponent<BirdVel>().velocity.y;
            outV.Normalize();
            return outV;
        }
        else
            return outV;
    }
    Vector3 Cohesion(GameObject bird)
    {
        Vector3 cohesion = Vector3.zero;
        int s = 0;

        foreach (GameObject r in birds)
        {
            if (r != bird && (bird.transform.position - r.transform.position).sqrMagnitude < cohesRange * cohesRange)
            {
                cohesion.x += r.transform.position.x;
                cohesion.z += r.transform.position.z;
                s++;
            }
        }
        if (s > 0)
        {
            cohesion.x /= s;
            cohesion.z /= s;
            cohesion.y = bird.transform.position.y;
            cohesion -= bird.transform.position;
            cohesion.Normalize();
            return cohesion;
        }
        else
            return bird.transform.position;
    }
    Vector3 Separation(GameObject bird)
    {
        Vector3 separate = Vector3.zero;
        int s = 0;

        foreach (GameObject r in birds)
        {
            if (r != bird && (bird.transform.position - r.transform.position).sqrMagnitude < separRange * separRange)
            {
                separate.x += r.transform.position.x - bird.transform.position.x;
                separate.z += r.transform.position.z - bird.transform.position.z;
                s++;
            }
        }
        if (s > 0)
        {
            separate.x /= -s;
            separate.z /= -s;
            separate.y = bird.transform.position.y;
            separate.Normalize();
            return separate;
        }
        else
            return bird.transform.position;
    }
    Vector3 Follow(GameObject bird)
    {
        Vector3 outV = Vector3.zero;

        outV = targetLanding - bird.transform.position;
        outV.Normalize();

        return outV;
    }
}
