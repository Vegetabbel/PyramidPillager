using UnityEngine;
using System.Collections;

public class MiddleRoomColl : MonoBehaviour {
	
	//Color colorStart;
	//Color colorEnd;
	//float duration = 1.0f;
	
	//GameObject parent;
	
	private GameObject gameMaster;
	// Use this for initialization
	void Start () {
		gameMaster = GameObject.Find ("GameController");
		this.gameObject.name = transform.parent.name;
		
		//parent = GameObject.Find (transform.parent.name);
		//colorStart = renderer.material.color;
		//colorEnd = Color(colorStart.r, colorStart.g, colorStart.b, 0.0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player") {
			if (this.name == "Endroom(Clone)") {
				gameMaster.SendMessage ("EnterRoom", "End");
			}
			else {
				gameMaster.SendMessage ("EnterRoom", "Middle");
			}
		}
	}
	
	/*
	void OnMouseDown () {
		FadeOut();
	}

	void FadeOut ()
	{
		for (t = 0.0; t < duration; t += Time.deltaTime) {
			renderer.material.color = Color.Lerp (colorStart, colorEnd, t/duration);
			yield;
		}
	}*/
}
