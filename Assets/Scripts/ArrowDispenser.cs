using UnityEngine;
using System.Collections;

public class ArrowDispenser : MonoBehaviour {

    private objectPool objectPool;

    public GameObject arrow;
    public GameObject destination;
    private Vector3 direction;

    private bool canShoot = true;
    public float delay = 1.5f;
    public float speed = 5;

	void Start ()
    {
        objectPool = GameObject.FindGameObjectWithTag("GameController").GetComponent<objectPool>();
        direction = Vector3.Normalize(destination.transform.position - transform.position);
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
                objectPool.arrowPool[i].transform.LookAt(destination.transform);
                objectPool.arrowPool[i].GetComponent<ArrowScript>().ArrowIsActive = true;
                objectPool.arrowPool[i].GetComponent<ArrowScript>().GetParentStatueValues(gameObject);
                break;
            }
            else if (i == objectPool.arrowPool.Count - 1)
            {
                GameObject arrow = objectPool.createNewArrow();
                arrow.SetActive(true);
                arrow.transform.position = transform.position;
                arrow.transform.LookAt(destination.transform);
                arrow.GetComponent<ArrowScript>().ArrowIsActive = true;
                arrow.GetComponent<ArrowScript>().GetParentStatueValues(gameObject);
                break;
                //Debug.Log("lol");
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
