using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour {


    [SerializeField]
    private Interactable[] targets = null;
    [SerializeField]
    private Vector2 radiusBox;

    private bool isActivated = false;



    public bool Activate(bool active)
    {
        isActivated = active;
        bool result = false;

        foreach(Interactable target in targets)
        {
     
            if (target.Interact())
            {
                result = true;
            }
            else
            {
                Debug.Log("error activating target");
                result = false;
            }
        }

        return result;
        

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Physics2D.OverlapBox(transform.position, radiusBox, 0, LayerMask.GetMask("PlayerObstacle")) || Physics2D.OverlapBox(transform.position, radiusBox, 0, LayerMask.GetMask("Player")))
        {
            if (!isActivated)
            {
                Activate(true);
                Debug.Log("activated target");

            }
        }
        else
        {
            if (isActivated)
            {
                Activate(false);
                Debug.Log("deactivated target");
            }
        }
		
	}


}
