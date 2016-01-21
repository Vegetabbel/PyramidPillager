using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour
{  
    public GameObject[] levelPrefabsHorizontal;
    public GameObject[] levelPrefabsVertical;
    public GameObject levelPrefabTreasureRoom;
    public int treasureRoomPlace = 5;

    private bool firstLevelReady = false;
    private GameObject lastInitializedLevel;
    private Vector3 lastEndpoint;
    private Vector3 newStartpoint;
    private GameObject newLevel;
    private int currentLevel = 0;

    public enum NextLevelStyle { Horizontal, Vertical, TreasureRoom }
    private NextLevelStyle levelStyle = NextLevelStyle.Horizontal;

    void Start()
    {
        RandomizeLevel();
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
            lastInitializedLevel = newLevel;   
        }
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


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RandomizeLevel();
        }
    }
}
