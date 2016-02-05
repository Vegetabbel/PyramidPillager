using UnityEngine;
using System.Collections;

public class MovingSpikes : MonoBehaviour {

    public float moveSpeedLeft = 2;
    public float moveSpeedRight = 2;
    public float stopTimeLeft = 0.5f;
    public float stopTimeRight = 0.5f;
    public bool startMovingRight = true;

    public GameObject leftPoint;
    public GameObject rightPoint;

    private bool atLeftPoint;

	void Start ()
    {
        if (startMovingRight)
        {
            atLeftPoint = true;
            moveRight();
        }
        else
        {
            atLeftPoint = false;
            moveLeft();
        }
    }

    void Update()
    {
        if (atLeftPoint)
        {
            Invoke("moveRight", stopTimeLeft);
        }
        else
        {
            Invoke("moveLeft", stopTimeRight);
        }
    }

    private void moveLeft()
    {
        if (!(transform.position.x - moveSpeedLeft * Time.deltaTime <= leftPoint.transform.position.x))
        {
            if (transform.position.x > leftPoint.transform.position.x)
            {
                transform.position = new Vector3(transform.position.x - moveSpeedLeft * Time.deltaTime, transform.position.y, 0f);
            }
            else
            {
                transform.position = leftPoint.transform.position;
                atLeftPoint = true;
            }
        }
        else
        {
            transform.position = leftPoint.transform.position;
            atLeftPoint = true;
        }
    }
    private void moveRight()
    {
        if (!(transform.position.x + moveSpeedRight * Time.deltaTime >= rightPoint.transform.position.x))
        {
            if (transform.position.x < rightPoint.transform.position.x)
            {
                transform.position = new Vector3(transform.position.x + moveSpeedRight * Time.deltaTime, transform.position.y, 0f);
            }
            else
            {
                transform.position = rightPoint.transform.position;
                atLeftPoint = false;
            }
        }
        else
        {
            transform.position = rightPoint.transform.position;
            atLeftPoint = false;
        }
    }
}
