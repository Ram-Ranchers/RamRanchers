using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUnit : MonoBehaviour
{
    const float pathUpdateMoveThreshold = .5f;
    const float minPathUpdateTime = .2f;

    public Transform target;
    public float speed = 20f;
    public float turnSpeed = 3f;
    public float turnDst = 5f;

    Path path;

    void Start()
    {
        StartCoroutine(UpdatePath());
    }
    
    public void OnClick()
    {
        if (target == null)
        {
            return;
        }

        target = GameObject.Find("target").GetComponent<Transform>();
   
        Ray _ray;
        RaycastHit _raycastHit;

        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _raycastHit, 1000f))
        {
            target.transform.position = _raycastHit.point;
        }
    }
    
    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            path = new Path(waypoints, transform.position, turnDst);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    public IEnumerator UpdatePath()
    {
        if(target != null)
        {
            PathRequestManager.RequestPath(new PathRequest(transform.position, target.transform.position, OnPathFound));

            float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
            Vector3 targetPosOld = target.transform.position;

            while (true)
            {
                yield return new WaitForSeconds(minPathUpdateTime);
                if ((target.transform.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
                {
                    PathRequestManager.RequestPath(new PathRequest(transform.position, target.transform.position, OnPathFound));
                    targetPosOld = target.transform.position;
                }
            }
        }
    }

    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;

        float speedPercent = 1f;

        transform.LookAt(path.lookPoints[0]);
        while(followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);

            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {
                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
            }
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            path.DrawWithGizmos();
        }
    }
}