using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy {

    public TurretProjectile projectile;
    public Transform muzzle;

    [Space (10)]
    public float projectileSpeed = 10;

    [Space (10)]
    public bool shootsInIntervals = false;
    [Range (100, 100000)]
    public float msBetweenShots = 500;

    private float nextShotTime = 0;
    
	void Start () {
		
	}

    void Update () {
        if (shootsInIntervals) {
            if (msBetweenShots >= 100 && Time.time > nextShotTime) {
                nextShotTime = Time.time + msBetweenShots / 1000;
                Shoot ();
            }
        }
    }

    public void Shoot () {
        TurretProjectile newProjectile = Instantiate (projectile, muzzle.position, muzzle.rotation) as TurretProjectile;
        newProjectile.speed = projectileSpeed;
    }

}
