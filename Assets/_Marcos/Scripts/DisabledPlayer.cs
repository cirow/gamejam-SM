using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledPlayer : MonoBehaviour {

    private Rigidbody2D rigid;

    private bool isKinematic = false;

    private void Awake () {
        rigid = GetComponent<Rigidbody2D> ();
    }

    void Start () {

    }


    void Update () {
        if (!isKinematic && rigid.velocity.y == 0) {
            rigid.bodyType = RigidbodyType2D.Kinematic;
            isKinematic = true;
        }
    }

}
