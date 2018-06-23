using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable {

    public override bool Interact()
    {
        isOn = !isOn;

        gameObject.SetActive(!isOn);
        return true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
