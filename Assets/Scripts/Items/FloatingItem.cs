using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingItem : Item {

    [Header ("Floating Settings")]
    public float groundOffset = 1;

    void Awake () {
        PositionItem ();
    }

    private void PositionItem () {
        RaycastHit2D[] hits = Physics2D.RaycastAll (transform.position, Vector2.down, 10);

        foreach (RaycastHit2D hit in hits) {
            if (hit.transform != transform) {
                transform.position = hit.point + Vector2.up * groundOffset;
                break;
            }
        }
    }

}
