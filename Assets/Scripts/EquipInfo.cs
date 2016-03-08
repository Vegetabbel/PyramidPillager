using UnityEngine;
using System.Collections;
using System.IO;

public class EquipInfo : MonoBehaviour {

    private int equipIndex = 5;
    private TextMesh text;
    private TextMesh name;
    private TextMesh effect;

    void Start ()
    {
        text = GetComponent<TextMesh>();
        name = transform.Find("name").GetComponent<TextMesh>();
        effect = transform.Find("effect").GetComponent<TextMesh>();
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
            /*
            Ankh - Ankh is usually seen in ancient paintings as an object held in the hands of gods or pharaos. It is a symbol of life and so it was often used when a new pharaoh took rein. It was pressed to their lips to bless them with good and long life.
            This item blesses you with one extra health point.
            
            Sceptres - Sceptres are often associated with power in Ancient Egypt. There are many different kinds of sceptres, but the most common kinds are the heqa-sceptre and the flail. They are often seen as a symbol of power held in the hands of a pharao.
            This item blesses you with more jump power.
            
            Book of Toth - Book of Toth was believed to have been written by the god Toth himself. It is said to contain knowledge not meant for humans as well as spells, such as making it possible for human to understand animals. There are stories of the book having been stolen, but those never have an happy ending.
            This item blesses you with the ability to stay in animal and spirit form for longer.
            
            Scarab - Scarab were popular kind of amulet in Ancient Egypt. They were thought to take after Khepri, one of the sun god Ra's reincarnations. It was thought that Khepri rolled the sun across the sky, much like the beetles the amulets took after did to dung.
            This item blesses you with speed.
            
            The Wadjet - In one of the god Horus' battles against the god Set, Horus had his right eye torn out. The god Toth used his spit to put the eye back together, after which it became a symbol of protection. The sun and the moon are often regarded as Horus' eyes.
            This item protects you from the passage of time for a brief moment.
            */
            case 0:
                name.text = "Equipped: Ankh";
                text.text = ResolveTextSize("Ankh is usually seen in ancient paintings as an object held in the hands of gods or pharaos. It is a symbol of life and so it was often used when a new pharaoh took rein. It was pressed to their lips to bless them with good and long life.",25);
                effect.text = ResolveTextSize("This item blesses you with one extra health point.", 25);
                break;
            case 1:
                name.text = "Equipped: Sceptres";
                text.text = ResolveTextSize("Sceptres are often associated with power in Ancient Egypt. There are many different kinds of sceptres, but the most common kinds are the heqa-sceptre and the flail. They are often seen as a symbol of power held in the hands of a pharao.", 25);
                effect.text = ResolveTextSize("This item blesses you with more jump power.", 25);
                break;
            case 2:
                name.text = "Equipped: Scarab";
                text.text = ResolveTextSize("Scarab were popular kind of amulet in Ancient Egypt. They were thought to take after Khepri, one of the sun god Ra's reincarnations. It was thought that Khepri rolled the sun across the sky, much like the beetles the amulets took after did to dung.", 25);
                effect.text = ResolveTextSize("This item blesses you with speed.", 24);
                break;
            case 3:
                name.text = "Equipped: Book of Toth";
                text.text = ResolveTextSize("Book of Toth was believed to have been written by the god Toth himself. It is said to contain knowledge not meant for humans as well as spells, such as making it possible for human to understand animals. There are stories of the book having been stolen, but those never have an happy ending.", 25);
                effect.text = ResolveTextSize("This item blesses you with the ability to stay in animal and spirit form for longer.", 25);
                break;
            case 4:
                name.text = "Equipped: The Wadjet";
                text.text = ResolveTextSize("In one of the god Horus' battles against the god Set, Horus had his right eye torn out. The god Toth used his spit to put the eye back together, after which it became a symbol of protection. The sun and the moon are often regarded as Horus' eyes.", 25);
                effect.text = ResolveTextSize("This item protects you from the passage of time. Hold left CTRL to use", 24);
                break;
            case 5:
                name.text = "Equipped: Nothing";
                text.text = ResolveTextSize("Equip an item to activate bonuses", 25);
                effect.text = ResolveTextSize(" ", 25);
                break;
            default:
                name.text = "Equipped: nothing";
                text.text = "none";
                effect.text = "none";
                break;
        }
    }
    // Wrap text by line height
    private string ResolveTextSize(string input, int lineLength)
    {

        // Split string by char " "         
        string[] words = input.Split(" "[0]);

        // Prepare result
        string result = "";

        // Temp line string
        string line = "";

        // for each all words        
        foreach (string s in words)
        {
            // Append current word into line
            string temp = line + " " + s;

            // If line length is bigger than lineLength
            if (temp.Length > lineLength)
            {

                // Append current line into result
                result += line + "\n";
                // Remain word append into new line
                line = s;
            }
            // Append current word into current line
            else
            {
                line = temp;
            }
        }

        // Append last line into result        
        result += line;

        // Remove first " " char
        return result.Substring(1, result.Length - 1);
    }
}
