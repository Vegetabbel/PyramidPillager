using UnityEngine;
using System.Collections;

public class LevelControllerMk2 : MonoBehaviour
{
    public GameObject playerPrefab;

    private GameObject player;

    void Start()
    {
        RandomizeLevel();
        CreatePlayer();
    }

    void RandomizeLevel()
    {

    }

    void CreatePlayer()
    {
        player = (GameObject)Instantiate(playerPrefab, Vector3.zero/*lastInitializedLevel.transform.Find("Startpoint").position*/, Quaternion.identity);
        Camera.main.GetComponent<CameraFollow>().target = player;
    }
}
