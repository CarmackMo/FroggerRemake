using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{
    // Start is called before the first frame update

    public float linearMovementSpeed = 3;
    public Animator animator;
    public int totalHP = 0;
    private Vector3 initPos;
    public enum MovementMethod
    {
        linear, non_linear, auto_forward
    };
    public MovementMethod movementMethod, initMovementMethod;
    private bool triggerAutoForward=false;
    private float lastHorizontalInput=0, lastVerticalInput=0;  //  input from last flame
    private GameObject platform=null;
    private Vector3 offset;
    private bool dying=false;
    private int damage = 0;
    private int autoForwardFacing=0;

    void Start()
    {
        initPos = transform.position;
        initMovementMethod = movementMethod;
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
        if (movementMethod.Equals(MovementMethod.non_linear))   // press a botton move a tile
        {
            if (lastHorizontalInput == 0 && horizontalInput != 0)  // prevent simultaneously input
            {
                animator.SetFloat("VerticalDirection", 0);
                if (horizontalInput > 0)
                {
                    offset.x += 1;
                    animator.SetFloat("HorizontalDireciton", 1);
                    animator.SetTrigger("PlayerInput");
                }
                else if (horizontalInput < 0)
                {
                    offset.x -= 1;
                    animator.SetFloat("HorizontalDireciton", -1);
                    animator.SetTrigger("PlayerInput");
                }
            }
            else if (lastVerticalInput==0&&verticalInput!=0)  
            {
                animator.SetFloat("HorizontalDireciton", 0);
                if (verticalInput > 0)
                {
                    offset.y += 1;
                    animator.SetFloat("VerticalDirection", 1);
                    animator.SetTrigger("PlayerInput");
                }
                else if (verticalInput < 0)
                {
                    offset.y -= 1;
                    animator.SetFloat("VerticalDirection", -1);
                    animator.SetTrigger("PlayerInput");
                }
            }
            lastHorizontalInput = horizontalInput;
        }
        else if (movementMethod.Equals(MovementMethod.linear))
        {
            offset += Vector3.up * linearMovementSpeed * Time.deltaTime * verticalInput;
            offset += Vector3.right * linearMovementSpeed * Time.deltaTime * horizontalInput;
        }
        else if (movementMethod.Equals(MovementMethod.auto_forward))
        {
            if (!triggerAutoForward && (lastHorizontalInput != 0||lastVerticalInput!=0))
            {
                triggerAutoForward = true;
                if (horizontalInput != 0)
                {
                    if (horizontalInput > 0)
                        ++autoForwardFacing;
                    else if (horizontalInput < 0)
                        --autoForwardFacing;
                }
            }
            if (triggerAutoForward)
            {
                if (lastHorizontalInput == 0 && horizontalInput != 0)
                {
                    if (horizontalInput > 0)
                        ++autoForwardFacing;
                    else if (horizontalInput < 0)
                        --autoForwardFacing;
                }
                autoForwardFacing = Mathf.Clamp(autoForwardFacing, -2, 2);
                switch (autoForwardFacing)
                {
                    case 0:
                        offset += Vector3.up * linearMovementSpeed * Time.deltaTime;
                        break;
                    case 1:
                        offset += Vector3.up * linearMovementSpeed * Time.deltaTime / 2;
                        offset += Vector3.right * linearMovementSpeed * Time.deltaTime / 2;
                        break;
                    case 2:
                        offset += Vector3.right * linearMovementSpeed * Time.deltaTime;
                        break;
                    case -1:
                        offset += Vector3.up * linearMovementSpeed * Time.deltaTime / 2;
                        offset += Vector3.left * linearMovementSpeed * Time.deltaTime / 2;
                        break;
                    case -2:
                        offset += Vector3.left * linearMovementSpeed * Time.deltaTime;
                        break;
                }
            }
            lastHorizontalInput = horizontalInput;
            lastVerticalInput = verticalInput;
        }
        transform.Translate(offset);


        if (platform!=null)  // frog steps on a platform
        {
            transform.position = platform.transform.position + offset;
            if (platform.GetComponent<PlatformsController>().sink)  // it sinked!
            {
                offset = new Vector3(0, 0, 0);
                platform = null;
            }
        }
        else
        {
            offset = new Vector3(0, 0, 0);
            if (movementMethod.Equals(MovementMethod.non_linear))   // round the axis to int
                transform.position = new Vector3(Mathf.Round(transform.position.x + 0.5f) - 0.5f, Mathf.Round(transform.position.y + 0.5f) - 0.5f, Mathf.Round(transform.position.z + 0.5f) - 0.5f);
        }
    }
    void switchPlatform(GameObject p)
    {
        platform = p;
        offset = transform.position - platform.transform.position;
        if (movementMethod.Equals(MovementMethod.non_linear))
            offset.y = Mathf.Round(offset.y);
    }

    // hit & entering partforms decection
    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag.Equals("Enemy")|| other.tag.Equals("Obstacle") || other.tag.Equals("Lava"))
        {
            GameplayController.Instance.SetGameOver(false);
        }

        if (other.tag.Equals("Platform"))
        {
            if (movementMethod.Equals(MovementMethod.auto_forward))   // entering a platform results in changing movement methord
                movementMethod = MovementMethod.linear;
            if (platform != null)  // stay on mutiple platforms
            {
                if (!platform.GetComponent<Collider2D>().bounds.Contains(transform.position) && other.GetComponent<Collider2D>().bounds.Contains(transform.position))
                {   // if central point in other platform
                    switchPlatform(other);
                }
            }
            else
                if (!other.GetComponent<PlatformsController>().sink)
                {
                    switchPlatform(other);
                }
                else   // still sinkking
                    return;
        }
        else if (platform == null && other.tag.Equals("Water"))
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

            ContactFilter2D filter = new ContactFilter2D();  // try to find another platform to stand on
            filter.useTriggers = true;
            Collider2D[] results = new Collider2D[10];
            GetComponent<Collider2D>().OverlapCollider(filter, results);
            foreach (Collider2D p in results)
            {
                if (!p)  //no platform
                    break;
                if (p.gameObject.tag.Equals("Platform")&&platform!=p)
                {
                    switchPlatform(p.gameObject);
                    break;
                }
            }
            if (movementMethod.Equals(MovementMethod.non_linear))   // round the axis to int
                transform.position = new Vector3(Mathf.Round(transform.position.x + 0.5f) - 0.5f, Mathf.Round(transform.position.y + 0.5f) - 0.5f, Mathf.Round(transform.position.z + 0.5f) - 0.5f);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag.Equals("Ends") && !other.GetComponent<EndPointsController>().achieved)  //entering a ending point
        {
            other.GetComponent<EndPointsController>().arriveEndPoint();
            GameplayController.Instance.IncreaseAchievedObjective();
            if (!GameplayController.Instance.gameOver)  // reach an ending point
            {
                GameObject lava = GameObject.FindGameObjectWithTag("Lava");  //reset lava
                lava.GetComponent<LavaController>().resetLava();
                transform.position = initPos;  //reset postion
                platform = null; //reset platform
                offset = Vector3.zero;
                movementMethod = initMovementMethod;  //reset movement mothod
                if (movementMethod.Equals(MovementMethod.auto_forward))
                {
                    triggerAutoForward = false;
                    autoForwardFacing = 0;
                }
                    
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
