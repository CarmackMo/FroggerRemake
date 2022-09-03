using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{
    // Start is called before the first frame update
    float lastHorizontalInput=0, lastVerticalInput=0;  //  input from last flame
    public bool linearMove = false;
    GameObject platform=null;
    Vector3 offset;
    GameObject[] endPointObjects;
    public int endPointsAchievedNum=0;
    public int totalHP = 0;
    bool gameOver = false;

    private int damage = 0;

    void Start()
    {
        endPointObjects = GameObject.FindGameObjectsWithTag("Ends");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
            return;
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
            Time.timeScale = 0;
            SetGameOver();
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
            Time.timeScale = 0;
            SetGameOver();
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
        GameObject other=collision.gameObject;
        if (other.tag.Equals("Ends") && !other.GetComponent<EndPointsController>().achieved)
        {
            other.GetComponent<EndPointsController>().arriveEndPoint();
            if (++endPointsAchievedNum==endPointObjects.Length)   // winning condition
            {
                GamePanel.Instance.ShowGameResult(true);
            }
        }
        else if (other != null &&
                 other.GetComponent<Obstacles>() != null &&
                 other.GetComponent<Obstacles>().knockbackable)  // Knockback
        {
            damage++;
            Obstacles obstacles = other.GetComponent<Obstacles>();
            GamePanel.Instance.UpdateHPText(damage, totalHP);
            
            if (totalHP - damage > 0)
                transform.Translate(Vector3.down * obstacles.knockbackStrength);
            else
                SetGameOver();
        }
    }

    public void SetGameOver()
    {
        gameOver = true;
        GamePanel.Instance.ShowGameResult(false);
    }
}
