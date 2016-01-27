using UnityEngine;
using System.Collections;

public class HoverHighlightScript : MonoBehaviour {

	public Sprite sprite1;
	public Sprite sprite2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter () {
		GetComponent <SpriteRenderer>().sprite = sprite1;
	}

	void OnMouseExit() {
		GetComponent <SpriteRenderer>().sprite = sprite2;
	}
}
