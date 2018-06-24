using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTurret : MonoBehaviour {

    private Turret turretParent;

	// Use this for initialization
	void Start () {
        turretParent = GetComponentInParent<Turret>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Shoot()
    {
        turretParent.Shoot();
    }
}
