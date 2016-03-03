using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Random = UnityEngine.Random;

public class TreasureEquipScript : MonoBehaviour {

	private GameObject[] treasures = new GameObject[5];
	private GameObject[] highlight = new GameObject[5];

	private const string fileName = "TreasureSave.txt";
	private const string equippedFileName = "EquippedSave.txt";
	private GameObject gameMaster;
	string[] treasureArray = new string[5];
	string equipped = "5";

	GameObject temp;
	int tempInt;

	// Use this for initialization
	void Start () {
		treasures = GameObject.FindGameObjectsWithTag ("treasure");
	
		tempInt = 0;
		for (int i = 0; i < treasures.Length; i++) {
			tempInt = treasures [i].gameObject.transform.GetSiblingIndex ();
			temp = treasures [tempInt];
			treasures [tempInt] = treasures [i];
			treasures [i] = temp;
		}
			
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

		if (File.Exists(fileName)) {
			equipped = File.ReadAllText (equippedFileName);
			File.Delete(equippedFileName);
			File.WriteAllText (equippedFileName, equipped);
		}

		for (int i = 0; i < treasures.Length; i++) {
			highlight[i] = treasures[i].gameObject.transform.GetChild (2).gameObject;
			highlight [i].SetActive (false);
		}

		for (int i = 0; i < treasureArray.Length; i++) {
			if (treasureArray[i] == "1") {
				temp = treasures [i].gameObject.transform.GetChild (1).gameObject;
				temp.SetActive (false);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetKey (KeyCode.K)) {
			treasureArray [0] = "0";
			treasureArray [1] = "0";
			treasureArray [2] = "0";
			treasureArray [3] = "0";
			treasureArray [4] = "0";
			File.WriteAllLines(fileName, treasureArray);
		}
		if (Input.GetKey (KeyCode.L)) {
			treasureArray [0] = "1";
			treasureArray [1] = "1";
			treasureArray [2] = "0";
			treasureArray [3] = "1";
			treasureArray [4] = "0";
			File.WriteAllLines(fileName, treasureArray);
		}*/
	}

	void EquipThis (int num) {
        print(num);
		if (num == 5) {
			//UNEQUIP
			for (int i = 0; i < highlight.Length; i++) {
				highlight [i].SetActive (false);
			}
			equipped = num.ToString ();
			File.Delete(equippedFileName);
			File.WriteAllText (equippedFileName, equipped);
		} else if (treasureArray[num] == "1") {
			for (int i = 0; i < highlight.Length; i++) {
				highlight [i].SetActive (false);
			}
			highlight [num].SetActive (true);
			equipped = num.ToString ();
			File.Delete(equippedFileName);
			File.WriteAllText (equippedFileName, equipped);
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
