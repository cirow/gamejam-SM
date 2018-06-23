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

        //if (skipFirstPhysicsFrame && !isKinematic && rigid.velocity.y == 0) {
        //    rigid.bodyType = RigidbodyType2D.Kinematic;
        //    isKinematic = true;
        //}

    }

    //private void FixedUpdate () {
    //    skipFirstPhysicsFrame = true;
    //}

    //private void CheckIfGrounded () {
    //    RaycastHit2D hit = Physics2D.BoxCast (transform.position + (Vector3.down * 0.2f), new Vector2 (1, 0.01f), 0, Vector2.down, 0.1f, collisionMask);

    //    if (hit) {
    //        print ("hitando algo");

    //        collider.enabled = true;
    //        rigid.bodyType = RigidbodyType2D.Kinematic;
    //        isKinematic = true;

    //        rigid.velocity = Vector2.zero;
    //    }
    //}


    private void LaguesUpdate () {
        velocity.y += gravity * Time.deltaTime;
        controller.Move (velocity * Time.deltaTime, Vector2.zero);

        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
    }

}
