﻿using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GameObject.Find("Player").transform.position = new Vector3(8, 30, 8);
    }

    // Update is called once per frame
    void Update()
    {
		
    }
}
