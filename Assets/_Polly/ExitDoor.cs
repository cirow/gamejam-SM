using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : Interactable
{

	public Keycard[] keycards;
	public override bool Interact()
	{
		throw new System.NotImplementedException();
	}

	// Use this for initialization
	void Start ()
	{
		keycards = GameObject.FindObjectsOfType<Keycard>();
		foreach (var card in keycards)
		{
			card.OnKeycardGet += CheckIfUnlocked;
		}
	}
	
	void CheckIfUnlocked()
	{
		foreach(var card in keycards)
		{
			if(!card.isActivated)
			{
				return;
			}
		}
		UnlockDoor();
	}

	void UnlockDoor()
	{
		gameObject.SetActive(false);
	}
	// Update is called once per frame
	void Update ()
	{
		
	}
}
