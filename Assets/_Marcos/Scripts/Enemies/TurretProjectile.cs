using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour {

    public LayerMask collisionMask;

    [Space (10)]
    public float speed = 10;
    [Range (1, 1000)]
    public float projectileLifetime = 8;


    private float limitTime;

    private void Awake () {
        limitTime = Time.time + projectileLifetime;
    }

    void Update () {
        if (Time.time > limitTime) {
            Destroy (gameObject);
        } else {
            float moveDistance = speed * Time.deltaTime;
            CheckCollisions (moveDistance);
            transform.Translate (Vector2.right * moveDistance);
        }
    }

    void CheckCollisions (float moveDistance) {
        Ray2D ray = new Ray2D (transform.position, transform.right);
        RaycastHit2D hit = Physics2D.Raycast (transform.position, transform.right, 0.1f);

        if (hit) {
            Player player = hit.collider.GetComponent<Player> ();
            if (player != null) {
                player.TakeHit ();
            }
            Destroy (gameObject);
        }
    }

}
