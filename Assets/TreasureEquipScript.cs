using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Random = UnityEngine.Random;

public class TreasureEquipScript : MonoBehaviour {

	private GameObject[] treasures = new GameObject[5];

	private const string fileName = "TreasureSave.txt";
	private GameObject gameMaster;
	string[] treasureArray = new string[5];

	// Use this for initialization
	void Start () {
		GameObject.FindGameObjectsWithTag ("treasure");

		if (File.Exists(fileName)) {
			treasureArray = File.ReadAllLines (fileName);
			File.Delete(fileName);
		}
		else {
			treasureArray [0] = "0";
			treasureArray [1] = "0";
			treasureArray [2] = "0";
			treasureArray [3] = "0";
			treasureArray [4] = "0";
		}
		File.WriteAllLines(fileName, treasureArray);


	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.K)) {
			treasureArray [0] = "0";
			treasureArray [1] = "0";
			treasureArray [2] = "0";
			treasureArray [3] = "0";
			treasureArray [4] = "0";
			File.WriteAllLines(fileName, treasureArray);
		}
	}

	void EquipThis (int num) {
		if (treasureArray[num] == "1") {
			
		} else {
			
		}
	}

	void OnTriggerEnter (Collider other) {
			if (File.Exists(fileName)) {
				treasureArray = File.ReadAllLines (fileName);
				File.Delete(fileName);
			}
			else {
				treasureArray [0] = "0";
				treasureArray [1] = "0";
				treasureArray [2] = "0";
				treasureArray [3] = "0";
				treasureArray [4] = "0";
			}

			int rnd = Random.Range (0,5);
			Debug.Log (rnd);
			treasureArray [rnd] = "1";
			File.WriteAllLines(fileName, treasureArray);

			string aa = "";
			for (int i = 0; i < treasureArray.Length; i++) {
				aa = aa + i + ":" + treasureArray [i] + "  ";
			}
			Debug.Log (aa);

			Destroy (this);
			/*
			StreamWriter sr;
			if (File.Exists(fileName))
			{
				Debug.Log(fileName + " already exists.");
				//sr = File.OpenRead (fileName);
				File.Delete (fileName);
			}
			else {
				File.WriteAllLines(fileName, int[] aa);
				int[] aa = File.ReadAllLines(fileName);
				
			}

			sr = File.CreateText (fileName);
			int rnd = Random.Range (0,5);

			switch (rnd) {
			case 0:
				sr.WriteLine ("Treasure nro 1");
				break;
			case 1:
				sr.WriteLine ("Treasure nro 2");
				break;
			case 2:
				sr.WriteLine ("Treasure nro 3");
				break;
			case 3:
				sr.WriteLine ("Treasure nro 4");
				break;
			case 4:
				sr.WriteLine ("Treasure nro 5");
				break;
			}
			sr.WriteLine ("Treasures");
			Debug.Log (sr);
			sr.Close();*/
	}
}
