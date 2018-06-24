using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : Interactable
{

    [SerializeField]
    private NextLevelPass nextLevel;

	public Keycard[] keycards;
	public override bool Interact()
	{
		throw new System.NotImplementedException();
	}

	// Use this for initialization
	void Start ()
	{
        LockNextLevel(true);
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
        GetComponent<Animator>().enabled = true;
        LockNextLevel(false);
       
		//gameObject.SetActive(false);
	}

    void LockNextLevel(bool lockIt)
    {
        nextLevel.GetComponent<Collider2D>().enabled = !lockIt;

    }
    // Update is called once per frame
    void Update ()
	{
		
	}
}
