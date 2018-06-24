using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour {


    [SerializeField]
    private Interactable[] targets = null;
    [SerializeField]
    private Vector2 radiusBox;
    [SerializeField]
    private Sprite spriteOff;
    [SerializeField]
    private Sprite spriteOn;

    private SpriteRenderer spriteRenderer;


    private bool isActivated = false;



    public bool Activate(bool active)
    {
        isActivated = active;
        bool result = false;

        foreach(Interactable target in targets)
        {
            if (target != null) {
                if (target.Interact ()) {
                    result = true;
                } else {
                    Debug.Log ("error activating target");
                    result = false;
                }
            } else {
                Debug.Log ("(" + gameObject.name + "): target de PushButton está nulo!");
            }
        }

        return result;
        

    }

    // Use this for initialization
    void Start () {
        radiusBox = new Vector2(radiusBox.x * transform.localScale.x, radiusBox.y * transform.localScale.y);
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
        if (Physics2D.OverlapBox(transform.position, radiusBox, 0, LayerMask.GetMask("PlayerObstacle")) || Physics2D.OverlapBox(transform.position, radiusBox, 0, LayerMask.GetMask("Player")))
        {
            if (!isActivated)
            {
                Activate(true);
                spriteRenderer.sprite = spriteOn;
                Debug.Log("activated target");


            }
        }
        else
        {
            if (isActivated)
            {
                Activate(false);
                spriteRenderer.sprite = spriteOff;
                Debug.Log("deactivated target");
            }
        }
		
	}


    private IEnumerator MoveButton()
	{
		return null;
	}


}
