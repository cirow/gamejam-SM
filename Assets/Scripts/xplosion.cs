using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xplosion : MonoBehaviour {


    private void Awake()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, (float)-.3);
    }

    // Use this for initialization
    void Start () {
        AudioManager.instance.PlayXplosion();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FinishXplosion()
    {

        Destroy(gameObject);
    }
}
