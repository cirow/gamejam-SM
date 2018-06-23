using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour {

    public GameObject pauseCanvas;
    public Button resumeButton;
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


}
