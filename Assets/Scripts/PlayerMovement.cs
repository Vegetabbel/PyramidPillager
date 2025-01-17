﻿using UnityEngine;
using System.Collections;
using System.IO;

[System.Serializable]
public class IsisValues
{
    public float accelerationSpeedNormal = 100f;        //Should be around 100f
    public float accelerationSpeedSprint = 150f;        //Should be around 150f
    public float accelerationSpeedAir = 10f;            //Should be about 1/10 of normal acceleration speed

    public float maxMoveSpeedNormal = 8f;               //Should be around 8f
    public float maxMoveSpeedSprint = 16f;              //Should be around 16f

    public float jumpForce = 16f;                       //Should be around 16f
    public float jumpHoldTimeMax = 2f;                  //Should be around 2f

    public float wallJumpForceHoriz = 800f;             //Should be around 800f
    public float wallJumpForceVerti = 600f;             //Should be around 600f

    public float slowingSpeed = 0.6f;                   //How fast the player slows down on ground, should be around 0.6f
    public float minMoveSpeed = 2f;                     //Player will stop instantly when x-velocity goes below this (on ground), should be around 2f

}
[System.Serializable]
public class HawkValues
{
    public float moveSpeedVer = 150f;
    public float moveSpeedVerMax = 12f;

    public float moveSpeedHor = 100f;
    public float moveSpeedHorMax = 8f;

    public float minMoveSpeedHor = 2.5f;
    public float slowingSpeedHor = 1.2f;

    public float minMoveSpeedVer = 2f;
    public float slowingSpeedVer = 0.8f;
}
[System.Serializable]
public class CatValues
{
    public float accelerationSpeedNormal = 200f;
    public float accelerationSpeedAir = 80f;
    public float maxMoveSpeed = 20f;

    public float jumpForce = 8f;
    public float jumpHoldTimeMax = 2f;

    public float slowingSpeed = 1f;                      //How fast the player slows down on ground
    public float minMoveSpeed = 3.5f;                    //Player will stop instantly when x-velocity goes below this (on ground)
}
[System.Serializable]
public class GhostValues
{
    public float accelerationSpeedNormal = 50f;        
    public float accelerationSpeedAir = 50f;
    public float maxMoveSpeedNormal = 4f;
    public float gravityPercentage = 0.05f;             
}

public class PlayerMovement : MonoBehaviour {

    private bool isAlive = true;

    public enum PlayerState { Idle, Moving, Sprinting, Soaring, Falling, TouchingLeftWall, TouchingRightWall }
    public PlayerState playerState;

    public enum PlayerForm { Isis, Hawk, Cat, Ghost };
    public PlayerForm playerForm;

    private int equipIndex = 5; // 5: no equip
                                // 0: extra life
                                // 1: jump force +5
                                // 2: Max move speed ...normal + 3 ...sprint + 6
                                // 3: Form gauge decrease -10
                                // 4: Hold ctrl to slow time down 0.5sec

    private float IsisMaxMoveSpeedNormalNoEquip;
    private float IsisMaxMoveSpeedSprintNoEquip;
    private float IsisJumpForceNormal;

    public Object isisAnimController;
    public Object hawkAnimController;
    public Object catAnimController;
    public Object ghostAnimController;

    private float formGaugeCurrentValue;
    public float formGaugeDecreaseValue = 20;
    private float formGaugeDecreaseValueActive;
    public float formGaugeFillValue = 10;
    public float formGaugeMaxValue = 100;

    private int lives;
    private int normalLiveAmount = 3;
    private int equipLiveAmount = 4;

    //Isis values
    public IsisValues isisValues = new IsisValues();

    private float accelerationSpeedActive;              //Active move speed, set in Update()
    private float maxMoveSpeedActive;                   //Active max move speed, set in Update() and Start()
    private float jumpHoldTime;                      
   
    private bool isGrounded = false;
    private bool ableToJump = false;
    private bool isTouchingLeftWall = false;
    private bool isTouchingRightWall = false;

    //Hawk values
    public HawkValues hawkValues = new HawkValues();

    //Cat values
    public CatValues catValues = new CatValues();
    private float catAccelerationSpeedActive;               //Set in Update()
    private float catJumpHoldTime;

