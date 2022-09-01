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
    GameObject[] gameOverObjects;

    void Start()
    {
        gameOverObjects = gameOverObjects = GameObject.FindGameObjectsWithTag("GameOver");
        hideGameOver();
    }

    // Update is called once per frame
    void Update()
    {
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
        }
        else
        {
            offset = new Vector3(0, 0, 0);
        }
    }
    // hit & entering partforms decection
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform" && platform==null)
        {
            platform = collision.gameObject;
            offset = transform.position-platform.transform.position;
        }
            
        else if (platform==null && (collision.gameObject.tag == "Obstacle" || 
            collision.gameObject.tag == "Water"))
        {
            // death or hit
            showGameOver();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject==platform)
        {
            platform = null;
            offset = new Vector3(0, 0, 0);
            if (linearMove)
                transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
        }
    }

    private void hideGameOver()
    {
        foreach (GameObject gameOverObject in gameOverObjects)
        {
            gameOverObject.SetActive(false);
        }
    }
    private void showGameOver()
    {
        foreach (GameObject gameOverObject in gameOverObjects)
        {
            gameOverObject.SetActive(true);
        }
    }
}
