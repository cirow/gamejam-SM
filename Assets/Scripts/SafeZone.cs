using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
[RequireComponent (typeof (Rigidbody2D))]
public class SafeZone : MonoBehaviour {

    private void Awake () {
        GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        DisabledPlayer disabledPlayer = collision.GetComponent<DisabledPlayer> ();

        print (collision.tag + " | é null? " + (disabledPlayer == null));

        if (disabledPlayer != null) {
            disabledPlayer.Destroy ();
        }
    }

}
