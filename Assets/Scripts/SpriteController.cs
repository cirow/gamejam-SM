using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour {

    private Player player;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    private bool wasGrounded = true;
    private bool wasfalling = false;

	void Start () {
        player = FindObjectOfType<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
	}
	
	void Update () {
        spriteRenderer.flipX = !player.facingRight;
        anim.SetBool("moving", player.moving);

        if (player.grounded)
        {
            anim.SetBool("grounded", true);
            wasfalling = false;
        }
        else
        {
            anim.SetBool("grounded", false);

            if(player.Velocity.y <= 0 && !player.grounded && !wasfalling)
            {
                anim.SetTrigger("down");
                wasfalling = true;
            }
            else
            {
                if (!player.grounded && wasGrounded)
                {
                    if (player.Velocity.y > 0)
                    {
                        //Debug.Log("pulou");
                        anim.SetTrigger("up");
						if(AudioManager.instance !=null)
						{
							AudioManager.instance.PlayJump();
						}
                    }
                }
            }


        }

        

        wasGrounded = player.grounded;


	}
}
