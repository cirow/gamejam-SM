﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCog : MonoBehaviour {
	public int rotateSpeed = 50;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(new Vector3(0, 0, rotateSpeed) * Time.deltaTime);
	}
}
