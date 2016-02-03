using UnityEngine;
using System.Collections;

public class RandomizeMap : MonoBehaviour
{
	/// <summary>
	/// MapRandomizing
	/// </summary>
	public GameObject doorBlock;
	public GameObject startRoom;
	public GameObject playerPrefab;
	private GameObject player;

	private GameObject startLevel;
	private GameObject middleLevel;
	private GameObject endLevel;
	private GameObject[] horizontalLevels = new GameObject[15];

	public GameObject[] horizontalPrefabs;
	GameObject childA;
	GameObject childB;

	private Vector3 respawnPoint;

	void Start()
	{
		horizontalPrefabs = Resources.LoadAll<GameObject> ("Maps");
		print (horizontalPrefabs.Length);

		horizontalPrefabs = ArrayRandom (horizontalPrefabs);
		RandomizeLevel();
		CreatePlayer();
	}
	
	public void RandomizeLevel()
	{
		startLevel = (GameObject)Instantiate(startRoom, Vector3.zero, Quaternion.identity);
		startLevel.transform.Rotate(new Vector3(90,90,0));
		respawnPoint = startLevel.transform.Find("SpawnPoint").position;

		childA = startLevel.gameObject.transform.FindChild("BindPointExit").gameObject;

		for (int i = 0; i < 5; i++) {
			SpawnRoom (i);
		}

		//For Middle room!!!!
		/*
		middleLevel = (GameObject)Instantiate(startRoom, Vector3.zero, Quaternion.identity);
		middleLevel.transform.Rotate(new Vector3(90,90,0));
		respawnPoint = middleLevel.transform.Find ("SpawnPoint").position;

		for (int i = 5; i < 10; i++) {
			SpawnRoom (i);
		}*/
	}

	void SpawnRoom (int num) {
		if (num != 0) {
			childA = horizontalLevels[num - 1].gameObject.transform.FindChild("BindPointExit").gameObject;
		}

		horizontalLevels[num] = (GameObject)Instantiate(horizontalPrefabs[num], Vector3.zero, Quaternion.identity);
		horizontalLevels[num].transform.Rotate(new Vector3(90,90,0));

		childB = horizontalLevels[num].gameObject.transform.FindChild("BindPointEntry").gameObject;
		horizontalLevels[num].name = "Map" + (num + 1);

		Vector3 offset = childA.transform.position - childB.transform.position;
		horizontalLevels[num].transform.position += offset;
	}

	void EnterRoom (string roomName) {
		int roomNum;
		roomNum = int.Parse(roomName.Substring (roomName.Length - 1, 1));

		GameObject block = (GameObject)Instantiate(doorBlock, horizontalLevels[roomNum - 1].gameObject.transform.FindChild("BindPointEntry").position, Quaternion.identity);
		block.transform.Rotate(new Vector3(90,90,0));
		block.transform.SetParent (horizontalLevels [roomNum - 1].transform);

		respawnPoint = horizontalLevels[roomNum - 1].transform.Find("SpawnPoint").position;

		if (roomNum == 1) {
			startLevel.SetActive (false);
		}
		else {
			horizontalLevels[roomNum - 2].SetActive (false);
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
		Camera.main.GetComponent<CameraFollow>().target = player;
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			RandomizeLevel();
		}

		if (player.transform.position.y < -35) {
			player.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
			player.transform.position = respawnPoint;
		}
	}

	void Die() {
		player.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
		player.transform.position = respawnPoint;
	}
}