    //Ghost values
    public GhostValues ghostValues = new GhostValues();
    private float ghostAccelerationSpeedActive;


    private Rigidbody rb;
    private BoxCollider bc;
    private SpriteRenderer sr;
    private Animator anim;

    private float latestXpos;
    private float latestYpos;

    public GameObject bottomLeftCorner;
    public GameObject bottomRightCorner;
    public GameObject catBottomLeftCorner;
    public GameObject catBottomRightCorner;

	GameObject TutorialSpawnPoint;

    public Texture heart;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        sr = transform.Find("SpriteRenderer").GetComponent<SpriteRenderer>();
        anim = transform.Find("SpriteRenderer").GetComponent<Animator>();

        maxMoveSpeedActive = isisValues.maxMoveSpeedNormal;
        IsisMaxMoveSpeedNormalNoEquip = isisValues.maxMoveSpeedNormal;
        IsisMaxMoveSpeedSprintNoEquip = isisValues.maxMoveSpeedSprint;
        lives = normalLiveAmount;
        formGaugeDecreaseValueActive = formGaugeDecreaseValue;
        IsisJumpForceNormal = isisValues.jumpForce;

        heart = (Texture)Resources.Load("Heart");

        playerForm = PlayerForm.Isis;
        formGaugeCurrentValue = formGaugeMaxValue;

        anim.runtimeAnimatorController = (RuntimeAnimatorController)isisAnimController;
        if (!File.Exists("EquippedSave.txt"))
        {
            equipIndex = 5;
        }
        else
        {
            equipIndex = int.Parse(File.ReadAllText("EquippedSave.txt"));
        }
        print(equipIndex);
        //Check equip index 0 - Extra life
        if (equipIndex == 0)
        {
            lives = equipLiveAmount;
        }
        else
        {
            lives = normalLiveAmount;
        }

        //Check equip index 1 - Jump force +5
        if (equipIndex == 1)
        {
            isisValues.jumpForce += 5;
        }
        else
        {
            isisValues.jumpForce = IsisJumpForceNormal;
        }

        //Check equip index 2 - Max move speed ...normal + 3 ...sprint + 6
        if (equipIndex == 2)
        {
            isisValues.maxMoveSpeedNormal += 3;
            isisValues.maxMoveSpeedSprint += 6;
        }
        else
        {
            isisValues.maxMoveSpeedNormal = IsisMaxMoveSpeedNormalNoEquip;
            isisValues.maxMoveSpeedSprint = IsisMaxMoveSpeedSprintNoEquip;
        }

