using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keycard : Interactable
{
	[SerializeField]
	private Vector2 radiusBox;
	public bool isActivated = false;
	private ExitDoor exitDoor;
	public delegate void KeycardGetEvent();
	public KeycardGetEvent OnKeycardGet;

	public bool Activate(bool active, Collider2D target)
	{
		isActivated = active;
		bool result = false;

		Interact();


		result = true;		
		return result;
	}
	public override bool Interact()
	{
		if(isActivated)
		{
			if(OnKeycardGet != null)
			{
				OnKeycardGet();
			}		
			gameObject.SetActive(false);
		}
		else
		{
			gameObject.SetActive(true);
		}		
		return true;
	}



	// Use this for initialization
	void Start ()
	{
		//exitDoor = GameObject.FindObjectOfType<ExitDoor>();
		//exitDoor.keycards.Add(gameObject.GetComponent<Keycard>());		
	}
	
	// Update is called once per frame
	void Update ()
	{
		Collider2D mCollider = Physics2D.OverlapBox(transform.position, radiusBox, 0, LayerMask.GetMask("Player"));
		if (mCollider != null && mCollider.tag == "Player")
		{
			if (!isActivated)
			{
				Activate(true, mCollider);
				Debug.Log("activated target");
			}
		}
		
	}
}
