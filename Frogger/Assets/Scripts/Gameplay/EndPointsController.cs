using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointsController : MonoBehaviour
{
    public bool achieved = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void arriveEndPoint()
    {
        GetComponent<Renderer>().material.color = Color.green;  // change later
        achieved = true;
    }
}
