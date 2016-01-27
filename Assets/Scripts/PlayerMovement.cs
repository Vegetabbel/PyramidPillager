using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody rb;

    private float moveSpeedPhysics; //'Active' move speed, set in Update()
    private float groundMoveSpeedPhysics = 100f;
    private float airMoveSpeedPhysics; //Is set in Start()
    private float sprintMoveSpeedPhysics = 150f;
    private float maxMoveSpeedPhysics; //'Active max move speed, set in Update() and Start()
    private float normalMaxMoveSpeedPhysics = 8f;
    private float sprintMaxMoveSpeedPhysics = 16f;

    private float moveSpeedArcade = 10f;

    private float jumpForce = 500f;
    private float wallJumpForce = 200f;

    private bool isGrounded = false;
    private bool isTouchingLeftWall = false;
    private bool isTouchingRightWall = false;

    public GameObject bottomLeftCorner;
    public GameObject bottomRightCorner;

    public enum Controls {Physics, Arcade};
    public Controls controlState;


	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        airMoveSpeedPhysics = groundMoveSpeedPhysics * 0.1f;
        maxMoveSpeedPhysics = normalMaxMoveSpeedPhysics;
	}

    void Update()
    {
        //Check if player is standing on something
        if (Physics.Raycast(bottomLeftCorner.transform.position, -Vector3.up, 0.2f) ||
            Physics.Raycast(bottomRightCorner.transform.position, -Vector3.up, 0.2f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        //Sprinting
        if (Input.GetKey(KeyCode.LeftShift) || !isGrounded)
        {
            maxMoveSpeedPhysics = sprintMaxMoveSpeedPhysics;
        }
        else
        {
            maxMoveSpeedPhysics = normalMaxMoveSpeedPhysics;
        }
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            moveSpeedPhysics = sprintMoveSpeedPhysics;
        }
        else
        {
            moveSpeedPhysics = normalMaxMoveSpeedPhysics;
        }

        //Set movement speed in air
        if (!isGrounded)
        {
            moveSpeedPhysics = airMoveSpeedPhysics;
            moveSpeedArcade = 3;
        }
        else
        {
            moveSpeedPhysics = groundMoveSpeedPhysics;            
            moveSpeedArcade = 10f;
        }
    }

    void FixedUpdate ()
    {
        switch (controlState)
        {
            case Controls.Physics:
                //Move left
                if (Input.GetKey(KeyCode.A))
                {
                    rb.AddForce(-1 * moveSpeedPhysics, 0, 0);
                }
                //Move right
                if (Input.GetKey(KeyCode.D))
                {
                    rb.AddForce(moveSpeedPhysics, 0, 0);
                }
                //Jump
                if (isGrounded)
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        rb.AddForce(0, jumpForce, 0);
                    }
                }
                //Wall jump
                else
                {
                    if (isTouchingLeftWall)
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            rb.AddForce(wallJumpForce, jumpForce * 0.5f, 0);
                        }
                    }
                    if (isTouchingRightWall)
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            rb.AddForce(-1 * wallJumpForce, jumpForce * 0.5f, 0);
                        }
                    }
                } 

                //Restrict maximum movement speed
                rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxMoveSpeedPhysics, maxMoveSpeedPhysics), rb.velocity.y, rb.velocity.z);
                break;
                 
            case Controls.Arcade:
                //Move left
                if (Input.GetKey(KeyCode.A))
                {
                    rb.position = new Vector3   (rb.position.x - moveSpeedArcade * Time.deltaTime,
                                                rb.position.y,
                                                rb.position.z);
                }
                //Move right
                if (Input.GetKey(KeyCode.D))
                {
                    rb.position = new Vector3   (rb.position.x + moveSpeedArcade * Time.deltaTime,
                                                rb.position.y,
                                                rb.position.z);
                }
                //Jump
                if (isGrounded)
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        rb.AddForce(0, jumpForce, 0);
                    }
                }
                else
                {
                    if (isTouchingLeftWall)
                    {
                        if (Input.GetKey(KeyCode.Space))
                        {
                            rb.AddForce(wallJumpForce, jumpForce * 0.5f, 0);
                        }
                    }
                    if (isTouchingRightWall)
                    {
                        if (Input.GetKey(KeyCode.Space))
                        {
                            rb.AddForce(-1 * wallJumpForce, jumpForce * 0.5f, 0);
                        }
                    }
                }
                break;

            default:
                Debug.Log("No control type selected");
                break;
        }
    }
    public bool IsTouchingLeftWall
    {
        get { return isTouchingLeftWall;  }
        set { isTouchingLeftWall = value; }
    }
    public bool IsTouchingRightWall
    {
        get { return isTouchingRightWall; }
        set { isTouchingRightWall = value; }
    }
    public bool IsGrounded
    {
        get { return isGrounded; }
        set { isGrounded = value; }
    }
}
