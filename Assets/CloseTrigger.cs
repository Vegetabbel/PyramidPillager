using UnityEngine;
using System.Collections;

public class CloseTrigger : MonoBehaviour
{
    void Start()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        LevelExitdirection direction = GetComponent<ExitLocation>().levelEntryDirection;

        switch (direction)
        {
            case LevelExitdirection.left:
                transform.Find("Entrypoints/Left/Door").gameObject.SetActive(true);
                break;

            case LevelExitdirection.top:
                transform.Find("Entrypoints/Top/Door").gameObject.SetActive(true);
                break;

            case LevelExitdirection.right:
                transform.Find("Entrypoints/Right/Door").gameObject.SetActive(true);
                break;

            case LevelExitdirection.bottom:
                transform.Find("Entrypoints/Bottom/Door").gameObject.SetActive(true);
                break;

            default:
                break;
        }
    }
}
