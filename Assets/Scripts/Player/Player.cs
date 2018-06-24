using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

    public Text environmentUI;

    [Space (10)]
    public int maxJumps = 1;
    public float moveSpeed = 6;
    public float sprintSpeed = 10;
    public float acceleration = 10;

    [Space (10)]
    public float jumpRequestTime = 0.1f; // Make a request for a jump, so that if the player can't immediately execute the jump, it tries again inside the request window

    [Header ("Interactions")]
    public Vector3 interactionDescriptionOffset = new Vector3 (0, 50, 0);
    public int interactionRadius = 1;

    [Header ("Lague's Settings")]
    public float maxJumpHeight = 4;
    public float minJumpHeight = 0.5f;
    public float timeToJumpApex = 0.5f;
    public float accelerationTimeGrounded = 0.1f;
    public float accelerationTimeAirborne = 0.2f;


    public Vector2 position {
        get {
            if (invincible) {
                return deathPosition;
            } else {
                return transform.position;
            }
        }
    }
    public bool facingRight { get; private set; }
    public bool moving { get; private set; }
    public bool sprinting { get; private set; }

    public bool invincible { get; private set; }

    public bool grounded
    {
        get
        {
            return controller.collisions.below;
        }
    }

    private int itemLayerMask;

    private GameManager gameManager;
    private CameraBehaviour cameraBehaviour;
    private PlayerInput playerInput;

    //private SpawnPoint[] spawnPoints;
    [SerializeField]
    private Transform spawnPoint;

    private Rigidbody2D rigid;

    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    private float currentSpeed;
    private bool hasJumped;

    private GameObject itemInRange;
    private Item itemScript;

    private float lastJumpRequest;

    // Sebastian Lague's
    private Controller2D controller;

    private float gravity = -20;
    private float maxJumpVelocity;
    private float minJumpVelocity;

    private Vector3 velocity;
    public Vector3 Velocity
    {
        get
        {
            return velocity;
        }
    }

    private float velocityXSmoothing;

    private float timeToWallUnstick;
    // >>>>>>>>>>>>>>>>>>>
    
    // Lever
    private GameObject leverInRange;
    private Lever leverScript;
    // >>>>>>>>>>>>>>>>>>>

    private Vector3 deathPosition;

    #region MonoBehaviour
    void Awake () {
        gameManager = FindObjectOfType<GameManager> ();
        cameraBehaviour = FindObjectOfType<CameraBehaviour> ();
        playerInput = FindObjectOfType<PlayerInput> ();

        boxCollider = GetComponent<BoxCollider2D> ();
        spriteRenderer = GetComponentInChildren<SpriteRenderer> ();

        if (playerInput == null) {
            playerInput = gameObject.AddComponent<PlayerInput> ();
        }

        itemLayerMask = LayerMask.GetMask ("Item");
        
        rigid = GetComponent<Rigidbody2D> ();

        controller = GetComponent<Controller2D> ();

        facingRight = true;

        LaguesAwake ();

        lastJumpRequest = -jumpRequestTime;
	}

    void Update () {
        if (playerInput.IsJumpPressedDown ()) {
            lastJumpRequest = Time.time;
        }

        LaguesUpdate ();
        
        UpdateFacingDirection ();

		CheckInteractableRadius();
		ToggleLever();
        //CheckItemRadius ();
        //GrabItem ();
        UpdatePlayerUI ();
    }
    #endregion

    #region Sebastian Lague's
    private void LaguesAwake () {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs (gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);

        print ("Gravity: " + gravity + " | Jump Velocity: " + maxJumpVelocity);
    }

    private void LaguesUpdate () {
        Vector2 input = playerInput.GetRawMovementInput ();
        int wallDirX = (controller.collisions.left) ? -1 : 1;
        
        float targetVelocityX = input.x * (sprinting ? sprintSpeed : moveSpeed);
        velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne);
        
        moving = input.x != 0;
        
        // Jump Input
        if (lastJumpRequest + jumpRequestTime > Time.time) {
            if (controller.collisions.below) { // Regular Jump
                velocity.y = maxJumpVelocity;
            }
        }
        if (playerInput.IsJumpPressedUp ()) {
            if (velocity.y > minJumpVelocity) {
                velocity.y = minJumpVelocity;
            }
        }
        
        // todo 1) Keep sprinting speed during a jump (to simulate momentum)
        // todo 2) Disallow sprinting if the player was not sprinting when he triggered a jump
        sprinting = playerInput.IsSprinting ();

        velocity.y += gravity * Time.deltaTime;
        controller.Move (velocity * Time.deltaTime, input);

        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
    }
    #endregion
    
    private void UpdateFacingDirection () {
        if (moving && velocity.x != 0) {
            facingRight = velocity.x > 0;
        }
    }

    public void ResetPlayerPosition () {
        StartCoroutine (WaitToReset ());
    }

    private IEnumerator WaitToReset () {
        invincible = true;
        HidePlayer ();
        playerInput.BlockPlayerInput (true);
        deathPosition = transform.position;

        yield return new WaitForSeconds (1.2f);

        facingRight = true;
        velocity = Vector3.zero;
        transform.position = spawnPoint.position;

        invincible = false;
        ShowPlayer ();
        playerInput.BlockPlayerInput (false);
    }

    public void TakeHit () {
        gameManager.DisableCurrentPlayer (false);
    }

    public void ShowPlayer () {
        boxCollider.enabled = true;
        spriteRenderer.enabled = true;
    }

    public void HidePlayer () {
        boxCollider.enabled = false;
        spriteRenderer.enabled = false;
    }

    #region Item Interaction
    private void GrabItem () {
        if (playerInput.IsUseItemPressedDown () && itemInRange) {
            Destroy (itemInRange);
            itemInRange = null;

            if (itemScript) {
                //playerSlots.EquipItem (itemScript);
                //todo Usar o item quando passar por ele
                itemScript = null;
            }
        }
    }

    private void CheckItemRadius () {
        Collider2D collider = Physics2D.OverlapCircle (transform.position, interactionRadius, itemLayerMask);

        if (collider) {
            //Debug.DrawLine (transform.position, collider.transform.position);
            itemInRange = collider.gameObject;
            itemScript = itemInRange.GetComponent<Item> ();
        } else {
            itemInRange = null;
            itemScript = null;
        }
    }

    private void UpdatePlayerUI () {
        if (environmentUI != null) {
            if (itemInRange) {
                if (itemScript) {
                    environmentUI.text = "Grab\n" + itemScript.itemName;
                } else {
                    environmentUI.text = "Grab\nItem";
                }

                environmentUI.transform.position = Camera.main.WorldToScreenPoint (itemInRange.transform.position) + interactionDescriptionOffset;

                environmentUI.gameObject.SetActive (true);
            } else {
                environmentUI.gameObject.SetActive (false);
            }
        }
    }
    #endregion

    #region Lever Interaction
    private void CheckInteractableRadius () {

        Collider2D collider = Physics2D.OverlapCircle (transform.position, interactionRadius, LayerMask.GetMask ("Lever"));

        if (collider) {
            Debug.DrawLine (transform.position, collider.transform.position);
            leverInRange = collider.gameObject;
            leverScript = leverInRange.GetComponent<Lever> ();
        } else {
            leverInRange = null;
            leverScript = null;
        }
    }

    private void ToggleLever () {
        if (Input.GetButtonDown ("Use Item") && leverInRange) {
            leverScript.Activate ();
        }
    }

    public Vector2 GetSpawnPoint()
    {
        return spawnPoint.position;
    }

    public void ChangeSpawn(Vector2 newSpawn)
    {
        spawnPoint.position = newSpawn;

    }
    #endregion
}
