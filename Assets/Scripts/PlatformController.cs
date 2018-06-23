using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Platform bugs:
        1 - When player jumps under a platform and touches it. If the platform is moving downwards, the player stays 'glued' to the platform, since the jumping
    gravity is not updated.
        2 - Player touching horizontally a platform which is also moving horizontally. The player does not collide with the platform in this case.
*/

public class PlatformController : RaycastController {

    [Header ("Moving Platform Settings")]
    public LayerMask passengerMask;
    public float speed = 2;
    public float waitTime;
    [Range (0, 2)]
    public float easeAmount;
    public bool cyclic = false;

    [Space (10)]
    public Vector3[] localWaypoints;

    Vector3[] globalWaypoints;

    int fromWaypointIndex;
    float percentBetweenWaypoints;
    float nextMoveTime;

    List<PassengerMovement> passengerMovement;
    Dictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D> ();

	protected override void Awake () {
        base.Awake ();

        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++) {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
	}
	
	void Update () {
        UpdateRaycastOrigins ();

        Vector3 velocity = localWaypoints.Length > 0 ? CalculatePlatformMovement () : Vector3.zero;

        CalculatePassengerMovement (velocity);

        MovePassengers (true);
        transform.Translate (velocity);
        MovePassengers (false);
	}

    void OnDrawGizmos () {
        if (localWaypoints != null) {
            Gizmos.color = Color.red;
            float size = 0.3f;

            for (int i = 0; i < localWaypoints.Length; i++) {
                Vector3 globalWaypointPos = Application.isPlaying ? globalWaypoints[i] : (localWaypoints[i] + transform.position);
                Gizmos.DrawLine (globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine (globalWaypointPos - Vector3.right * size, globalWaypointPos + Vector3.right * size);
            }
        }
    }

    float Ease (float x) {
        float a = easeAmount + 1;
        return Mathf.Pow (x, a) / (Mathf.Pow (x, a) + Mathf.Pow (1 - x, a));
    }

    Vector3 CalculatePlatformMovement () {
        if (Time.time < nextMoveTime) {
            return Vector3.zero;
        }

        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance (globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01 (percentBetweenWaypoints);
        float easedPercentage = Ease (percentBetweenWaypoints);

        Vector3 newPos = Vector3.Lerp (globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentage);

        if (percentBetweenWaypoints >= 1) {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            if (!cyclic) {
                if (fromWaypointIndex >= globalWaypoints.Length - 1) {
                    fromWaypointIndex = 0;
                    System.Array.Reverse (globalWaypoints);
                }
            }

            nextMoveTime = Time.time + waitTime;
        }

        return newPos - transform.position;
    }

    void MovePassengers (bool beforeMovePlatform) {
        foreach (PassengerMovement passenger in passengerMovement) {
            if (!passengerDictionary.ContainsKey (passenger.transform)) {
                passengerDictionary.Add (passenger.transform, passenger.transform.GetComponent<Controller2D> ());
            }

            if (passenger.moveBeforePlatform == beforeMovePlatform) {
                passengerDictionary[passenger.transform].Move (passenger.velocity, passenger.standingOnPlatform);
            }
        }
    }

    void CalculatePassengerMovement (Vector3 velocity) {
        HashSet<Transform> movedPassengers = new HashSet<Transform> ();
        passengerMovement = new List<PassengerMovement> ();

        float directionX = Mathf.Sign (velocity.x);
        float directionY = Mathf.Sign (velocity.y);
        
        // Vertically moving platform
        if (velocity.y != 0) {
            float rayLength = Mathf.Abs (velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++) {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                if (hit && hit.distance != 0) {
                    if (!movedPassengers.Contains (hit.transform)) {
                        movedPassengers.Add (hit.transform);

                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

                        passengerMovement.Add (new PassengerMovement (hit.transform, new Vector3 (pushX, pushY), directionY == 1, true));
                    }
                }
            }
        }

        // Horizontally moving platform
        if (velocity.x != 0) {
            float rayLength = Mathf.Abs (velocity.x) + skinWidth;

            for (int i = 0; i < horizontalRayCount; i++) {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                if (hit && hit.distance != 0) {
                    if (!movedPassengers.Contains (hit.transform)) {
                        movedPassengers.Add (hit.transform);

                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth; // The small force downwards forces the passenger to check collisions below, so that he can jump if possible

                        passengerMovement.Add (new PassengerMovement (hit.transform, new Vector3 (pushX, pushY), false, true));
                    }
                }
            }
        }

        // Passenger on top of a horizontally or downward moving platform
        if (directionY == -1 || velocity.y == 0 && velocity.x != 0) {
            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRayCount; i++) {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up, rayLength, passengerMask);

                if (hit && hit.distance != 0) {
                    if (!movedPassengers.Contains (hit.transform)) {
                        movedPassengers.Add (hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        passengerMovement.Add (new PassengerMovement (hit.transform, new Vector3 (pushX, pushY), true, false));
                    }
                }
            }
        }
    }

    struct PassengerMovement {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement (Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform) {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }
	
}
