using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class DisabledPlayer : MonoBehaviour {
    
    private Controller2D controller;

    private float gravity = -20;

    private Vector3 velocity;

    private void Awake () {
        controller = GetComponent<Controller2D> ();
    }

    void Start () {

    }


    void Update () {
        LaguesUpdate ();
    }

    public void Destroy () {
        GameManager manager = FindObjectOfType<GameManager> ();
        if (manager != null) {
            manager.CreateExplosion (transform.position);
        }
        Destroy (gameObject);
    }

    private void LaguesUpdate () {
        velocity.y += gravity * Time.deltaTime;
        controller.Move (velocity * Time.deltaTime, Vector2.zero);

        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
    }

    public void FlipSprite (bool flip) {
        GetComponentInChildren<SpriteRenderer> ().flipX = flip;
    }

}
