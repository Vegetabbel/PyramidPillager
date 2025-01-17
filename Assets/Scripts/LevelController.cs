﻿using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour
{  
    public GameObject[] levelPrefabsHorizontal;
    public GameObject[] levelPrefabsVertical;
    public GameObject levelPrefabTreasureRoom;
    public int treasureRoomPlace = 5;
    public KeyCode newLevelKey;
    public GameObject playerPrefab;

    private bool firstLevelReady = false;
    private GameObject lastInitializedLevel;
    private GameObject newLevel;
    private Vector3 lastEndpoint;
    private Vector3 newStartpoint;
    private Vector3 nextEndpointWorldSpace;
    private Vector3 newStartpointWorldSpace;
    private int currentLevel = 0;
    private GameObject player;
    private bool blockerReady = false;
    private float distance = 10000;
    private GameObject blocker;

    public enum NextLevelStyle { Horizontal, Vertical, TreasureRoom }
    private NextLevelStyle levelStyle = NextLevelStyle.Horizontal;

    void Start()
    {
        RandomizeLevel();
        CreatePlayer();
    }

    void RandomizeLevel()
    {
        if (!firstLevelReady)
        {
            lastInitializedLevel = (GameObject)Instantiate(levelPrefabsHorizontal[Random.Range(0, levelPrefabsHorizontal.Length)], Vector3.zero, Quaternion.identity);
            firstLevelReady = true;
        }
        else
        {
            switch (levelStyle)
            {
                case NextLevelStyle.Horizontal:
                    lastEndpoint = lastInitializedLevel.transform.Find("Endpoint").localPosition;
                    newLevel = (GameObject)Instantiate(levelPrefabsHorizontal[Random.Range(0, levelPrefabsHorizontal.Length)], Vector3.zero, Quaternion.identity);
                    newStartpoint = newLevel.transform.Find("Startpoint").localPosition;
                    break;

                case NextLevelStyle.Vertical:
                    lastEndpoint = lastInitializedLevel.transform.Find("Endpoint").localPosition;
                    newLevel = (GameObject)Instantiate(levelPrefabsVertical[Random.Range(0, levelPrefabsVertical.Length)], Vector3.zero, Quaternion.identity);
                    newStartpoint = newLevel.transform.Find("Startpoint").localPosition;
                    break;

                case NextLevelStyle.TreasureRoom:
                    lastEndpoint = lastInitializedLevel.transform.Find("Endpoint").localPosition;
                    newLevel = (GameObject)Instantiate(levelPrefabTreasureRoom, Vector3.zero, Quaternion.identity);
                    newStartpoint = newLevel.transform.Find("Startpoint").localPosition;
                    break;

                default:
                    break;
            }
            newLevel.transform.position = lastInitializedLevel.transform.position + lastEndpoint - newStartpoint;
            Destroy(lastInitializedLevel, 1f);
            lastInitializedLevel = newLevel;
        }
        nextEndpointWorldSpace = lastInitializedLevel.transform.Find("Endpoint").position;
        newStartpointWorldSpace = lastInitializedLevel.transform.Find("Startpoint").position;
        blocker = lastInitializedLevel.transform.Find("Model/Door").gameObject;
        blocker.SetActive(false);

        if (currentLevel == treasureRoomPlace)
        {
            levelStyle = NextLevelStyle.TreasureRoom;
        }
        else if (currentLevel == treasureRoomPlace + 1)
        {
            levelStyle = NextLevelStyle.Vertical;
        }
        currentLevel++;
    }

    void CreatePlayer()
    {
        player = (GameObject)Instantiate(playerPrefab, lastInitializedLevel.transform.Find("Startpoint").position, Quaternion.identity);
        Camera.main.GetComponent<CameraFollow>().target = player;
    }

    void Update()
    {
        if (Input.GetKeyDown(newLevelKey))
        {
            RandomizeLevel();
        }
        if (player)
        {
            if (Vector3.Distance(player.transform.position, nextEndpointWorldSpace) < 1f)
            {
                RandomizeLevel();
                blockerReady = true;
            }
        } 
        if (blockerReady)
        {
            if (Vector3.Distance(player.transform.position, newStartpointWorldSpace) > 1f)
            {
                blocker.SetActive(true);
                blockerReady = false;
            }
        }
    }
}
