using UnityEngine;
using System.Collections;

public class FallingSpikes : MonoBehaviour {

    public float fallingSpeed = 5;
    public float detectionRange = 20;
    private bool fallen = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter()
    {
        rb.velocity = new Vector3(0f,0f,0f);
        fallen = true;
    }

	void Update ()
    {
        if (!fallen)
        {
            RaycastHit hit;

            Debug.DrawLine(new Vector3((transform.position.x - (transform.localScale.x / 2)), transform.position.y, transform.position.z), new Vector3((transform.position.x - (transform.localScale.x / 2)), transform.position.y - detectionRange, transform.position.z), Color.green, 0.2f);
            Debug.DrawLine(new Vector3((transform.position.x + (transform.localScale.x / 2)), transform.position.y, transform.position.z), new Vector3((transform.position.x + (transform.localScale.x / 2)), transform.position.y - detectionRange, transform.position.z), Color.green, 0.2f);

            if (Physics.Raycast(new Vector3((transform.position.x - (transform.localScale.x / 2)), transform.position.y, transform.position.z),
                -Vector3.up, out hit, detectionRange) ||
                Physics.Raycast(new Vector3((transform.position.x + (transform.localScale.x / 2)), transform.position.y, transform.position.z),
                -Vector3.up, out hit, detectionRange))
            {
                if (hit.transform.tag == "Player")
                {
                    rb.isKinematic = false;
                    rb.velocity = new Vector3(0f, -fallingSpeed, 0f);
                }
            }
        }
	}
}
