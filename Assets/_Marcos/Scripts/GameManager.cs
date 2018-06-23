using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum IngameState {
    Running, Paused, GameOver
}

[RequireComponent (typeof (PlayerInput))]
public class GameManager : MonoBehaviour {

    public GameObject disabledPlayerPrefab;

    public IngameState currentGameState { get; private set; }

    private PlayerInput playerInput;
    private PlayerLives lives;

    private PauseScreen pauseScreen;

    private Player player;

    private Coroutine blockPlayerCoroutine;

    private bool gamePaused = false;

	void Awake () {
        playerInput = GetComponent<PlayerInput> ();
        lives = GetComponent<PlayerLives> ();

        pauseScreen = GetComponentInChildren<PauseScreen> ();

        player = FindObjectOfType<Player> ();

        gamePaused = false;
	}

    void Update () {
        if (playerInput.IsPausePressedDown ()) {
            TogglePause ();
        }

		if (blockPlayerCoroutine == null && playerInput.IsResetPressedDown ()) {
            DisableCurrentPlayer ();
        }
	}

    public void DisableCurrentPlayer () {
        Vector3 playerPosition = player.position;
        player.ResetPlayerPosition ();
        CreateCadaver (playerPosition);

        lives.SubtractLife ();
        if (lives.currentLivesCount <= 0) {
            GameOver ();
        }

        blockPlayerCoroutine = StartCoroutine (BlockPlayerInputTemporarily ());
    }

    private void CreateCadaver (Vector3 position) {
        Instantiate (disabledPlayerPrefab, position, Quaternion.identity);
    }

    private IEnumerator BlockPlayerInputTemporarily () {
        playerInput.BlockPlayerInput (true);

        yield return new WaitForSeconds (1);

        playerInput.BlockPlayerInput (false);

        blockPlayerCoroutine = null;
    }

    #region Ingame UI
    public void TogglePause () {
        if (gamePaused) {
            UnpauseGame ();
        } else {
            PauseGame ();
        }
    }

    private void PauseGame () {
        pauseScreen.ShowPauseScreen (true);
        Time.timeScale = 0;
        playerInput.BlockPlayerInput (true);
        gamePaused = true;
    }

    private void UnpauseGame () {
        pauseScreen.ShowPauseScreen (false);
        Time.timeScale = 1;
        if (blockPlayerCoroutine == null) {
            playerInput.BlockPlayerInput (false);
        }
        gamePaused = false;
    }

    public void RestartLevel () {
        UnpauseGame ();
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    private void GameOver () {

    }
    #endregion

}
