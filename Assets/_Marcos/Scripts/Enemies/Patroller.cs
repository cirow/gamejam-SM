using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class Patroller : Enemy {

    public Transform patrolLeftLimit;
    public Transform patrolRightLimit;

    [Space (10)]
    public int speed = 10;
    public float waitTimeWhenDirectionChange;
    public bool startingDirectionRight;

    private float accelerationTimeGrounded = 0.1f;
    private float accelerationTimeAirborne = 0.2f;

    private Controller2D controller;

    private float gravity = -20;

    private float velocityXSmoothing;

    private float horizontalMovement;
    private Vector3 velocity;

    private float patrolLeftLimitX, patrolRightLimitX;

    private bool goingRight;

    private bool patrolPaused;

    void Awake () {
        controller = GetComponent<Controller2D> ();

        goingRight = startingDirectionRight;

        patrolLeftLimitX = patrolLeftLimit.position.x;
        patrolRightLimitX = patrolRightLimit.position.x;
    }
	
	void Update () {
        Patrol ();
        LaguesUpdate ();
    }

    private void OnDrawGizmos () {
        if (!Application.isPlaying) {
            Gizmos.color = Color.red;

            float size = 0.5f;

            if (patrolLeftLimit != null) {
                Vector3 position = patrolLeftLimit.position;
                Gizmos.DrawLine (position - Vector3.up * size, position + Vector3.up * size);
                Gizmos.DrawLine (position - Vector3.right * size, position + Vector3.right * size);
            }

            if (patrolRightLimit != null) {
                Vector3 position = patrolRightLimit.position;
                Gizmos.DrawLine (position - Vector3.up * size, position + Vector3.up * size);
                Gizmos.DrawLine (position - Vector3.right * size, position + Vector3.right * size);
            }
        } else {
            Gizmos.color = Color.red;

            float size = 0.5f;

            if (patrolLeftLimit != null) {
                Vector3 position = new Vector3 (patrolLeftLimitX, transform.position.y, 0);
                Gizmos.DrawLine (position - Vector3.up * size, position + Vector3.up * size);
                Gizmos.DrawLine (position - Vector3.right * size, position + Vector3.right * size);
            }

            if (patrolRightLimit != null) {
                Vector3 position = new Vector3 (patrolRightLimitX, transform.position.y, 0);
                Gizmos.DrawLine (position - Vector3.up * size, position + Vector3.up * size);
                Gizmos.DrawLine (position - Vector3.right * size, position + Vector3.right * size);
            }
        }
    }

    private void Patrol () {
        if (goingRight) {
            if (controller.collisions.right || transform.position.x >= patrolRightLimitX) {
                ChangeDirection ();
            }
        } else if (!goingRight) {
            if (controller.collisions.left || transform.position.x <= patrolLeftLimitX) {
                ChangeDirection ();
            }
        }

        if (patrolPaused) {
            horizontalMovement = 0;
        } else {
            horizontalMovement = goingRight ? speed : -speed;
        }
    }

    private void ChangeDirection () {
        goingRight = !goingRight;
        StartCoroutine (PausePatrol ());
        print ("changed direction!");
    }

    private IEnumerator PausePatrol () {
        patrolPaused = true;
        yield return new WaitForSeconds (waitTimeWhenDirectionChange);
        patrolPaused = false;
    }

    private void LaguesUpdate () {
        velocity.x = Mathf.SmoothDamp (velocity.x, horizontalMovement, ref velocityXSmoothing, controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne);

        velocity.y += gravity * Time.deltaTime;
        controller.Move (velocity * Time.deltaTime, Vector3.zero);

        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
    }

}
