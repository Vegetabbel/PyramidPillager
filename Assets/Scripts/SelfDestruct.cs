using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

    public float delay;

	void Start()
    {
        Invoke("SelfDestruction", delay);
    }
    private void SelfDestruction()
    {
        Destroy(gameObject);
    }
}
