using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public bool blockPlayerInput;

    public bool JumpDown () {
        return Input.GetButtonDown ("Jump");
    }

}
