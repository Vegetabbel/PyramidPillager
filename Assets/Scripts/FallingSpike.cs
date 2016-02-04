using UnityEngine;
using System.Collections;

public class FallingSpike : MonoBehaviour
{

    public float fallingSpeed = 5;
    public float detectionRange = 20;
    private bool fallen = false;
    private Rigidbody rb;

    public GameObject triggeredText;
    public Material triggeredMaterial;
    private bool triggered;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter()
    {
        rb.isKinematic = true;
        rb.velocity = new Vector3(0f, 0f, 0f);
        fallen = true;
    }
    void OnTriggerExit()
    {
        rb.isKinematic = false;
        rb.velocity = new Vector3(0f, -fallingSpeed, 0f);
        fallen = false;
    }

    void Update()
    {
        if (!fallen)
        {
            RaycastHit hit;
            RaycastHit hit2;

            Debug.DrawLine(new Vector3((transform.position.x - (transform.localScale.x / 2)), transform.position.y, transform.position.z),
                new Vector3((transform.position.x - (transform.localScale.x / 2)), transform.position.y - detectionRange, transform.position.z),
                Color.green, 0.2f);
            Debug.DrawLine(new Vector3((transform.position.x + (transform.localScale.x / 2)), transform.position.y, transform.position.z),
                new Vector3((transform.position.x + (transform.localScale.x / 2)), transform.position.y - detectionRange, transform.position.z),
                Color.green, 0.2f);

            if (Physics.Raycast(new Vector3((transform.position.x - (transform.localScale.x / 2)), transform.position.y, transform.position.z),
                -Vector3.up, out hit, detectionRange))
            {
                if (hit.transform.tag == "Player")
                {
                    rb.isKinematic = false;
                    rb.velocity = new Vector3(0f, -fallingSpeed, 0f);

                    if (!triggered)
                    {
                        Instantiate(triggeredText, transform.position, Quaternion.identity);
                        GetComponent<Renderer>().material = triggeredMaterial;
                        triggered = true;
                    }
                }
            }
            if (Physics.Raycast(new Vector3((transform.position.x + (transform.localScale.x / 2)), transform.position.y, transform.position.z),
                -Vector3.up, out hit2, detectionRange))
            {
                if (hit2.transform.tag == "Player")
                {
                    rb.isKinematic = false;
                    rb.velocity = new Vector3(0f, -fallingSpeed, 0f);

                    if (!triggered)
                    {
                        Instantiate(triggeredText, transform.position, Quaternion.identity);
                        GetComponent<Renderer>().material = triggeredMaterial;
                        triggered = true;
                    }
                }
            }
        }
    }
}
