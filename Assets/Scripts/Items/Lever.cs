using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {


    [SerializeField]
    private Interactable target = null;


    public bool Activate()
    {
        if (target.Interact())
        {
            Debug.Log("Activated target");
            return true;
        }
        else
        {
            Debug.Log("error activating target");
            return false;
        }

    }

	// Use this for initialization
	void Start () {
		if(target == null)
        {
            Debug.Log("Error, Lever without target");
        }
	}
	
	// Update is called once per frame
	void Update () {

	}
}
