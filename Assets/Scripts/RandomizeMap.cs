using UnityEngine;
using System.Collections;

public class RandomizeMap : MonoBehaviour
{
	/// <summary>
	/// MapRandomizing
	/// </summary>
	public GameObject doorBlock;
	public GameObject startRoom;
	public GameObject middleRoom;
	public GameObject endRoom;
	public GameObject playerPrefab;
	private GameObject player;

	private GameObject startLevel;
	private GameObject middleLevel;
	private GameObject endLevel;
	private GameObject[] levels = new GameObject[15];

	private GameObject[] horizontalPrefabs = new GameObject[14];
	private GameObject[] verticalPrefabs = new GameObject[14];
	GameObject childA;
	GameObject childB;

	private Vector3 respawnPoint;

	void Start()
	{
		horizontalPrefabs = Resources.LoadAll<GameObject> ("Horizontal");
		horizontalPrefabs = ArrayRandom (horizontalPrefabs);
		print (horizontalPrefabs.Length);

		verticalPrefabs = Resources.LoadAll<GameObject> ("Vertical");
		verticalPrefabs = ArrayRandom (verticalPrefabs);
		print (verticalPrefabs.Length);

		RandomizeLevel();
		CreatePlayer();
	}
	
	public void RandomizeLevel()
	{
		startLevel = (GameObject)Instantiate(startRoom, Vector3.zero, Quaternion.identity);
		//startLevel.transform.Rotate(new Vector3(90,90,0));
		respawnPoint = startLevel.transform.Find("SpawnPoint").position;

		childA = startLevel.gameObject.transform.FindChild("BindPointExit").gameObject;

		for (int i = 0; i < 5; i++) {
			SpawnRoom (i);
		}
	
		//MiddleLevel
		middleLevel = (GameObject)Instantiate(middleRoom, Vector3.zero, Quaternion.identity);
		childA = middleLevel.gameObject.transform.FindChild("BindPointEntry").gameObject;
		childB = levels[4].gameObject.transform.FindChild("BindPointExit").gameObject;
		Vector3 offset = childB.transform.position - childA.transform.position;
		middleLevel.transform.position += offset;
		childA = middleLevel.gameObject.transform.FindChild ("BindPointExit").gameObject;

		for (int i = 5; i < 10; i++) {
			SpawnRoom (i);
		}

		endLevel = (GameObject)Instantiate (endRoom, Vector3.zero, Quaternion.identity);
		childA = endLevel.gameObject.transform.FindChild ("BindPointEntry").gameObject;
		childB = levels [9].gameObject.transform.FindChild ("BindPointExit").gameObject;
		offset = childB.transform.position - childA.transform.position;
		endLevel.transform.position += offset;
	}

	void SpawnRoom (int num) {
		if (num != 0 && num != 5) {
			childA = levels[num - 1].gameObject.transform.FindChild("BindPointExit").gameObject;
		}

		if (num >= 5) {
			levels[num] = (GameObject)Instantiate(verticalPrefabs[num - 5], Vector3.zero, Quaternion.identity);
		}
		else {
			levels[num] = (GameObject)Instantiate(horizontalPrefabs[num], Vector3.zero, Quaternion.identity);
		}

		//TEMP!!!
//		if (num < 5) {
//			horizontalLevels[num].transform.Rotate(new Vector3(90,90,0));
//		}
//		else {
//			horizontalLevels[num].transform.Rotate(new Vector3(360,90,0));
//		}
		
		childB = levels[num].gameObject.transform.FindChild("BindPointEntry").gameObject;
		levels[num].name = "Map" + (num + 1);

		Vector3 offset = childA.transform.position - childB.transform.position;
		levels[num].transform.position += offset;
	}

	void EnterRoom (string roomName) {
		GameObject block;
		int roomNum = 0;

		if (roomName == "Middle") {
			block = (GameObject)Instantiate(doorBlock, middleLevel.gameObject.transform.FindChild("BindPointEntry").position, Quaternion.identity);
			block.transform.SetParent (middleLevel.transform);
			respawnPoint = middleLevel.transform.Find("SpawnPoint").position;
			levels[4].SetActive(false);
		}
		else if (roomName == "End") {
			block = (GameObject)Instantiate(doorBlock, endLevel.gameObject.transform.FindChild("BindPointEntry").position, Quaternion.identity);
			block.transform.SetParent (endLevel.transform);
			respawnPoint = endLevel.transform.Find("SpawnPoint").position;
			levels[9].SetActive(false);
			block.transform.Rotate (new Vector3 (180, 180, 270));
			Application.LoadLevel("MenuScene");
		}
		else {
			roomNum = int.Parse(roomName.Substring (3, 1 + (roomName.Length - 4)));
			block = (GameObject)Instantiate(doorBlock, levels[roomNum - 1].gameObject.transform.FindChild("BindPointEntry").position, Quaternion.identity);
			block.transform.SetParent (levels [roomNum - 1].transform);
			respawnPoint = levels[roomNum - 1].transform.Find("SpawnPoint").position;

			if (roomNum == 1) {
				startLevel.SetActive (false);
			}
			else if (roomNum == 6) {
				middleLevel.SetActive (false);
			}
			else {
				levels[roomNum - 2].SetActive (false);
			}
		}

		//TEMP
		if (roomNum > 5) {
			block.transform.Rotate (new Vector3 (360, 90, 0));
		} else {
			block.transform.Rotate (new Vector3 (90, 90, 0));
		}
	}

	GameObject[] ArrayRandom(GameObject[] array) {
		for (int i = 0; i < array.Length; i++ )
		{
			GameObject tmp = array[i];
			int r = Random.Range(i, array.Length);
			array[i] = array[r];
			array[r] = tmp;
		}
		return array;
	}

	void CreatePlayer()
	{
		player = (GameObject)Instantiate(playerPrefab, respawnPoint, Quaternion.identity);
		//Camera.main.GetComponent<CameraFollow>().target = player;
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			RandomizeLevel();
		}

		if (player.transform.position.y < -30000) {
			player.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
			player.transform.position = respawnPoint;
		}
	}

	void Die() {
		player.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
		player.transform.position = respawnPoint;
	}
}
