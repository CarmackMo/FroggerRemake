using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    private float speed;
    private Vector3 direction;
    public Generator.ObjectType type;


    private void Start() { }

    private void Update() 
    {
        PlatformMove();
        PlatformDespawn();
    }

    public void PlatformInit(float speed, Vector3 direction, Generator.ObjectType type) 
    {
        this.speed = speed; 
        this.direction = direction;
        this.type = type;
    }


    private void PlatformMove()
    {
        gameObject.transform.Translate(direction * speed * Time.deltaTime);
    }


    private void PlatformDespawn()
    {
        // If platform move outside the horizontal boundary
        if (direction.x < 0 && transform.position.x < -16f)
            Destroy(gameObject);
        else if (direction.x > 0 && transform.position.x > 16f)
            Destroy(gameObject);

        // If platform move outside the verticle boundary
        if (direction.y < 0 && transform.position.y < -15f)
            Destroy(gameObject);
        else if (direction.y > 0 && transform.position.y > 15f)
            Destroy(gameObject);
    }






    //// Start is called before the first frame update
    //[Serializable]
    //public class Path   // used to make the object move along the trajectory
    //{
    //    public GameObject point;
    //    public float moveTime;
    //    public float waitTime;
    //    public Vector3 speed;  // move speed to next point
    //}

    //public Path[] path;
    //public enum MovementMethod 
    //{
    //    blancing, repeating, stopping
    //};
    //public MovementMethod movementMethod;
    //public int position = 0;  // the number of point the object currently at
    //public float sinkTime = 0;
    //public float sinkInterval = -1;  // -1 means never sink
    //public bool sink = false;
    //public Color linesColor = Color.blue;
    //private float remainingWaitTime = 0, remainingMoveTime=0;
    //private int direction = 1;
    //private bool stopped=false, turned=false;  // turned: record if object turned in last frame when it's in blancing method
    //private float remainingSinkTime=0, remainingSinkInterval=0;

    //void Start()
    //{
    //    for(int i = 0; i < path.Length - 1; i++)
    //        path[i].speed = (path[i + 1].point.transform.position - path[i].point.transform.position) / path[i].moveTime;
    //    path[^1].speed = (path[0].point.transform.position - path[^1].point.transform.position) / path[^1].moveTime;
    //    // dont forget to set the last point's speed
    //    remainingMoveTime = path[0].moveTime;
    //    remainingWaitTime = path[0].waitTime;
    //    if (sink)
    //    {
    //        remainingSinkTime = sinkTime;
    //        remainingSinkInterval = 0;
    //    }
    //    else
    //    {
    //        remainingSinkInterval = sinkInterval;
    //        remainingSinkTime = 0;
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (stopped)
    //        return;
    //    if (remainingMoveTime>0)  // still moving
    //    {
    //        remainingMoveTime -= Time.deltaTime;
    //        remainingMoveTime = Mathf.Max(remainingMoveTime, 0);
    //        transform.position += direction * Time.deltaTime * path[position].speed;
    //    }
    //    if (remainingMoveTime==0&&remainingWaitTime > 0) // still waiting
    //    {
    //        remainingWaitTime -= Time.deltaTime;
    //        remainingWaitTime = Mathf.Max(remainingWaitTime, 0);
    //    }
    //    if (remainingWaitTime==0&& remainingMoveTime == 0)  // time to head off next point
    //    {
    //        position += direction;
    //        if (movementMethod == MovementMethod.blancing &&
    //            ((position == path.Length && direction == 1) || (position == -1 && direction == -1)))
    //        {
    //            direction = -direction;  // change direction
    //            position += direction;
    //            turned = true;
    //        }
    //        else if (movementMethod == MovementMethod.repeating && position == path.Length)
    //        {
    //            position = 0;
    //            transform.position = path[0].point.transform.position;  // make it back to first point
    //        }
    //        else if (movementMethod == MovementMethod.stopping && position == path.Length)
    //        {
    //            stopped = true;
    //            return;
    //        }

    //        // prevent funny stuffs from happening if dropping frames
    //        if (turned)
    //        {
    //            transform.position = path[0].point.transform.position;
    //            turned = false;
    //        }
    //        else if (movementMethod == MovementMethod.blancing && direction<0)
    //            transform.position = path[position-direction].point.transform.position;   // when object is blancing back
    //        else
    //            transform.position = path[position].point.transform.position;
    //        remainingMoveTime = path[position].moveTime;
    //        remainingWaitTime = path[position].waitTime;
    //    }

        
    //    if (sinkInterval>0)
    //    {
    //        if (remainingSinkInterval>0)    
    //            remainingSinkInterval -= Time.deltaTime;
    //        if (remainingSinkTime > 0)
    //            remainingSinkTime -= Time.deltaTime;
    //        if (!sink && remainingSinkInterval <= 0)  // Time to sink!
    //        {
    //            sink = true;
    //            remainingSinkInterval = 0;
    //            remainingSinkTime = sinkTime;
    //            Color color = GetComponent<Renderer>().material.color;
    //            color.a = 0;
    //            GetComponent<Renderer>().material.color = color;
    //        }
    //        else if (sink && remainingSinkTime <= 0)  // Time to float!
    //        {
    //            sink = false;
    //            remainingSinkInterval = sinkInterval;
    //            remainingSinkTime = sinkTime;
    //            Color color = GetComponent<Renderer>().material.color;
    //            color.a = 1;
    //            GetComponent<Renderer>().material.color = color;
    //        }
    //    }

    //}
    //void OnDrawGizmos()  // draw lines on the editer
    //{
    //    Gizmos.color = linesColor;
    //    for (int i = 0; i < path.Length - 1; i++)
    //    {
    //        if (path[i].point && path[i + 1].point)
    //        {
    //            Gizmos.DrawLine(path[i].point.transform.position, path[i + 1].point.transform.position);
    //        }
    //    }
    //}
}
