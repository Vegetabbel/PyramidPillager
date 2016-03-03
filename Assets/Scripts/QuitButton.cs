using UnityEngine;
using System.Collections;

public class QuitButton : MonoBehaviour {
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
		if (LoadedLevel == 0) {
			Application.Quit ();
			LoadedLevel = Application.loadedLevel;
		}
		else if (LoadedLevel == 1) {
            Application.Quit();
            LoadedLevel = Application.loadedLevel;
		}
		else if (LoadedLevel == 2) {
			Application.LoadLevel(1);
			LoadedLevel = Application.loadedLevel;
		}
		else if (LoadedLevel == 3) {
			Application.LoadLevel(1);
			LoadedLevel = Application.loadedLevel;
		}
		else if (LoadedLevel == 4) {
			Application.LoadLevel(1);
			LoadedLevel = Application.loadedLevel;
		}
	}
}
