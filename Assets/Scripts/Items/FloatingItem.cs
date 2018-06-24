using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingItem : MonoBehaviour {

    [Header ("Floating Settings")]
    public float groundOffset = 1;

	public float degreesPerSecond = 15.0f;
	public float amplitude = 0.25f;
	public float frequency = 1f;


	Vector3 posOffset = new Vector3();
	Vector3 tempPos = new Vector3();

	void Start () {
		posOffset = transform.position;
	}

	void FixedUpdate()
	{
		
		transform.Rotate(new Vector3(0, 80, 0) * Time.deltaTime);
		// Float up/down with a Sin()
		tempPos = posOffset;
		tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

		transform.position = tempPos;

	}



}
