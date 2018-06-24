using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    
    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.GetComponent<Player> () != null) {
            //print ("player entrou no inimigo!");

            GameManager manager = FindObjectOfType<GameManager> ();
            if (manager != null) {
                manager.DisableCurrentPlayer (false);
            }
        }
    }

}
