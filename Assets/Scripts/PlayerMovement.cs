using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody rb;
    private float moveSpeedPhysics = 100f;
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
	}

    void Update()
    {
        if (!isGrounded)
        {
            moveSpeedPhysics = 30;
            moveSpeedArcade = 3;
        }
        else
        {
            moveSpeedPhysics = 100f;
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
                rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -15f, 15f), rb.velocity.y, rb.velocity.z);
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
}
