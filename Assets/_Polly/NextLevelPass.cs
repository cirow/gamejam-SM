using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelPass : MonoBehaviour {

	public LevelManager levelManager;
	public string nextLevel;
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			levelManager.LoadLevel(nextLevel);
		}
	}
}
