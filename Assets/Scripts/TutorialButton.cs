using UnityEngine;
using System.Collections;

public class TutorialButton : MonoBehaviour
{
    int LoadedLevel = 0;

    void OnMouseDown()
    {
        LoadedLevel = Application.loadedLevel;

        if (LoadedLevel == 1)
        {
            Application.LoadLevel(7);
            LoadedLevel = Application.loadedLevel;
        }

    }
}