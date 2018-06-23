using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour {

    public Text livesText;

    [Space (10)]
    [Range (1, 15)]
    public int totalLives = 3;

    public int currentLivesCount { get; private set; }

    private void Awake () {
        currentLivesCount = totalLives;
        livesText.text = currentLivesCount.ToString ();
    }

    public void AddLife () {
        currentLivesCount++;
        livesText.text = currentLivesCount.ToString ();
    }

    public void SubtractLife () {
        currentLivesCount--;
        livesText.text = currentLivesCount.ToString ();
    }

}
