using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelPrefabs
{
    public GameObject[] levelPrefabsRightEntry;
    public GameObject[] levelPrefabsTopEntry;
    public GameObject[] levelPrefabsLeftEntry;
    public GameObject[] levelPrefabsBottomEntry;
}

public enum LevelExitdirection { left, top, right, bottom }

public class LevelControllerMk2 : MonoBehaviour
{
    /// <summary>
    /// pasdasdasdasdasd
    /// lol :D
    /// </summary>
    public GameObject playerPrefab;
    public LevelPrefabs levelPrefabs;
    public GameObject previousLevel;

    private GameObject player;
    private bool firstLevelReady = false;
    private GameObject currentLevel;
    private GameObject newLevel;
    private Vector3 currentExitpoint;
    private Vector3 newStartpoint;
    
    private LevelExitdirection levelExitDirection;

    void Start()
    {
        RandomizeLevel();
        CreatePlayer();
    }

    public void RandomizeLevel()
    {
        if (!firstLevelReady)
        {
            currentLevel =  (GameObject)Instantiate(levelPrefabs.levelPrefabsLeftEntry
                            [Random.Range(0, levelPrefabs.levelPrefabsLeftEntry.Length)],
                            Vector3.zero, Quaternion.identity);
            levelExitDirection = currentLevel.GetComponent<ExitLocation>().levelExitDirection;
            firstLevelReady = true;
        }
        else
        {
            if (previousLevel)
            {
                Destroy(previousLevel);
            }

            switch (levelExitDirection)
            {
                case LevelExitdirection.left:
                    currentExitpoint = currentLevel.transform.Find("Entrypoints/Left").localPosition;
                    newLevel =  (GameObject)Instantiate(levelPrefabs.levelPrefabsRightEntry
                                [Random.Range(0, levelPrefabs.levelPrefabsRightEntry.Length)],
                                Vector3.zero, Quaternion.identity);
                    newLevel.GetComponent<ExitLocation>().levelEntryDirection = LevelExitdirection.right;
                    newStartpoint = newLevel.transform.Find("Entrypoints/Right").localPosition;
                    break;

                case LevelExitdirection.top:
                    currentExitpoint = currentLevel.transform.Find("Entrypoints/Top").localPosition;
                    newLevel =  (GameObject)Instantiate(levelPrefabs.levelPrefabsBottomEntry
                                [Random.Range(0, levelPrefabs.levelPrefabsBottomEntry.Length)],
                                Vector3.zero, Quaternion.identity);
                    newLevel.GetComponent<ExitLocation>().levelEntryDirection = LevelExitdirection.bottom;
                    newStartpoint = newLevel.transform.Find("Entrypoints/Bottom").localPosition;
                    break;

                case LevelExitdirection.right:
                    currentExitpoint = currentLevel.transform.Find("Entrypoints/Right").localPosition;
                    newLevel =  (GameObject)Instantiate(levelPrefabs.levelPrefabsLeftEntry
                                [Random.Range(0, levelPrefabs.levelPrefabsLeftEntry.Length)],
                                Vector3.zero, Quaternion.identity);
                    newLevel.GetComponent<ExitLocation>().levelEntryDirection = LevelExitdirection.left;
                    newStartpoint = newLevel.transform.Find("Entrypoints/Left").localPosition;
                    break;

                case LevelExitdirection.bottom:
                    currentExitpoint = currentLevel.transform.Find("Entrypoints/Bottom").localPosition;
                    newLevel =  (GameObject)Instantiate(levelPrefabs.levelPrefabsTopEntry
                                [Random.Range(0, levelPrefabs.levelPrefabsTopEntry.Length)],
                                Vector3.zero, Quaternion.identity);
                    newLevel.GetComponent<ExitLocation>().levelEntryDirection = LevelExitdirection.top;
                    newStartpoint = newLevel.transform.Find("Entrypoints/Top").localPosition;
                    break;

                default:
                    break;
            }
            newLevel.transform.position = currentLevel.transform.position + currentExitpoint - newStartpoint;
            levelExitDirection = newLevel.GetComponent<ExitLocation>().levelExitDirection;
            previousLevel = currentLevel;
            currentLevel = newLevel;
        }
    }

    void CreatePlayer()
    {
        player = (GameObject)Instantiate(playerPrefab, currentLevel.transform.Find("Entrypoints/Left").position, Quaternion.identity);
        Camera.main.GetComponent<CameraFollow>().target = player;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            RandomizeLevel();
        }
    }
}
