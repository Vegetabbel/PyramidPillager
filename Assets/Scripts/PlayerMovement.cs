using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody rb;
    private float moveSpeedPhysics = 50f;
    private float moveSpeedArcade = 10f;
    private float jumpForce = 50f;

    public enum Controls {Physics, Arcade};
    public Controls controlState;


	void Start ()
    {
        rb = GetComponent<Rigidbody>();
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
                if (Input.GetKey(KeyCode.Space))
                {
                    rb.AddForce(0, jumpForce, 0);
                }
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
                if (Input.GetKey(KeyCode.Space))
                {
                    rb.AddForce(0, jumpForce, 0);
                }
                break;

            default:
                Debug.Log("No control type selected");
                break;
        }
    }

	void Update ()
    {
             
    }
}
