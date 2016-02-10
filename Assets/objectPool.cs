﻿using UnityEngine;
using System.Collections.Generic;

public class objectPool : MonoBehaviour {

    public static List<GameObject> arrowPool = new List<GameObject>();
    public GameObject arrow;
    private int poolSize = 10;

	void Start ()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject instance = Instantiate(arrow);
            arrowPool.Add(instance);
            instance.SetActive(false);
        }
	}
}
