using UnityEngine;
using System.Collections;

public class BackgroundSlide : MonoBehaviour {

    public int slideSpeed = 2;
	
	void Update ()
    {
        if (transform.position.x - slideSpeed * Time.deltaTime <= -30.72f)
        {
            transform.position = new Vector3(30.72f - transform.position.x - slideSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - slideSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        }
	}
}
