using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody rb;
    private float moveSpeedPhysics; //'Active' move speed, set in Update()
    private float groundMoveSpeedPhysics = 100f;
    private float airMoveSpeedPhysics; //Is set in Start()
    private float sprintMoveSpeedPhysics = 123; //Is added on top of normal move speed
    private float sprintMaxMoveSpeedPhysics = 12f;
    private float maxMoveSpeedPhysics = 8f;
    private float moveSpeedArcade = 10f;
    private float jumpForce = 400f;
    private float wallJumpForce = 300f;
    private bool isGrounded = false;
    private bool isTouchingLeftWall = false;
    private bool isTouchingRightWall = false;

    public enum Controls {Physics, Arcade};
    public Controls controlState;


	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        airMoveSpeedPhysics = groundMoveSpeedPhysics * 0.5f;
	}

    void Update()
    {
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

        if (Physics.Raycast(gameObject.transform.position, -Vector3.up, 1f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void FixedUpdate ()
    {
        switch (controlState)
        {
            case Controls.Physics:
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    
                }
                //Move left
                if (Input.GetKey(KeyCode.A))
                {
                    rb.AddForce(-1 * groundMoveSpeedPhysics, 0, 0);
                }
                //Move right
                if (Input.GetKey(KeyCode.D))
                {
                    rb.AddForce(groundMoveSpeedPhysics, 0, 0);
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
