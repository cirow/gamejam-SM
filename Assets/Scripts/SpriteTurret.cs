using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTurret : MonoBehaviour {

    private Turret turretParent;
	public float animSpeed = 2;
	// Use this for initialization
	void Start () {
        turretParent = GetComponentInParent<Turret>();
		GetComponent<Animator>().speed = animSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Shoot()
    {
        turretParent.Shoot();
    }
}
