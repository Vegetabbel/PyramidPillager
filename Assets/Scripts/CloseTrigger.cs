using UnityEngine;
using System.Collections;

public class CloseTrigger : MonoBehaviour
{
    private GameObject doorLeft;
    private GameObject doorTop;
    private GameObject doorRight;
    private GameObject doorBottom;
    private bool doorsClosed = false;

    void Awake()
    {
        doorLeft = GetComponentInParent<ExitLocation>().transform.Find("Entrypoints/Left/Door").gameObject;
        doorLeft.SetActive(false);
        doorTop = GetComponentInParent<ExitLocation>().transform.Find("Entrypoints/Top/Door").gameObject;
        doorTop.SetActive(false);
        doorRight = GetComponentInParent<ExitLocation>().transform.Find("Entrypoints/Right/Door").gameObject;
        doorRight.SetActive(false);
        doorBottom = GetComponentInParent<ExitLocation>().transform.Find("Entrypoints/Bottom/Door").gameObject;
        doorBottom.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && !doorsClosed)
        {
            LevelExitdirection direction = GetComponentInParent<ExitLocation>().levelExitDirection;

            switch (direction)
            {
                case LevelExitdirection.left:
                    doorTop.SetActive(true);
                    doorRight.SetActive(true);
                    doorBottom.SetActive(true);
                    print("Left");
                    break;

                case LevelExitdirection.top:
                    doorLeft.SetActive(true);
                    doorRight.SetActive(true);
                    doorBottom.SetActive(true);
                    print("Top");
                    break;

                case LevelExitdirection.right:
                    doorLeft.SetActive(true);
                    doorTop.SetActive(true);
                    doorBottom.SetActive(true);
                    print("Right");
                    break;

                case LevelExitdirection.bottom:
                    doorLeft.SetActive(true);
                    doorTop.SetActive(true);
                    doorRight.SetActive(true);
                    print("Bottom");
                    break;

                default:
                    break;
            }

            doorsClosed = true;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelControllerMk2>().RandomizeLevel();
        }
    }
}
