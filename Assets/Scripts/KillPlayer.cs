using UnityEngine;
using System.Collections;

public class KillPlayer : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponentInParent<PlayerMovement>().playerForm != PlayerMovement.PlayerForm.Ghost)
            {
                other.GetComponentInParent<PlayerMovement>().IsAlive = false;
            }
        }
    }
}
