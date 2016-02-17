using UnityEngine;
using System.Collections;

public class FormGaugePickup : MonoBehaviour {

    public float value = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().FormGaugeCurrentValue += value;
            Destroy(gameObject);
        }
    }
}
