using UnityEngine;
using System.Collections;

public class MovingSpikes : MonoBehaviour {

    public enum Axis { Horizontal, Vertical };
    public Axis axis = Axis.Horizontal;

    [System.Serializable]
    public class HorizontalValues
    {
        public float moveSpeedLeft = 2;
        public float moveSpeedRight = 2;
        public float stopTimeLeft = 0.5f;
        public float stopTimeRight = 0.5f;
        public bool startMovingRight = true;
    }
    public HorizontalValues horizontalValues = new HorizontalValues();
    [System.Serializable]
    public class VerticalValues
    {
        public float moveSpeedUp = 2;
        public float moveSpeedDown = 2;
        public float stopTimeTop = 0.5f;
        public float stopTimeBottom = 0.5f;
        public bool startMovingDown = true;
    }
    public VerticalValues verticalValues = new VerticalValues();

    private GameObject leftPoint;
    private GameObject rightPoint;
    private GameObject topPoint;
    private GameObject bottomPoint;

    private float stopTime;

    private bool moving = true;
    private bool atLeftPoint;
    private bool atTopPoint;

    void Start ()
    {
        if (gameObject.transform.parent.Find("Left Point").gameObject != null)
        {
            leftPoint = gameObject.transform.parent.Find("Left Point").gameObject;
        }
        if (gameObject.transform.parent.Find("Right Point").gameObject != null)
        {
            rightPoint = gameObject.transform.parent.Find("Right Point").gameObject;
        }
        if (gameObject.transform.parent.Find("Top Point").gameObject != null)
        {
            topPoint = gameObject.transform.parent.Find("Top Point").gameObject;
        }
        if (gameObject.transform.parent.Find("Bottom Point").gameObject != null)
        {
            bottomPoint = gameObject.transform.parent.Find("Bottom Point").gameObject;
        }

        switch (axis)
        {          
            case Axis.Horizontal:
                if (horizontalValues.startMovingRight)
                {
                    atLeftPoint = true;
                }
                else
                {
                    atLeftPoint = false;
                }
                break;
            case Axis.Vertical:
                if (verticalValues.startMovingDown)
                {
                    atTopPoint = true;
                }
                else
                {
                    atTopPoint = false;
                }
                break;
            default:
                Debug.Log("No axis selected");
                break;
        }
    }

    void Update()
    {
        if (moving)
        {
            switch (axis)
            {
                case Axis.Horizontal:
                    if (atLeftPoint)
                    {
                        moveRight();
                    }
                    else if (!atLeftPoint)
                    {
                        moveLeft();
                    }
                    break;
                case Axis.Vertical:
                    if (atTopPoint)
                    {
                        moveDown();
                    }
                    else if (!atTopPoint)
                    {
                        moveUp();
                    }
                    break;
                default:
                    Debug.Log("No axis selected");
                    break;
            }
        }
        else if (!IsInvoking("ToggleMovement"))
        {
            Invoke("ToggleMovement", stopTime);
        }
    }

    private void ToggleMovement()
    {
        moving = true;
    }

    private void moveLeft()
    {
        if (leftPoint != null)
        {
            if (transform.position.x - horizontalValues.moveSpeedLeft * Time.deltaTime <= leftPoint.transform.position.x)
            {
                transform.position = new Vector3(leftPoint.transform.position.x, leftPoint.transform.position.y, 0f);
                atLeftPoint = true;
                moving = false;
                stopTime = horizontalValues.stopTimeLeft;
            }
            else
            {
                transform.position = new Vector3(transform.position.x - horizontalValues.moveSpeedLeft * Time.deltaTime, transform.position.y, 0f);
            }
        }
        else
        {
            Debug.Log("Left Point has been deleted/does not exist");
        }
    }

    private void moveRight()
    {
        if (rightPoint != null)
        {
            if (transform.position.x + horizontalValues.moveSpeedRight * Time.deltaTime >= rightPoint.transform.position.x)
            {
                transform.position = new Vector3(rightPoint.transform.position.x, rightPoint.transform.position.y, 0f);
                atLeftPoint = false;
                moving = false;
                stopTime = horizontalValues.stopTimeRight;
            }
            else
            {
                transform.position = new Vector3(transform.position.x + horizontalValues.moveSpeedRight * Time.deltaTime, transform.position.y, 0f);
            }
        }
        else
        {
            Debug.Log("Right Point has been deleted/does not exist");
        }
    }

    private void moveUp()
    {
        if (topPoint != null)
        {
            if (transform.position.y + verticalValues.moveSpeedUp * Time.deltaTime >= topPoint.transform.position.y)
            {
                transform.position = new Vector3(topPoint.transform.position.x, topPoint.transform.position.y, 0f);
                atTopPoint = true;
                moving = false;
                stopTime = verticalValues.stopTimeTop;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + verticalValues.moveSpeedUp * Time.deltaTime, 0f);
            }
        }
        else
	    {
            Debug.Log("Top Point has been deleted/does not exist");
        }
    }
    private void moveDown()
    {
        if (bottomPoint != null)
        {
            if (transform.position.y - verticalValues.moveSpeedDown * Time.deltaTime <= bottomPoint.transform.position.y)
            {
                transform.position = new Vector3(bottomPoint.transform.position.x, bottomPoint.transform.position.y, 0f);
                atTopPoint = false;
                moving = false;
                stopTime = verticalValues.stopTimeBottom;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - verticalValues.moveSpeedDown * Time.deltaTime, 0f);
            }
        }
        else
	    {
            Debug.Log("Bottom Point has been deleted/does not exist");
        }
    }
}
