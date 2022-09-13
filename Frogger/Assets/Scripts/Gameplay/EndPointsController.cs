using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointsController : MonoBehaviour
{
    public bool achieved = false;

    public GameObject squrrelImage;
    public GameObject holeImage;


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
        holeImage.SetActive(false);
        squrrelImage.SetActive(true);
        achieved = true;
    }
}
