using UnityEngine;
using System.Collections;

public class EquipTreasure : MonoBehaviour {

	public GameObject Contrl;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown () {
		Contrl.SendMessage (int.Parse(this.name.ToString()));
	}
}
