using UnityEngine;
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
                text.text = "Equipped: rinkula \n\nEffect: Grants one extra life \n\nDescription: asd \nasd \nasd \nasd \nasd \nasd \nasd";
                break;
            case 1:
                text.text = "Equipped: sauveli \n\nEffect: Isis can jump higher \n\nDescription: asd \nasd \nasd \nasd \nasd \nasd \nasd";
                break;
            case 2:
                text.text = "Equipped: räpsytin \n\nEffect: Isis can run and sprint faster \n\nDescription: asd \nasd \nasd \nasd \nasd \nasd \nasd";
                break;
            case 3:
                text.text = "Equipped: käärylä \n\nEffect: Form gauge depletes slower \n\nDescription: asd \nasd \nasd \nasd \nasd \nasd \nasd";
                break;
            case 4:
                text.text = "Equipped: öttiäinen \n\nEffect: Hold down Left CTRL to slow down time \n\nDescription: asd \nasd \nasd \nasd \nasd \nasd \nasd";
                break;
            case 5:
                text.text = "Equipped: none";
                break;
            default:
                text.text = "Equipped: none";
                break;
        }
    }
}
