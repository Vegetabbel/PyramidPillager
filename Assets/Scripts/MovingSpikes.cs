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

        public GameObject leftPoint;
        public GameObject rightPoint;
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

        public GameObject topPoint;
        public GameObject bottomPoint;
    }
    public VerticalValues verticalValues = new VerticalValues();

    private bool atLeftPoint;
    private bool atTopPoint;

	void Start ()
    {
        switch (axis)
        {
            case Axis.Horizontal:
                if (horizontalValues.startMovingRight)
                {
                    atLeftPoint = true;
                    moveRight();
                }
                else
                {
                    atLeftPoint = false;
                    moveLeft();
                }
                break;
            case Axis.Vertical:
                if (verticalValues.startMovingDown)
                {
                    atTopPoint = true;
                    moveDown();
                }
                else
                {
                    atTopPoint = false;
                    moveUp();
                }
                break;
            default:
                Debug.Log("No axis selected");
                break;
        }
    }

    void Update()
    {
        switch (axis)
        {
            case Axis.Horizontal:
                if (atLeftPoint)
                {
                    Invoke("moveRight", horizontalValues.stopTimeLeft);
                }
                else
                {
                    Invoke("moveLeft", horizontalValues.stopTimeRight);
                }
                break;
            case Axis.Vertical:
                if (atTopPoint)
                {
                    Invoke("moveDown", verticalValues.stopTimeTop);
                }
                else
                {
                    Invoke("moveUp", verticalValues.stopTimeBottom);
                }
                break;
            default:
                break;
        }

    }

    private void moveLeft()
    {
        if (!(transform.position.x - horizontalValues.moveSpeedLeft * Time.deltaTime <= horizontalValues.leftPoint.transform.position.x))
        {
            if (transform.position.x > horizontalValues.leftPoint.transform.position.x)
            {
                transform.position = new Vector3(transform.position.x - horizontalValues.moveSpeedLeft * Time.deltaTime, transform.position.y, 0f);
            }
            else
            {
                transform.position = horizontalValues.leftPoint.transform.position;
                atLeftPoint = true;
            }
        }
        else
        {
            transform.position = horizontalValues.leftPoint.transform.position;
            atLeftPoint = true;
        }
    }
    private void moveRight()
    {
        if (!(transform.position.x + horizontalValues.moveSpeedRight * Time.deltaTime >= horizontalValues.rightPoint.transform.position.x))
        {
            if (transform.position.x < horizontalValues.rightPoint.transform.position.x)
            {
                transform.position = new Vector3(transform.position.x + horizontalValues.moveSpeedRight * Time.deltaTime, transform.position.y, 0f);
            }
            else
            {
                transform.position = horizontalValues.rightPoint.transform.position;
                atLeftPoint = false;
            }
        }
        else
        {
            transform.position = horizontalValues.rightPoint.transform.position;
            atLeftPoint = false;
        }
    }

    private void moveUp()
    {
        if (!(transform.position.y + verticalValues.moveSpeedUp * Time.deltaTime >= verticalValues.topPoint.transform.position.y))
        {
            if (transform.position.y < verticalValues.topPoint.transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + verticalValues.moveSpeedUp * Time.deltaTime, 0f);
            }
            else
            {
                transform.position = verticalValues.topPoint.transform.position;
                atTopPoint = true;
            }
        }
        else
        {
            transform.position = verticalValues.topPoint.transform.position;
            atTopPoint = true;
        }
    }
    private void moveDown()
    {
        if (!(transform.position.y - verticalValues.moveSpeedDown * Time.deltaTime <= verticalValues.bottomPoint.transform.position.y))
        {
            if (transform.position.y > verticalValues.bottomPoint.transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - verticalValues.moveSpeedDown * Time.deltaTime, 0f);
            }
            else
            {
                transform.position = verticalValues.bottomPoint.transform.position;
                atTopPoint = false;
            }
        }
        else
        {
            transform.position = verticalValues.bottomPoint.transform.position;
            atTopPoint = false;
        }
    }
}
