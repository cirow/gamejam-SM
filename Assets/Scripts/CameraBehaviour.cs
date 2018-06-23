using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {
    
    [Space (10)]
    public float followSpeed = 8;
    public float horizontalSpeed = 4;

    [Space (10)]
    public Vector2 cameraOffset = new Vector2 (2, 1.5f);

    private Player player;
    private Vector2 currentOffset;

    private float cameraDistance;
    
	void Awake () {
        player = FindObjectOfType<Player> ();
        currentOffset = new Vector2 (player.facingRight ? cameraOffset.x : -cameraOffset.x, cameraOffset.y);

        cameraDistance = -10;
	}
	
	void Start () {
		
	}

    void Update () {

        Vector3 targetPos = CalculateTargetPosition ();
        transform.position = Vector3.Lerp (transform.position, targetPos, Time.deltaTime * followSpeed);
        //transform.position = targetPos;
    }

    private Vector3 CalculateTargetPosition () {
        Vector3 targetPosition;

        Vector2 targetOffset = new Vector2 (player.facingRight ? cameraOffset.x : -cameraOffset.x, cameraOffset.y);
        
        currentOffset = Vector2.Lerp (currentOffset, targetOffset, Time.deltaTime * horizontalSpeed);

        Vector2 playerPos = player.position + currentOffset;
        targetPosition = new Vector3 (playerPos.x, playerPos.y, cameraDistance);

        return targetPosition;
    }

}
