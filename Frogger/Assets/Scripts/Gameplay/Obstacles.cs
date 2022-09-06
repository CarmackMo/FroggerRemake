using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private float speed;
    private Vector3 direction;

    public bool knockbackable;
    public float knockbackStrength;
    public Generator.ObjectType type;


    private void Start() { }

    private void Update()
    {
        ObstacleMove();
        ObstacleDespawn();
    }


    public void ObstacleInit(float speed, Vector3 direction, Generator.ObjectType type, 
                             bool knockbackable, float knockbackStrength)
    {
        this.speed = speed;
        this.direction = direction;
        this.type = type;
        this.knockbackable = knockbackable;
        this.knockbackStrength = knockbackStrength;
    }

    private void ObstacleMove()
    {
        gameObject.transform.Translate(direction * speed * Time.deltaTime);
    }

    private void ObstacleDespawn()
    {
        // If obstacle move outside the horizontal boundary
        if (direction.x < 0 && transform.position.x < -16f)
            Destroy(gameObject);
        else if (direction.x > 0 && transform.position.x > 16f)
            Destroy(gameObject);

        // If obstacle move outside the verticle boundary
        if (direction.y < 0 && transform.position.y < -15f)
            Destroy(gameObject);
        else if (direction.y > 0 && transform.position.y > 15f)
            Destroy(gameObject);
    }


}
