using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDebug : MonoBehaviour {

    public Text aboveLabel;
    public Text belowLabel;
    public Text leftLabel;
    public Text rightLabel;
    public Text sprintingLabel;

    [Space (10)]
    public bool showItemRadius = false;

    private Player player;
    private Controller2D controller;

    void Awake () {
        player = GetComponent<Player> ();
        controller = GetComponent<Controller2D> ();
    }

    void Update () {
        UpdateBoolResult (aboveLabel, "above", controller.collisions.above);
        UpdateBoolResult (belowLabel, "below", controller.collisions.below);
        UpdateBoolResult (leftLabel, "left", controller.collisions.left);
        UpdateBoolResult (rightLabel, "right", controller.collisions.right);
        UpdateBoolResult (sprintingLabel, "sprinting", player.sprinting);
    }

    void OnDrawGizmos () {
        if (showItemRadius && player != null) {
            Gizmos.DrawWireSphere (transform.position, player.interactionRadius);
        }
    }

    private void UpdateBoolResult (Text label, string name, bool value) {
        if (label) {
            label.text = name + " = <color=" + (value ? "green>true" : "red>false") + "</color>";
        }
    }

}
