using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private bool isAlive = true;

    public enum PlayerState { Idle, Moving, Sprinting, Soaring, Falling, TouchingLeftWall, TouchingRightWall }
    public PlayerState playerState;

    public enum PlayerForm { Isis, };
    public PlayerForm playerForm = PlayerForm.Isis;

    private float accelerationSpeedActive;          //Active move speed, set in Update()
    public float accelerationSpeedNormal;           //Should be around 100f
    public float accelerationSpeedSprint;           //Should be around 150f
    public float accelerationSpeedAir;              //Should be about 1/10 of normal acceleration speed
    private float maxMoveSpeedActive;               //'Active max move speed, set in Update() and Start()
    public float maxMoveSpeedNormal;                //Should be around 8f
    public float maxMoveSpeedSprint;                //Should be around 16f

    public float jumpForce;                         //Should be around 16f
    private float jumpHoldTime;                      
    public float jumpHoldTimeMax;                   //Should be around 2f

    public float wallJumpForceHoriz;                //Should be around 800f
    public float wallJumpForceVerti;                //Should be around 600f

    private bool isGrounded = false;
    private bool ableToJump = false;
    private bool isTouchingLeftWall = false;
    private bool isTouchingRightWall = false;

    public float slowingSpeed;                      //How fast the player slows down on ground, should be around 0.3f
    public float minMoveSpeed;                      //Player will stop instantly when x-velocity goes below this (on ground), should be around 1f

    private Rigidbody rb;
    public GameObject bottomLeftCorner;
    public GameObject bottomRightCorner;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        maxMoveSpeedActive = maxMoveSpeedNormal;
	}

    void Update()
    {
        //Death
        if (!isAlive)
        {
            Destroy(gameObject);
        }

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
            maxMoveSpeedActive = maxMoveSpeedSprint;
        }
        else
        {
            maxMoveSpeedActive = maxMoveSpeedNormal;
        }
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            accelerationSpeedActive = accelerationSpeedSprint;
        }
        else
        {
            accelerationSpeedActive = maxMoveSpeedNormal;
        }

        //Set movement speed in air
        if (!isGrounded)
        {
            accelerationSpeedActive = accelerationSpeedAir;
        }
        else
        {
            accelerationSpeedActive = accelerationSpeedNormal;
        }

        //Move left
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-accelerationSpeedActive, 0, 0);
        }              
        //Move right
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(accelerationSpeedActive, 0, 0);
        }
        //Friction / stopping
        if (isGrounded)
        {
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                if (rb.velocity.x < minMoveSpeed)
                {
                    rb.velocity = new Vector3(rb.velocity.x + slowingSpeed, rb.velocity.y, 0f);
                }
                else if (rb.velocity.x > minMoveSpeed)
                {
                    rb.velocity = new Vector3(rb.velocity.x - slowingSpeed, rb.velocity.y, 0);
                }
                if ((rb.velocity.x <= minMoveSpeed && rb.velocity.x >= 0) || (rb.velocity.x >= minMoveSpeed && rb.velocity.x <= 0))
                {
                    rb.velocity = new Vector3(0, rb.velocity.y, 0f);
                }
            }                  
        }
        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpHoldTime = 0;
            ableToJump = true;
            //Debug.Log("Jump check" + Time.timeSinceLevelLoad);
        }
        if (Input.GetKey(KeyCode.Space) && ableToJump)
        {
            //Debug.Log("Jump hold" + Time.timeSinceLevelLoad);

            jumpHoldTime += 10 * Time.deltaTime;

            if (jumpHoldTime <= jumpHoldTimeMax)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);                     
            }
            else if (jumpHoldTime > jumpHoldTimeMax)
            {
                jumpHoldTime = 0;
                ableToJump = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ableToJump = false;
            jumpHoldTime = 0;
            //Debug.Log("Jump release" + Time.timeSinceLevelLoad);
        }

        //Wall jump
        else
        {
            if (isTouchingLeftWall && !isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.AddForce(wallJumpForceHoriz, wallJumpForceVerti, 0);
                }
            }
            if (isTouchingRightWall && !isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.AddForce(-1 * wallJumpForceHoriz, wallJumpForceVerti, 0);
                }
            }
        } 

        //Restrict maximum movement speed
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxMoveSpeedActive, maxMoveSpeedActive), rb.velocity.y, 0f);

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
    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }
}
