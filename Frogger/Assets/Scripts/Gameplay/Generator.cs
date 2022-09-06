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
        EnemyPlarform,
        SinkingPlatform,
    }

    [Header("Prefabs")]
    public GameObject obstacle;
    public GameObject knockBack;
    public GameObject normalPlatform;
    public GameObject enemyPlatform;
    public GameObject sinkingPlatform;

    [Header("Generate Parameters")]
    public Vector2 interval = Vector2.zero;             // Generate interval is within a range
    public Vector3 direction = Vector3.zero;
    public ObjectType type = ObjectType.NormalObstacle;

    [Header("Object Parameters")]
    public Vector2 speed = Vector2.zero;                // Moving speed of the object is within a range

    [Header("Obstacle Parameters")]
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
                return normalPlatform;
            case ObjectType.EnemyPlarform:
                return enemyPlatform;
            case ObjectType.SinkingPlatform:
                return sinkingPlatform;
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
                    obj.GetComponent<Obstacles>().
                        ObstacleInit(Random.Range(speed.x, speed.y), direction, type,  knockbackable, knockbackStrength);
                }
                else
                {
                    obj.GetComponent<Platforms>()
                        .PlatformInit(Random.Range(speed.x, speed.y), direction, type);
                }

                count = Random.Range(interval.x, interval.y);
            }

            count -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

}
