using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum IngameState {
    Running, Paused, GameOver
}

[RequireComponent (typeof (PlayerInput))]
public class GameManager : MonoBehaviour
{

    public LevelManager levelManager;
    public GameObject playerHud;

    [Space (10)]
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
        if (currentGameState != IngameState.GameOver) {
            if (playerInput.IsPausePressedDown ()) {
                TogglePause ();
            }

            if (blockPlayerCoroutine == null && playerInput.IsResetPressedDown ()) {
                DisableCurrentPlayer ();
            }
        }
	}

    public void DisableCurrentPlayer () {
        lives.SubtractLife ();
        if (lives.currentLivesCount <= 0) {
            GameOver ();
        } else {
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
        playerInput.BlockPlayerInput (true);

        yield return new WaitForSeconds (1);

        playerInput.BlockPlayerInput (false);

        blockPlayerCoroutine = null;
    }

    #region Ingame UI
    public void TogglePause () {
        switch (currentGameState) {
            case IngameState.Running:
                PauseGame ();
                break;
            case IngameState.Paused:
                UnpauseGame ();
                break;
        }
    }

    private void PauseGame () {
        pauseScreen.ShowPauseScreen (true);
        Time.timeScale = 0;
        playerInput.BlockPlayerInput (true);
        currentGameState = IngameState.Paused;
    }

    private void UnpauseGame () {
        pauseScreen.ShowPauseScreen (false);
        Time.timeScale = 1;
        if (blockPlayerCoroutine == null) {
            playerInput.BlockPlayerInput (false);
        }
        currentGameState = IngameState.Running;
    }

    public void RestartLevel () {
        Time.timeScale = 1;
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    public void ReturnToMainMenu () {
        Time.timeScale = 1;
        levelManager.LoadLevel ("MainMenu");
    }

    private void GameOver () {
        playerInput.BlockPlayerInput (true);
        playerHud.SetActive (false);
        currentGameState = IngameState.GameOver;
        pauseScreen.ShowGameOverScreen ();
    }
    #endregion

}
