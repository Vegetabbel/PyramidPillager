using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody rb;
    private float moveSpeed = 10;

    public enum Controls {Physics, Arcade};
    public Controls controlState;


	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	void Update ()
    {
        switch (controlState)
        {
            case Controls.Physics:
                //move left
                if (Input.GetKey(KeyCode.A))
                {
                    rb.AddForce(-10 * moveSpeed, 0, 0);
                }
                //move right
                if (Input.GetKey(KeyCode.D))
                {
                    rb.AddForce(10 * moveSpeed, 0, 0);
                }
                break;
            case Controls.Arcade:

                break;
            default:
                Debug.Log("No controls type selected");
                break;
        }
        
    }
}
