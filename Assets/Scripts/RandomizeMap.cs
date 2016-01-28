using UnityEngine;
using System.Collections;

public class RandomizeMap : MonoBehaviour
{
	private bool firstLevelReady = false;
	/// <summary>
	/// MapRandomizing
	/// </summary>
	public GameObject doorBlock;
	public GameObject startRoom;
	public GameObject playerPrefab;
	private GameObject player;
	private GameObject currentLevel;
	private GameObject newLevel;

	private GameObject startLevel;
	private GameObject levelA;
	private GameObject levelB;
	private GameObject middleLevel;
	private GameObject endLevel;

	public GameObject[] prefabss;

	private LevelExitdirection levelExitDirection;

	private Vector3 respawnPoint;
	int u = 0;

	void Start()
	{
		prefabss = Resources.LoadAll<GameObject> ("Maps");
		print (prefabss.Length);
		ArrayRandom (prefabss);

		RandomizeLevel();
		CreatePlayer();
	}
	
	public void RandomizeLevel()
	{
		if (!firstLevelReady)
		{
			startLevel = (GameObject)Instantiate(startRoom, Vector3.zero, Quaternion.identity);
			startLevel.transform.Rotate(new Vector3(90,90,0));
			GameObject childStart = startLevel.gameObject.transform.FindChild("BindPointExit").gameObject;
			respawnPoint = startLevel.transform.Find("SpawnPoint").position;

			levelA = (GameObject)Instantiate(prefabss[u], Vector3.zero, Quaternion.identity);
			levelA.transform.Rotate(new Vector3(90,90,0));
			u++;
			GameObject childA = levelA.gameObject.transform.FindChild("BindPointEntry").gameObject;



			Vector3 offset = childStart.transform.position - childA.transform.position;
			levelA.transform.position += offset;



			levelB = (GameObject)Instantiate(prefabss[u], Vector3.zero, Quaternion.identity);
			levelB.transform.Rotate(new Vector3(90,90,0));
			u++;
			GameObject childB = levelB.gameObject.transform.FindChild("BindPointEntry").gameObject;


			//currentLevel = (GameObject)Instantiate(prefabss[0], Vector3.zero, Quaternion.identity);
			//currentLevel.transform.Rotate(new Vector3(90,90,0));

			//newLevel = (GameObject)Instantiate(prefabss[1], Vector3.zero, Quaternion.identity);
			//newLevel.transform.Rotate(new Vector3(90,90,0));

			//GameObject childA = currentLevel.gameObject.transform.FindChild("BindPointExit").gameObject;
			//GameObject childB = newLevel.gameObject.transform.FindChild("BindPointEntry").gameObject;
			//Vector3 offset = childA.transform.position - childB.transform.position;
			//newLevel.transform.position += offset;

			//GameObject block = (GameObject)Instantiate(doorBlock, currentLevel.gameObject.transform.FindChild("BindPointEntry").position, Quaternion.identity);
			//block.transform.Rotate(new Vector3(90,90,0));

			firstLevelReady = true;
		}
		else
		{



			/*
			if (previousLevel)
			{
				Destroy(previousLevel);
			}*/
			
			/*switch (levelExitDirection)
			{
			case LevelExitdirection.left:
				currentExitpoint = currentLevel.transform.Find("Entrypoints/Left").localPosition;
				newLevel = (GameObject)Instantiate(levelPrefabs.levelPrefabsRightEntry[Random.Range(0, levelPrefabs.levelPrefabsRightEntry.Length)], Vector3.zero, Quaternion.identity);
				newLevel.GetComponent<ExitLocation>().levelEntryDirection = LevelExitdirection.right;
				newStartpoint = newLevel.transform.Find("Entrypoints/Right").localPosition;
				break;
				
			case LevelExitdirection.top:
				currentExitpoint = currentLevel.transform.Find("Entrypoints/Top").localPosition;
				newLevel = (GameObject)Instantiate(levelPrefabs.levelPrefabsBottomEntry[Random.Range(0, levelPrefabs.levelPrefabsBottomEntry.Length)], Vector3.zero, Quaternion.identity);
				newLevel.GetComponent<ExitLocation>().levelEntryDirection = LevelExitdirection.bottom;
				newStartpoint = newLevel.transform.Find("Entrypoints/Bottom").localPosition;
				break;
				
			case LevelExitdirection.right:
				currentExitpoint = currentLevel.transform.Find("Entrypoints/Right").localPosition;
				newLevel = (GameObject)Instantiate(levelPrefabs.levelPrefabsLeftEntry[Random.Range(0, levelPrefabs.levelPrefabsLeftEntry.Length)], Vector3.zero, Quaternion.identity);
				newLevel.GetComponent<ExitLocation>().levelEntryDirection = LevelExitdirection.left;
				newStartpoint = newLevel.transform.Find("Entrypoints/Left").localPosition;
				break;
				
			case LevelExitdirection.bottom:
				currentExitpoint = currentLevel.transform.Find("Entrypoints/Bottom").localPosition;
				newLevel = (GameObject)Instantiate(levelPrefabs.levelPrefabsTopEntry[Random.Range(0, levelPrefabs.levelPrefabsTopEntry.Length)], Vector3.zero, Quaternion.identity);
				newLevel.GetComponent<ExitLocation>().levelEntryDirection = LevelExitdirection.top;
				newStartpoint = newLevel.transform.Find("Entrypoints/Top").localPosition;
				break;
				
			default:
				break;
			}*/
			//newLevel.transform.position = currentLevel.transform.position + currentExitpoint - newStartpoint;
			//levelExitDirection = newLevel.GetComponent<ExitLocation>().levelExitDirection;
			//previousLevel = currentLevel;
			//currentLevel = newLevel;
		}
		/*nextEndpointWorldSpace = currentLevel.transform.Find("Endpoint").position;
        newStartpointWorldSpace = currentLevel.transform.Find("Startpoint").position;
        blocker = currentLevel.transform.Find("Model/Door").gameObject;
        blocker.SetActive(false);

        if (currentLevel == treasureRoomPlace)
        {
            levelStyle = NextLevelStyle.TreasureRoom;
        }
        else if (currentLevel == treasureRoomPlace + 1)
        {
            levelStyle = NextLevelStyle.Vertical;
        }
        currentLevel++;*/
	}

	void ArrayRandom(GameObject[] array) {
		for (int i = 0; i < array.Length; i++ )
		{
			GameObject tmp = array[i];
			int r = Random.Range(i, array.Length);
			array[i] = array[r];
			array[r] = tmp;
		}
		prefabss = array;
		print ("Array Randomized");
	}

	void CreatePlayer()
	{
		player = (GameObject)Instantiate(playerPrefab, respawnPoint, Quaternion.identity);
		Camera.main.GetComponent<CameraFollow>().target = player;
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			RandomizeLevel();
		}

		if (player.transform.position.y < -30) {
			player.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
			player.transform.position = respawnPoint;
		}
	}
}
