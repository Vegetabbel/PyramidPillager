using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {

    private bool arrowIsActive = false;

    private Vector3 direction;
    private float speed;

    private GameObject parentStatue;
	
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != parentStatue)
        {
            if (other.gameObject.tag == "ignore")
            {
                return;
            }
            else
            {
                arrowIsActive = false;
                gameObject.SetActive(false);
            }          
        }        
    }

	void Update ()
    {
        if (arrowIsActive)
        {
            move(direction, speed);
        }
	}


    public void move(Vector3 direction, float speed)
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    public void GetParentStatueValues(GameObject parentStatue)
    {
        this.parentStatue = parentStatue;
        ArrowDispenser arrowDispScript;
        arrowDispScript = parentStatue.GetComponent<ArrowDispenser>();
        speed = arrowDispScript.speed;
        direction = arrowDispScript.Direction;
    }
    public bool ArrowIsActive
    {
        get { return arrowIsActive; }
        set { arrowIsActive = value; }
    }
}
