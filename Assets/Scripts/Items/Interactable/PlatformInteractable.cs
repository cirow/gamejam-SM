using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformInteractable : Interactable {

    public override bool Interact()
    {
        isOn = !isOn;

        gameObject.SetActive(!isOn);
        return true;
    }

    // Use this for initialization
    void Start () {
        gameObject.SetActive(!isOn);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
