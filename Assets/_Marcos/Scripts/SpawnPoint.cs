﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos () {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere (transform.position, 0.25f);
    }

}
