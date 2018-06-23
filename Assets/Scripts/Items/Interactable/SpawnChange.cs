using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChange : Interactable {

    private Vector2 oldSpawnPoint;


    public override bool Interact()
    {
        if (!isOn)
        {
            oldSpawnPoint = FindObjectOfType<Player>().GetSpawnPoint();
            FindObjectOfType<Player>().ChangeSpawn((Vector2)transform.position);
            isOn = true;
        }
        else
        {
            FindObjectOfType<Player>().ChangeSpawn(oldSpawnPoint);
            isOn = false;
        }
        return true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
