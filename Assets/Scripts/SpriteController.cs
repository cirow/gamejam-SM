using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour {

    private Player player;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        spriteRenderer.flipX = !player.facingRight;
        anim.SetBool("moving", player.moving);
	}
}
