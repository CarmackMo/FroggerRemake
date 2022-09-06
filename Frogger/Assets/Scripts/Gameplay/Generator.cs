using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public enum ObjectType 
    { 
        NormalObstacle,  
        KnockBackObstacle, 
        NormalPlatform, 
    }

    [Header("Prefabs")]
    public GameObject obstacle;
    public GameObject knockBack;
    public GameObject platform;

    [Header("Generate Parameters")]
    public Vector2 interval = Vector2.zero;             // Generate interval is within a range
    public Vector3 direction = Vector3.zero;
    public ObjectType type = ObjectType.NormalObstacle;

    [Header("Object Parameters")]
    public float speed = 0;

    public bool knockbackable = false;
    public float knockbackStrength = 0;


    private void Start()
    {
        StartCoroutine(GenerateCoroutine());
    }


    public GameObject SelectSpawnObject(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.NormalObstacle:
                return obstacle;
            case ObjectType.KnockBackObstacle:
                return knockBack;
            case ObjectType.NormalPlatform:
                return platform;
            default:
                return obstacle;
        }
    }


    IEnumerator GenerateCoroutine()
    {
        float count = Random.Range(interval.x, interval.y);

        while (true)
        {
            if (count <= 0)
            {
                Vector3 pos = transform.position;
                if (direction.x != 0)
                    pos.x *= direction.x;
                if (direction.y != 0)
                    pos.y *= direction.y;

                GameObject obj = Instantiate(SelectSpawnObject(type), pos, Quaternion.identity);

                if (type == ObjectType.NormalObstacle || type == ObjectType.KnockBackObstacle)
                {
                    obj.GetComponent<Obstacles>().ObstacleInit(speed, direction, knockbackable, knockbackStrength);
                }
                else
                {

                }

                count = Random.Range(interval.x, interval.y);
            }

            count -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

}
