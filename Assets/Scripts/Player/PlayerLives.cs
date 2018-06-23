using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLives : MonoBehaviour {

    [Range (1, 15)]
    public int totalLives = 3;

    public int currentLivesCount { get; private set; }

    private void Awake () {
        currentLivesCount = totalLives;
    }

    public void AddLife () {
        currentLivesCount++;
    }

    public void SubtractLife () {
        currentLivesCount--;
    }

}
