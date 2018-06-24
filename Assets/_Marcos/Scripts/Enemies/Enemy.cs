using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {


    private void Awake () {
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.GetComponent<Player> () != null) {
            GameManager manager = FindObjectOfType<GameManager> ();
            if (manager != null) {
                manager.DisableCurrentPlayer (false);
            }
        } else {
            DisabledPlayer disabledPlayer = collision.GetComponent<DisabledPlayer> ();

            if (disabledPlayer != null) {
                disabledPlayer.Destroy ();
            }
        }
    }

}
