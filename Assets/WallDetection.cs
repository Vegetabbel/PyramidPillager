using UnityEngine;
using System.Collections;

public class WallDetection : MonoBehaviour {

    private PlayerMovement playerScript;
    public enum PlayerSide {LeftSide, RightSide};
    public PlayerSide ps;

    void Awake()
    {
        playerScript = GetComponentInParent<PlayerMovement>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "wall")
        {
            if (ps == PlayerSide.LeftSide)
            {
                playerScript.IsTouchingLeftWall = true;
            }
            if (ps == PlayerSide.RightSide)
            {
                playerScript.IsTouchingRightWall = true;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "wall")
        {
            playerScript.IsTouchingLeftWall = false;
            playerScript.IsTouchingRightWall = false;
        }
    }
}
