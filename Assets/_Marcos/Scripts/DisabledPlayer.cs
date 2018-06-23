using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledPlayer : MonoBehaviour {

    // Sebastian Lague's
    private Controller2D controller;

    private float gravity = -20;

    private Vector3 velocity;

    private void Awake () {
        //collider.enabled = false;
        controller = GetComponent<Controller2D> ();
    }

    void Start () {

    }


    void Update () {
        LaguesUpdate ();
    }

    public void Destroy () {
        Destroy (gameObject);
    }

    private void LaguesUpdate () {
        velocity.y += gravity * Time.deltaTime;
        controller.Move (velocity * Time.deltaTime, Vector2.zero);

        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
    }

}
