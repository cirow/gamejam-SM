
/***
  The majority of the code in this class was obtained from the 2D Platformer Controller for Unity 5 by Sebastian Lague.
  All videos of his implementation can be found on an YouTube playlist, at the following URL:
  - https://www.youtube.com/playlist?list=PLFt_AvWsXl0f0hqURlhyIoAabKPgRsqjz
  You can support his work through Patreon:
  - https://www.patreon.com/SebastianLague
 ***/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller2D : RaycastController {

    public float maxClimbAngle = 80;
    public float maxDescendAngle = 75;

    [HideInInspector]
    public CollisionInfo collisions;

    private Vector2 playerInput;
    
    protected override void Awake () {
        base.Awake ();
        collisions.faceDir = 1;
    }


    public void Move (Vector3 velocity, bool standingOnPlatform = false) {
        Move (velocity, Vector2.zero, standingOnPlatform);
    }

    public void Move (Vector3 velocity, Vector2 input, bool standingOnPlatform = false) {
        UpdateRaycastOrigins ();
        collisions.Reset ();
        collisions.velocityOld = velocity;
        playerInput = input;

        if (velocity.x != 0) {
            collisions.faceDir = (int) Mathf.Sign (velocity.x);
        }

        if (velocity.y < 0) {
            DescendSlope (ref velocity);
        }

        HorizontalCollisions (ref velocity);

        if (velocity.y != 0) {
            VerticalCollisions (ref velocity);
        }

        transform.Translate (velocity);

        if (standingOnPlatform) {
            collisions.below = true;
        }
    }

    void HorizontalCollisions (ref Vector3 velocity) {
        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs (velocity.x) + skinWidth;

        if (Mathf.Abs (velocity.x) < skinWidth) {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit) {
                if (hit.collider.tag == "Through" || hit.distance == 0) { // This avoids a horizontal movement problem that happens when the player is inside an obstacle, e.g. a platform moving downwards when passing through the player
                    continue;
                }
                
                float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxClimbAngle) { // Bottommost array
                    if (collisions.descendingSlope) {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }
                    float distanceToSlopeStart = 0; // Avoid slope climbing before the player collider has properly reached the slope
                    if (slopeAngle != collisions.slopeAngleOld) {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope (ref velocity, slopeAngle); // Climb Slope
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance; // Updates the ray length so that it's shortened, avoiding setting the velocity to a position further down (or up);

                    if (collisions.climbingSlope) {
                        velocity.y = Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void VerticalCollisions (ref Vector3 velocity) {
        float directionY = Mathf.Sign (velocity.y);
        float rayLength = Mathf.Abs (velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay (rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit) {
                if (hit.collider.tag == "Through") {
                    if (directionY == 1 || hit.distance == 0) {
                        continue; // The 'continue' keyword skips the rest of the iteration of the current For loop
                    }
                    if (collisions.fallingThroughPlatform) {
                        continue;
                    }
                    if (playerInput.y == -1) { // Allows the player to fall down the platform when pressing down
                        collisions.fallingThroughPlatform = true;
                        Invoke ("ResetFallingThroughPlatform", 0.1f);
                        continue;
                    }
                }

                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance; // Updates the ray length so that it's shortened, avoiding setting the velocity to a position further down (or up);

                if (collisions.climbingSlope) {
                    velocity.x = velocity.y / Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign (velocity.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

        // todo Move the following code to a new method
        if (collisions.climbingSlope) { // Check if there is a new slope during a slope climb
            float directionX = Mathf.Sign (velocity.x);
            rayLength = Mathf.Abs (velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit) { // If a new slope is found, update the stored slope angle
                float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);

                if (slopeAngle != collisions.slopeAngle) {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    void ClimbSlope (ref Vector3 velocity, float slopeAngle) {
        float moveDistance = Mathf.Abs (velocity.x);
        float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY) { // If not jumping on slope
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }
    
    void DescendSlope (ref Vector3 velocity) {
        float directionX = Mathf.Sign (velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft; // If moving right, raycast from the bottom left origin (the opposite origin, since it will be the one touching the slope), and vice-versa
        RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit) {
            float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);

            // todo Join those ifs in a new method CanDescendSlope
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) { // if not flat surface and under the maxDescendAngle
                if (Mathf.Sign (hit.normal.x) == directionX) { // check if the X axis of the hit normal (perpendicular vector to the slope) equals the directionX, means we're moving down the slope
                    if (hit.distance - skinWidth <= Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x)) { // check if close enough to the slope to begin properly descending (in cases of player falling down, since the ray is cast infinitely downwards)
                        float moveDistance = Mathf.Abs (velocity.x);
                        float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
                        velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    void ResetFallingThroughPlatform () {
        collisions.fallingThroughPlatform = false;
    }

    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;
        public int faceDir;
        public bool fallingThroughPlatform;

        public void Reset () {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

}
