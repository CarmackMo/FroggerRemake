using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{
    // Start is called before the first frame update
    float lastHorizontalInput=0, lastVerticalInput=0;  //  input from last flame
    public bool linearMove = false;
    void Start()
    {
        
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
                    transform.Translate(new Vector3(1, 0, 0));
                else
                    transform.Translate(new Vector3(-1, 0, 0));
            }
            else if (lastVerticalInput==0&&verticalInput!=0)  // prevent simultaneously input
            {
                if (verticalInput > 0)
                    transform.Translate(new Vector3(0, 1, 0));
                else
                    transform.Translate(new Vector3(0, -1, 0));
            }
            lastHorizontalInput = horizontalInput;
            lastVerticalInput = verticalInput;
        }
    }
}
