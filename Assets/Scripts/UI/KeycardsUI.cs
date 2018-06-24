using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeycardsUI : MonoBehaviour {

    public Image keycardSlotPrefab;

    [Space (10)]
    public Color deactiveSlotColor = Color.black;
    public Color activeSlotColor = Color.white;

    public Dictionary<Keycard, Image> keycardSlots;
    
    void Start () {
        Keycard[] keycards = FindObjectsOfType<Keycard> ();

        keycardSlots = new Dictionary<Keycard, Image> ();
        foreach (Keycard keycard in keycards) {
            Image newImage = Instantiate (keycardSlotPrefab, transform);
            newImage.color = deactiveSlotColor;
            newImage.sprite = keycard.GetComponent<SpriteRenderer> ().sprite;
            
            keycardSlots.Add (keycard, newImage);

            keycard.OnKeycardGet += UpdateSlots;
        }
	}

    void UpdateSlots () {
        foreach (var pair in keycardSlots) {
            if (pair.Key.isActivated) {
                pair.Value.color = activeSlotColor;
            }
        }
    }

}
