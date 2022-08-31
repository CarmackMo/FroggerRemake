using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlatformsController : MonoBehaviour
{
    // Start is called before the first frame update
    [Serializable]
    public class Path   // used to make the object move along the trajectory
    {
        public GameObject point;
        public float moveTime;
        public float waitTime;
        public Vector3 speed;  // move speed to next point
    }

    public Path[] path;
    public enum MovementMethod 
    {
        blancing, repeating, stopping
    };
    public MovementMethod movementMethod;
    public int position = 0;  // the number of point the object currently at
    private float remainingWaitTime = 0, remainingMoveTime=0;
    private int direction = 1;
    private bool stopped=false;

    void Start()
    {
        for(int i = 0; i < path.Length - 1; i++)
            path[i].speed = (path[i + 1].point.transform.position - path[i].point.transform.position) / path[i].moveTime;
        path[^1].speed = (path[0].point.transform.position - path[^1].point.transform.position) / path[^1].moveTime;
        // dont forget to set the last point's speed
        remainingMoveTime = path[0].moveTime;
        remainingWaitTime = path[0].waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopped)
            return;
        if (remainingMoveTime>0)  // still moving
        {
            remainingMoveTime -= Time.deltaTime;
            remainingMoveTime = Mathf.Max(remainingMoveTime, 0);
            transform.position += direction * Time.deltaTime * path[position].speed;
        }
        if (remainingMoveTime==0&&remainingWaitTime > 0) // still waiting
        {
            remainingWaitTime -= Time.deltaTime;
            remainingWaitTime = Mathf.Max(remainingWaitTime, 0);
        }
        if (remainingWaitTime==0&& remainingMoveTime == 0)  // time to head off next point
        {
            position += direction;
            if (movementMethod == MovementMethod.blancing &&
                ((position == path.Length && direction == 1) || (position == -1 && direction == -1)))
            {
                direction = -direction;  // change direction
                position += direction;
            }
            else if (movementMethod == MovementMethod.repeating && position == path.Length)
            {
                position = 0;
                transform.position = path[0].point.transform.position;  // make it back to first point
            }
            else if (movementMethod == MovementMethod.stopping && position == path.Length)
            {
                stopped = true;
                return;
            }
            remainingMoveTime = path[position].moveTime;
            remainingWaitTime = path[position].waitTime;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < path.Length - 1; i++)
        {
            if (path[i].point && path[i + 1].point)
            {
                Gizmos.DrawLine(path[i].point.transform.position, path[i + 1].point.transform.position);
            }
        }
    }
}
