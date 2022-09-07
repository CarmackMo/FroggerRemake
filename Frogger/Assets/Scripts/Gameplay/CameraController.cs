using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    public Vector2 maxPosition, minPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target!= null)
        {
            Vector3 targetPos=target.position;
            targetPos.z = transform.position.z;
            targetPos.x = Mathf.Clamp(targetPos.x,minPosition.x,maxPosition.x);
            targetPos.y = Mathf.Clamp(targetPos.y, minPosition.y, maxPosition.y);
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
        }
    }
}
