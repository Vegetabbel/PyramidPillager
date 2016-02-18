using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour {
	int LoadedLevel = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		LoadedLevel = Application.loadedLevel;

		Application.LoadLevel ("RandomizerTest");

		if (LoadedLevel == 0) {
			Application.LoadLevel (6);
			LoadedLevel = Application.loadedLevel;
		}/*
		else if (LoadedLevel == 1) {
			Application.LoadLevel (2);
		}*/

	}
}