        //Check equip index 3 - Form gauge decrease -10
        if (equipIndex == 3 && formGaugeDecreaseValueActive != formGaugeDecreaseValue - 10)
        {
            formGaugeDecreaseValueActive -= 10;
        }
        else
        {
            formGaugeDecreaseValueActive = formGaugeDecreaseValue;
        }
    }

    void Update()
    {
        //Animator booleans
        //Flip x if moving
        anim.SetBool("isGrounded", isGrounded);
        
        if (latestXpos > transform.position.x && Input.GetKey(KeyCode.LeftArrow))
        {
            sr.flipX = true;
        }
        if (latestXpos < transform.position.x && Input.GetKey(KeyCode.RightArrow))
        {
            sr.flipX = false;
        }
        latestXpos = transform.position.x;

        //Check if going up or down

        if (rb.velocity.y != 0)
        {
            if (latestYpos > transform.position.y)
            {
                anim.SetBool("isGoingUp", false);
            }
            if (latestYpos < transform.position.y)
            {
                anim.SetBool("isGoingUp", true);
            }
        }
        //Check if falling from high
        if (rb.velocity.y != 0)
        {
            if (latestYpos - 0.2f > transform.position.y)
            {
                anim.SetBool("isFalling", true);
            }
            else
            {
                anim.SetBool("isFalling", false);
            }
        }
        else
        {
            anim.SetBool("isFalling", false);
        }
        latestYpos = transform.position.y;

        //Death
        if (!isAlive)
        {
			if (GameObject.Find("GameController"))
            {				
                if (lives < 1)
                {
                    GameObject.Find("Cameras").transform.Find("GameOver").gameObject.SetActive(true);
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                    Invoke("LoadMenu", 3f);
                }
                else
                {
                    GameObject.Find("GameController").SendMessage("Die");
                    formGaugeCurrentValue = formGaugeMaxValue;
                    lives -= 1;
                    isAlive = true;
                    playerForm = PlayerForm.Isis;
                    bc.size = new Vector3(1f, 1.8f, 1f);
                    rb.useGravity = true;
                    anim.runtimeAnimatorController = (RuntimeAnimatorController)isisAnimController;

                }
            }
            else if (GameObject.Find("SpawnPoint1"))
            {
				this.transform.position = TutorialSpawnPoint.transform.position;
                formGaugeCurrentValue = formGaugeMaxValue;
                isAlive = true;
                playerForm = PlayerForm.Isis;
                bc.size = new Vector3(1f, 1.8f, 1f);
                rb.useGravity = true;
                anim.runtimeAnimatorController = (RuntimeAnimatorController)isisAnimController;
            }
        }
        else
        { 
            //Form gauge
            if (formGaugeCurrentValue > 0 && playerForm != PlayerForm.Isis)
            {
                formGaugeCurrentValue -= formGaugeDecreaseValueActive * Time.deltaTime;
            }
            else if (formGaugeCurrentValue < formGaugeMaxValue)
            {
                formGaugeCurrentValue += formGaugeFillValue * Time.deltaTime;
            }
            if (formGaugeCurrentValue > formGaugeMaxValue)
            {
                formGaugeCurrentValue = formGaugeMaxValue;
            }
            if (formGaugeCurrentValue <= 0)
            {
                isAlive = false;
            }

            //Transformations
            if (playerForm == PlayerForm.Isis && formGaugeCurrentValue > 0 && Input.GetKey(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.Z))
            {
                playerForm = PlayerForm.Hawk;

                bc.size = new Vector3(1f, 1f, 1f);
                rb.useGravity = false;

                if (anim.runtimeAnimatorController != (RuntimeAnimatorController)hawkAnimController)
                {
                    anim.runtimeAnimatorController = (RuntimeAnimatorController)hawkAnimController;
                }
            }
            else if (playerForm == PlayerForm.Isis && formGaugeCurrentValue > 0 && Input.GetKey(KeyCode.RightArrow) && Input.GetKeyDown(KeyCode.Z)
                || playerForm == PlayerForm.Isis && formGaugeCurrentValue > 0 && Input.GetKey(KeyCode.LeftArrow) && Input.GetKeyDown(KeyCode.Z))
            {
                playerForm = PlayerForm.Cat;

                bc.size = new Vector3(1.5f, 1f, 1f);
                rb.useGravity = true;

                if (anim.runtimeAnimatorController != (RuntimeAnimatorController)catAnimController)
                {
                    anim.runtimeAnimatorController = (RuntimeAnimatorController)catAnimController;
                }
            }
            else if (playerForm == PlayerForm.Isis && formGaugeCurrentValue > 0 && Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.Z))
            {
                playerForm = PlayerForm.Ghost;

                rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);

                bc.size = new Vector3(1f, 1.8f, 1f);
                rb.useGravity = false;

                if (anim.runtimeAnimatorController != (RuntimeAnimatorController)ghostAnimController)
                {
                    anim.runtimeAnimatorController = (RuntimeAnimatorController)ghostAnimController;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                playerForm = PlayerForm.Isis;

                bc.size = new Vector3(1f, 1.8f, 1f);
                rb.useGravity = true;

                if (anim.runtimeAnimatorController != (RuntimeAnimatorController)isisAnimController)
                {
                    anim.runtimeAnimatorController = (RuntimeAnimatorController)isisAnimController;
                }
            }

            //Check equip index 4 - Hold ctrl to slow time down 50%
            if (equipIndex == 4 && Input.GetKey(KeyCode.LeftControl))
            {
                Time.timeScale = 0.5f;
            }
            else
            {
                Time.timeScale = 1;
            }

            //Change controls based on active form
            switch (playerForm)
            {
                case PlayerForm.Isis:
                    #region

                    anim.SetBool("onRightWall", isTouchingRightWall);
                    anim.SetBool("onLeftWall", isTouchingLeftWall);
                    if (!isGrounded && isTouchingLeftWall)
                    {
                        sr.flipX = false;
                    }
                    if (!isGrounded && isTouchingRightWall)
                    {
                        sr.flipX = true;
                    }

                    //Check if player is standing on something
                    if (Physics.Raycast(bottomLeftCorner.transform.position, -Vector3.up, 0.3f) ||
                        Physics.Raycast(bottomRightCorner.transform.position, -Vector3.up, 0.3f))
                    {
                        isGrounded = true;
                    }
                    else
                    {
                        isGrounded = false;
                    }

                    //Sprinting
                    if (Input.GetKey(KeyCode.X) || !isGrounded)
                    {
                        maxMoveSpeedActive = isisValues.maxMoveSpeedSprint;
                        anim.SetFloat("runSpeed", 1f);
                    }
                    else
                    {
                        maxMoveSpeedActive = isisValues.maxMoveSpeedNormal;
                        anim.SetFloat("runSpeed", 1f);
                    }
                    if (Input.GetKey(KeyCode.X) && isGrounded)
                    {
                        accelerationSpeedActive = isisValues.accelerationSpeedSprint;
                        if (Mathf.Abs(rb.velocity.x) > isisValues.maxMoveSpeedNormal)
                        {
                            anim.SetFloat("runSpeed", 1.5f);
                        }
                    }
                    else
                    {
                        accelerationSpeedActive = isisValues.maxMoveSpeedNormal;
                        anim.SetFloat("runSpeed", 1f);
                    }

                    //Set movement speed in air
                    if (!isGrounded)
                    {
                        accelerationSpeedActive = isisValues.accelerationSpeedAir;
                    }
                    else
                    {
                        accelerationSpeedActive = isisValues.accelerationSpeedNormal;
                    }

                    //Running
                    if (!(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow)))
                    {
                        anim.SetBool("isRunning", false);
                    }
                    //Move left                      
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        rb.AddForce(-accelerationSpeedActive, 0, 0);
                        anim.SetBool("isRunning", true);
                    }
                    //Move right
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        rb.AddForce(accelerationSpeedActive, 0, 0);
                        anim.SetBool("isRunning", true);
                    }                       


                    //Friction / stopping
                    if (isGrounded)
                    {
                        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
                        {
                            if (rb.velocity.x < isisValues.minMoveSpeed)
                            {
                                rb.velocity = new Vector3(rb.velocity.x + isisValues.slowingSpeed, rb.velocity.y, 0f);
                            }
                            else if (rb.velocity.x > isisValues.minMoveSpeed)
                            {
                                rb.velocity = new Vector3(rb.velocity.x - isisValues.slowingSpeed, rb.velocity.y, 0);
                            }
                            if ((rb.velocity.x <= isisValues.minMoveSpeed && rb.velocity.x >= 0) || (rb.velocity.x >= isisValues.minMoveSpeed && rb.velocity.x <= 0))
                            {
                                rb.velocity = new Vector3(0, rb.velocity.y, 0f);
                            }
                        }
                    }
                    //Jump
                    if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
                    {
                        jumpHoldTime = 0;
                        ableToJump = true;
                    }
                    if (Input.GetKey(KeyCode.UpArrow) && ableToJump)
                    {

                        jumpHoldTime += 10 * Time.deltaTime;

                        if (jumpHoldTime <= isisValues.jumpHoldTimeMax)
                        {
                            rb.velocity = new Vector3(rb.velocity.x, isisValues.jumpForce, 0);
                        }
                        else if (jumpHoldTime > isisValues.jumpHoldTimeMax)
                        {
                            jumpHoldTime = 0;
                            ableToJump = false;
                        }
                    }
                    if (Input.GetKeyUp(KeyCode.UpArrow))
                    {
                        ableToJump = false;
                        jumpHoldTime = 0;
                    }

                    //Wall jump
                    else
                    {
                        if (isTouchingLeftWall && !isGrounded)
                        {
                            if (Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                rb.velocity = new Vector3(0, 0, 0);
                                rb.AddForce(isisValues.wallJumpForceHoriz, isisValues.wallJumpForceVerti, 0);
                            }
                        }
                        if (isTouchingRightWall && !isGrounded)
                        {
                            if (Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                rb.velocity = new Vector3(0, 0, 0);
                                rb.AddForce(-1 * isisValues.wallJumpForceHoriz, isisValues.wallJumpForceVerti, 0);
                            }
                        }
                    }

                    //Restrict maximum movement speed
                    rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxMoveSpeedActive, maxMoveSpeedActive), rb.velocity.y, 0f);
                    break;
                #endregion
                case PlayerForm.Hawk:
                    #region

                    //Move left
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        rb.AddForce(-hawkValues.moveSpeedHor, 0f, 0f);
                    }
                    //Move right
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        rb.AddForce(hawkValues.moveSpeedHor, 0f, 0f);
                    }
                    //Move up
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        rb.AddForce(0f, hawkValues.moveSpeedVer, 0f);
                    }
                    //Move down
                    if (Input.GetKey(KeyCode.DownArrow))
                    {
                        rb.AddForce(0f, -hawkValues.moveSpeedVer, 0f);
                    }
                    //Stopping horizontal
                    if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
                    {
                        if (rb.velocity.x < hawkValues.minMoveSpeedHor)
                        {
                            rb.velocity = new Vector3(rb.velocity.x + hawkValues.slowingSpeedHor, rb.velocity.y, 0f);
                        }
                        else if (rb.velocity.x > hawkValues.minMoveSpeedHor)
                        {
                            rb.velocity = new Vector3(rb.velocity.x - hawkValues.slowingSpeedHor, rb.velocity.y, 0);
                        }
                        if ((rb.velocity.x <= hawkValues.minMoveSpeedHor && rb.velocity.x >= 0) || (rb.velocity.x >= hawkValues.minMoveSpeedHor && rb.velocity.x <= 0))
                        {
                            rb.velocity = new Vector3(0, rb.velocity.y, 0f);
                        }
                    }
                    //Stopping vertical
                    if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
                    {
                        if (rb.velocity.y < hawkValues.minMoveSpeedVer)
                        {
                            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + hawkValues.slowingSpeedVer, 0f);
                        }
                        else if (rb.velocity.y > hawkValues.minMoveSpeedVer)
                        {
                            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - hawkValues.slowingSpeedVer, 0f);
                        }
                        if ((rb.velocity.y <= hawkValues.minMoveSpeedVer && rb.velocity.y >= 0) || (rb.velocity.y >= hawkValues.minMoveSpeedVer && rb.velocity.y <= 0))
                        {
                            rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);
                        }
                    }

                    //Restrict move speed to max
                    rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -hawkValues.moveSpeedHorMax, hawkValues.moveSpeedHorMax),
                                              Mathf.Clamp(rb.velocity.y, -hawkValues.moveSpeedVerMax, hawkValues.moveSpeedVerMax),
                                                            0f);

                    break;
                #endregion
                case PlayerForm.Cat:
                    #region

                    //Check if player is standing on something
                    if (Physics.Raycast(catBottomLeftCorner.transform.position, -Vector3.up, 0.2f) ||
                        Physics.Raycast(catBottomRightCorner.transform.position, -Vector3.up, 0.2f))
                    {
                        isGrounded = true;
                    }
                    else
                    {
                        isGrounded = false;
                    }

                    //Set movement speed in air
                    if (!isGrounded)
                    {
                        catAccelerationSpeedActive = catValues.accelerationSpeedAir;
                    }
                    else
                    {
                        catAccelerationSpeedActive = catValues.accelerationSpeedNormal;
                    }

                    //Move left
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        rb.AddForce(-catAccelerationSpeedActive, 0, 0);
                    }
                    //Move right
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        rb.AddForce(catAccelerationSpeedActive, 0, 0);
                    }
                    //Friction / stopping
                    if (isGrounded)
                    {
                        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
                        {
                            if (rb.velocity.x < catValues.minMoveSpeed)
                            {
                                rb.velocity = new Vector3(rb.velocity.x + catValues.slowingSpeed, rb.velocity.y, 0f);
                            }
                            else if (rb.velocity.x > catValues.minMoveSpeed)
                            {
                                rb.velocity = new Vector3(rb.velocity.x - catValues.slowingSpeed, rb.velocity.y, 0);
                            }
                            if ((rb.velocity.x <= catValues.minMoveSpeed && rb.velocity.x >= 0) || (rb.velocity.x >= catValues.minMoveSpeed && rb.velocity.x <= 0))
                            {
                                rb.velocity = new Vector3(0, rb.velocity.y, 0f);
                            }
                        }
                    }
                    //Jump
                    if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
                    {
                        catJumpHoldTime = 0;
                        ableToJump = true;
                        //Debug.Log("Jump check" + Time.timeSinceLevelLoad);
                    }
                    if (Input.GetKey(KeyCode.UpArrow) && ableToJump)
                    {
                        //Debug.Log("Jump hold" + Time.timeSinceLevelLoad);

                        catJumpHoldTime += 10 * Time.deltaTime;

                        if (catJumpHoldTime <= catValues.jumpHoldTimeMax)
                        {
                            rb.velocity = new Vector3(rb.velocity.x, catValues.jumpForce, 0);
                        }
                        else if (catJumpHoldTime > catValues.jumpHoldTimeMax)
                        {
                            catJumpHoldTime = 0;
                            ableToJump = false;
                        }
                    }
                    if (Input.GetKeyUp(KeyCode.UpArrow))
                    {
                        ableToJump = false;
                        catJumpHoldTime = 0;
                        //Debug.Log("Jump release" + Time.timeSinceLevelLoad);
                    }

                    //Restrict maximum movement speed
                    rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -catValues.maxMoveSpeed, catValues.maxMoveSpeed), rb.velocity.y, 0f);

                    break;
                #endregion
                case PlayerForm.Ghost:
                    #region

                    //Lower gravity
                    rb.AddForce(-Vector3.up * ghostValues.gravityPercentage * Physics.gravity.magnitude);

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

                    //Set movement speed in air
                    if (!isGrounded)
                    {
                        ghostAccelerationSpeedActive = ghostValues.accelerationSpeedAir;
                    }
                    else
                    {
                        ghostAccelerationSpeedActive = ghostValues.accelerationSpeedNormal;
                    }

                    //Move left
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        rb.AddForce(-ghostAccelerationSpeedActive, 0, 0);
                    }
                    //Move right
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        rb.AddForce(ghostAccelerationSpeedActive, 0, 0);
                    }
                    //Friction / stopping
                    if (isGrounded)
                    {
                        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
                        {
                            if (rb.velocity.x < isisValues.minMoveSpeed)
                            {
                                rb.velocity = new Vector3(rb.velocity.x + isisValues.slowingSpeed, rb.velocity.y, 0f);
                            }
                            else if (rb.velocity.x > isisValues.minMoveSpeed)
                            {
                                rb.velocity = new Vector3(rb.velocity.x - isisValues.slowingSpeed, rb.velocity.y, 0);
                            }
                            if ((rb.velocity.x <= isisValues.minMoveSpeed && rb.velocity.x >= 0) || (rb.velocity.x >= isisValues.minMoveSpeed && rb.velocity.x <= 0))
                            {
                                rb.velocity = new Vector3(0, rb.velocity.y, 0f);
                            }
                        }
                    }
                    //Restrict maximum movement speed
                    rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -ghostValues.maxMoveSpeedNormal, ghostValues.maxMoveSpeedNormal), rb.velocity.y, 0f);

                    break;
                #endregion
                default:
                    Debug.Log("No form selected");
                    break;
            }
        }
    
    }

    void OnGUI()
    {
        for (int i = 0; i < lives; i++)
        {
            GUI.Label(new Rect(new Vector2(30 + i*70, 20), new Vector2(60,60)), heart);
        }
        GUI.HorizontalSlider(new Rect(new Vector2(28, 90), new Vector2(200, 200)), formGaugeCurrentValue, 0f, 100f);
    }

	void TutorialCollider (GameObject spawn) {
		TutorialSpawnPoint = spawn;
	}

    private void LoadMenu()
    {
        Application.LoadLevel("MenuScene");
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
    public float FormGaugeCurrentValue
    {
        get { return formGaugeCurrentValue; }
        set { formGaugeCurrentValue = value; }
    }
}
