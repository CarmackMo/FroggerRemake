using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    // Start is called before the first frame update
    public float secondPreTile=4;
    public float maxPostion;
    private float speed;
    private Vector3 initPos;
    void Start()
    {
        speed = 1 / secondPreTile;
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (secondPreTile <=0)
            return;
        if (transform.position.y<maxPostion)
        {
            transform.Translate(Vector3.up*Time.deltaTime*speed);
        }
    }

    public void resetLava()
    {
        transform.position = initPos;
    }
}
