using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool linearMove = false;
    public int totalHP = 0;
    private Vector3 initPos;

    private float lastHorizontalInput=0, lastVerticalInput=0;  //  input from last flame
    private GameObject platform=null;
    private Vector3 offset;
    private bool dying=false;
    private int damage = 0;

    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameplayController.Instance.gameOver)
            return;
        if (dying)
        {
            if (platform != null||transform.position==initPos)  
            {
                dying = false;
            }
            else  // no platform? dead
                GameplayController.Instance.SetGameOver(false);
        }
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput=Input.GetAxisRaw("Vertical");
        if (!linearMove)
        {
            if (lastHorizontalInput == 0 && horizontalInput != 0)
            {
                if (horizontalInput > 0)
                    offset.x += 1;
                else
                    offset.x -= 1;
            }
            else if (lastVerticalInput==0&&verticalInput!=0)  // prevent simultaneously input
            {
                if (verticalInput > 0)
                    offset.y += 1;
                else
                    offset.y -= 1;
            }
            transform.Translate(offset);
            lastHorizontalInput = horizontalInput;
            lastVerticalInput = verticalInput;
        }
        if (platform!=null)  // frog steps on a platform
        {
            transform.position = platform.transform.position + offset;
            if (platform.GetComponent<PlatformsController>().sink)  // it sinked!
            {
                offset = new Vector3(0, 0, 0);
                if (!linearMove)   // round the axis to int
                    transform.position = new Vector3(Mathf.Round(transform.position.x + 0.5f) - 0.5f, Mathf.Round(transform.position.y + 0.5f) - 0.5f, Mathf.Round(transform.position.z + 0.5f) - 0.5f);
                platform = null;
            }
        }
        else
        {
            offset = new Vector3(0, 0, 0);
        }
    }

    // hit & entering partforms decection
    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag.Equals("Enemy"))
        {
            GameplayController.Instance.SetGameOver(false);
        }

        if (other.tag.Equals("Platform") && platform==null)
        {
            if (!other.GetComponent<PlatformsController>().sink)
            {
                platform = other;
                offset = transform.position - platform.transform.position;
            }
            else   // still sinkking
                return;
        }
        else if (platform==null && (other.tag.Equals("Obstacle") ||
            other.tag.Equals("Water")))
        {
            // death or hit
            dying = true;  // steped on water, judge if there is a platform next frame
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other == platform)
        {
            platform = null;
            offset = new Vector3(0, 0, 0);
            if (!linearMove)   // round the axis to int
                transform.position = new Vector3(Mathf.Round(transform.position.x+0.5f)-0.5f, Mathf.Round(transform.position.y + 0.5f) - 0.5f, Mathf.Round(transform.position.z + 0.5f) - 0.5f);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag.Equals("Ends") && !other.GetComponent<EndPointsController>().achieved)
        {
            other.GetComponent<EndPointsController>().arriveEndPoint();
            GameplayController.Instance.IncreaseAchievedObjective();

            if (GameplayController.Instance.gameOver == false)
            {
                transform.position = initPos;
            }
        }
        else if (other != null &&
                 other.GetComponent<Obstacles>() != null)
        {
            Obstacles obstacle = other.GetComponent<Obstacles>();

            // If is a knock back obstacle, deal with knock back logic
            if (obstacle.type == Generator.ObjectType.KnockBackObstacle)
            {
                damage++;
                GamePanel.Instance.UpdateHPText(damage, totalHP);

                if (totalHP - damage > 0)
                    transform.Translate(Vector3.down * obstacle.knockbackStrength);
                else
                    GameplayController.Instance.SetGameOver(false);

            }
            // If is a non konck back obstacle, player die immediately
            else
            {
                GameplayController.Instance.SetGameOver(false);
            }
        }
    }
}
