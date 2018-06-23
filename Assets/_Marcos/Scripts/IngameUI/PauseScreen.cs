using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour {

    public GameObject pauseCanvas;

    [Space (10)]
    public Text titleText;
    public Button resumeButton;
    public Button restartButton;
    public Button menuButton;
    
    private void Start () {
        ShowPauseScreen (false);
    }

    public void ShowPauseScreen (bool value) {
        if (value) {
            pauseCanvas.SetActive (true);
            resumeButton.Select ();
        } else {
            pauseCanvas.SetActive (false);
            menuButton.Select ();
        }
    }

    public void ShowGameOverScreen () {
        titleText.text = "No More Robs Left";
        resumeButton.gameObject.SetActive (false);

        pauseCanvas.SetActive (true);
        restartButton.Select ();
    }

}
