using UnityEngine;
using System.Collections;

public class TutorialSceneCollider : MonoBehaviour {

	GameObject gameMaster;

	// Use this for initialization
	void Start () {
		gameMaster = GameObject.Find ("GameController");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player") {
			Application.LoadLevel("TitleScene");
		}
	}
}
