using UnityEngine;
using System.Collections;

public class EquipTreasure : MonoBehaviour {

	public GameObject Contrl;
	string name;
	int num;

	// Use this for initialization
	void Start () {
		num = int.Parse (this.name);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown () {
		Contrl.SendMessage ("EquipThis", num);
	}
}
