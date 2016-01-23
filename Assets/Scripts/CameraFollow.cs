using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;

    void Update()
    {
        if (target)
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z - 10f);
        }
    }
}
