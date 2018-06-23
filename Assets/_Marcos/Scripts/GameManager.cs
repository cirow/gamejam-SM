using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (PlayerInput))]
public class GameManager : MonoBehaviour
{

    public GameObject disabledPlayerPrefab;

    private PlayerInput playerInput;

    private Player player;

    public Coroutine blockPlayerCoroutine;

	void Awake () {
        playerInput = GetComponent<PlayerInput> ();

        player = FindObjectOfType<Player> ();
	}

	void Update () {
		if (blockPlayerCoroutine == null && playerInput.IsResetPressedDown ()) {
            Vector3 playerPosition = player.position;
            player.ResetPlayerPosition ();
            CreateCadaver (playerPosition);

            blockPlayerCoroutine = StartCoroutine (BlockPlayerInputTemporarily ());
        }
	}

    private void CreateCadaver (Vector3 position) {
        Instantiate (disabledPlayerPrefab, position, Quaternion.identity);
    }

    private IEnumerator BlockPlayerInputTemporarily () {
        playerInput.blockPlayerInput = true;
        yield return new WaitForSeconds (1f);
        playerInput.blockPlayerInput = false;

        blockPlayerCoroutine = null;
    }

	

}
