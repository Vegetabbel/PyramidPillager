﻿using UnityEngine;
using System.Collections;
using System.IO;

public class EquipInfo : MonoBehaviour {

    private int equipIndex = 5;
    private TextMesh text;

	void Start ()
    {
        text = GetComponent<TextMesh>();
	}
	
	void Update ()
    {
        if (!File.Exists("EquippedSave.txt"))
        {
            equipIndex = 5;
        }
        else
        {
            equipIndex = int.Parse(File.ReadAllText("EquippedSave.txt"));
        }

        switch (equipIndex)
        {
            case 0:
                text.text = "Equipped: rinkula \n\nEffect: Extra life \n\nDescription: \nGrants an extra life \nasd \nasd \nasd \nasd \nasd";
                break;
            case 1:
                text.text = "Equipped: sauveli \n\nEffect: Jump higher \n\nDescription: \nISIS jumps higher \nasd \nasd \nasd \nasd \nasd";
                break;
            case 2:
                text.text = "Equipped: räpsytin \n\nEffect: Sprint faster \n\nDescription: \nISIS sprints faster \nasd \nasd \nasd \nasd \nasd";
                break;
            case 3:
			text.text = "Equipped: käärylä \n\nEffect: Longer form \n\nDescription: \nYour form gauge \ndepletes slower \nasd \nasd \nasd \nasd";
                break;
            case 4:
			text.text = "Equipped: öttiäinen \n\nEffect: Slow down time \n\nDescription: \nHold down Left CTRL \nto slow down time \nasd \nasd \nasd \nasd";
                break;
            case 5:
                text.text = "Equipped: none \n\nEffect: \nYou've got nothing!";
                break;
            default:
                text.text = "Equipped: none";
                break;
        }
    }
}
