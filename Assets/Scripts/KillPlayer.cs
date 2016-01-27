using UnityEngine;
using System.Collections;

public class KillPlayer : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInParent<PlayerMovement>().IsAlive = false;
        }
    }

}
