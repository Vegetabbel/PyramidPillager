using UnityEngine;
using System.Collections;

public class ArrowDispenser : MonoBehaviour {

    public GameObject arrow;
    public GameObject destination;
    private Vector3 direction;

    private bool canShoot = true;
    public float delay = 1.5f;
    public float speed = 5;

	void Start ()
    {
        direction = destination.transform.position - transform.position;
    }

    void Update ()
    {
        if (canShoot)
        {
            canShoot = false;
            Invoke("shootArrow", delay);
        }
	}

    private void shootArrow()
    {
        for (int i = 0; i < objectPool.arrowPool.Count; i++)
        {
            if (!objectPool.arrowPool[i].activeInHierarchy)
            {
                objectPool.arrowPool[i].SetActive(true);
                objectPool.arrowPool[i].transform.position = transform.position;
                //APPLY ROTATION HERE
                objectPool.arrowPool[i].GetComponent<ArrowScript>().ArrowIsActive = true;
                objectPool.arrowPool[i].GetComponent<ArrowScript>().GetParentStatueValues(gameObject);
                break;
            }
        }
        canShoot = true;
    }
    public Vector3 Direction
    {
        get { return direction; }
        set { direction = value; }
    }
}
