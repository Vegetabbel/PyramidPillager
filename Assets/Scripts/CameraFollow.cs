using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public bool showBox = true;
    public float cameraDistance;
    public GameObject target;
    private Collider tcollider;
    public Vector2 focusAreaSize;

    public float verticalOffset;
    public float lookAheadDstX;
    public float lookSmoothTimeX;
    public float verticalSmoothTime;

    private float currentLookAheadX;
    private float targetLookAheadX;
    private float lookAheadDirX;
    private float smoothLookVelocityX;
    private float smoothVelocityY;

    bool lookAheadStopped;

    FocusArea focusArea;

    void Start()
    {
        tcollider = target.GetComponent<Collider>();
        focusArea = new FocusArea(tcollider.bounds, focusAreaSize);
    }

    void LateUpdate()
    {
        focusArea.Update (tcollider.bounds);

        Vector2 focusPosition = focusArea.centre + Vector2.up * verticalOffset;

        if (focusArea.velocity.x != 0)
        {
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
            if ((Input.GetKey(KeyCode.A) && focusArea.velocity.x < 0) || (Input.GetKey(KeyCode.D) && focusArea.velocity.x > 0) && (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D))))
            {
                lookAheadStopped = false;
                targetLookAheadX = lookAheadDirX * lookAheadDstX;
            }
            else
            {
                if (!lookAheadStopped)
                {
                    lookAheadStopped = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX) / 4f;
                }
            }
        }

        
        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
        focusPosition += Vector2.right * currentLookAheadX;
        transform.position = (Vector3)focusPosition + Vector3.forward * -cameraDistance;
    }

    void OnDrawGizmos()
    {
        if (showBox)
        {
            Gizmos.color = new Color(1, 0, 0, .5f);
            Gizmos.DrawCube(focusArea.centre, focusAreaSize);
        }
    }

    struct FocusArea
    {
        public Vector2 centre;
        public Vector2 velocity;
        private float left, right;
        private float top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }   
}
