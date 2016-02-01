﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private bool isAlive = true;

    public enum PlayerState { Idle, Moving, Sprinting, Soaring, Falling, TouchingLeftWall, TouchingRightWall }
    public PlayerState playerState;

    public enum PlayerForm { Isis, Hawk, Cat, Ghost };
    public PlayerForm playerForm;

    public Sprite IsisSprite;
    public Sprite HawkSprite;
    public Sprite CatSprite;
    public Sprite GhostSprite;

    private float formGaugeCurrentValue = 1;
    public float formGaugeMaxValue;

    private float accelerationSpeedActive;          //Active move speed, set in Update()
    public float accelerationSpeedNormal = 100f;           //Should be around 100f
    public float accelerationSpeedSprint = 150f;           //Should be around 150f
    public float accelerationSpeedAir = 10f;              //Should be about 1/10 of normal acceleration speed
    private float maxMoveSpeedActive;               //'Active max move speed, set in Update() and Start()
    public float maxMoveSpeedNormal = 8f;                //Should be around 8f
    public float maxMoveSpeedSprint = 16f;                //Should be around 16f

    public float jumpForce = 16f;                         //Should be around 16f
    private float jumpHoldTime;                      
    public float jumpHoldTimeMax = 2f;                   //Should be around 2f

    public float wallJumpForceHoriz = 800f;                //Should be around 800f
    public float wallJumpForceVerti = 600f;                //Should be around 600f

    public float slowingSpeed = 0.6f;                      //How fast the player slows down on ground, should be around 0.6f
    public float minMoveSpeed = 2f;                      //Player will stop instantly when x-velocity goes below this (on ground), should be around 2f

    private bool isGrounded = false;
    private bool ableToJump = false;
    private bool isTouchingLeftWall = false;
    private bool isTouchingRightWall = false;

    //Hawk values
    public float hawkMoveSpeedVer = 150f;
    public float hawkMoveSpeedVerMax = 12f;

    public float hawkMoveSpeedHor = 100f;
    public float hawkMoveSpeedHorMax = 8f;

    public float hawkMinMoveSpeedHor = 2.5f;
    public float hawkSlowingSpeedHor = 1.2f;

    public float hawkMinMoveSpeedVer = 2f;
    public float hawkSlowingSpeedVer = 0.8f;

    private Rigidbody rb;
    private Transform tr;
    private BoxCollider bc;
    private SpriteRenderer sr;
    public GameObject bottomLeftCorner;
    public GameObject bottomRightCorner;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        bc = GetComponent<BoxCollider>();
        sr = GetComponent<SpriteRenderer>();
        maxMoveSpeedActive = maxMoveSpeedNormal;
        playerForm = PlayerForm.Isis;
    }

    void Update()
    {
        //Debug.Log();

        //Death
        if (!isAlive)
        {
            Destroy(gameObject);
        }

        //Transformations
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerForm = PlayerForm.Isis;
        }
        if (formGaugeCurrentValue > 0 && Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.Mouse0))
        {
            playerForm = PlayerForm.Hawk;
        }
        if (formGaugeCurrentValue > 0 && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Mouse0))
        {
            playerForm = PlayerForm.Cat;
        }
        if (formGaugeCurrentValue > 0 && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Mouse0))
        {
            playerForm = PlayerForm.Ghost;
        }

        //Change controls based on active form
        switch (playerForm)
        {
            case PlayerForm.Isis:
                #region

                bc.size = new Vector3(1f, 1.8f, 1f);
                rb.useGravity = true;
                sr.sprite = IsisSprite;

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
                break;
            #endregion
            case PlayerForm.Hawk:
                #region
                bc.size = new Vector3(1f, 1f, 1f);
                rb.useGravity = false;
                sr.sprite = HawkSprite;

                //Move left
                if (Input.GetKey(KeyCode.A))
                {
                    rb.AddForce(-hawkMoveSpeedHor, 0f, 0f);
                }
                //Move right
                if (Input.GetKey(KeyCode.D))
                {
                    rb.AddForce(hawkMoveSpeedHor, 0f, 0f);
                }
                //Move up
                if (Input.GetKey(KeyCode.W))
                {
                    rb.AddForce(0f, hawkMoveSpeedVer, 0f);
                }
                //Move down
                if (Input.GetKey(KeyCode.S))
                {
                    rb.AddForce(0f, -hawkMoveSpeedVer, 0f);
                }
                //Stopping horizontal
                if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                    {
                        if (rb.velocity.x < hawkMinMoveSpeedHor)
                        {
                            rb.velocity = new Vector3(rb.velocity.x + hawkSlowingSpeedHor, rb.velocity.y, 0f);
                        }
                        else if (rb.velocity.x > hawkMinMoveSpeedHor)
                        {
                            rb.velocity = new Vector3(rb.velocity.x - hawkSlowingSpeedHor, rb.velocity.y, 0);
                        }
                        if ((rb.velocity.x <= hawkMinMoveSpeedHor && rb.velocity.x >= 0) || (rb.velocity.x >= hawkMinMoveSpeedHor && rb.velocity.x <= 0))
                        {
                            rb.velocity = new Vector3(0, rb.velocity.y, 0f);
                        }
                    }
                //Stopping vertical
                if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                {
                    if (rb.velocity.y < hawkMinMoveSpeedVer)
                    {
                        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + hawkSlowingSpeedVer, 0f);
                    }
                    else if (rb.velocity.y > hawkMinMoveSpeedVer)
                    {
                        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - hawkSlowingSpeedVer, 0f);
                    }
                    if ((rb.velocity.y <= hawkMinMoveSpeedVer && rb.velocity.y >= 0) || (rb.velocity.y >= hawkMinMoveSpeedVer && rb.velocity.y <= 0))
                    {
                        rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);
                    }
                }

                //Restrict move speed to max
                rb.velocity = new Vector3(Mathf.Clamp(  rb.velocity.x, -hawkMoveSpeedHorMax, hawkMoveSpeedHorMax),
                                          Mathf.Clamp(  rb.velocity.y, -hawkMoveSpeedVerMax, hawkMoveSpeedVerMax),
                                                        0f);

                break;
            #endregion
            case PlayerForm.Cat:
                #region
                break;
            #endregion
            case PlayerForm.Ghost:
                #region
                break;
            #endregion
            default:
                Debug.Log("No form selected");
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
    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }
}
