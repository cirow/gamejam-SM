using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public bool blockPlayerInput;

    public bool IsResetPressedDown () {
        return Input.GetKeyDown (KeyCode.R);
    }

    public bool IsJumpPressedDown () {
        if (blockPlayerInput) {
            return false;
        }

        return Input.GetButtonDown ("Jump");
    }

    public bool IsJumpPressedUp () {
        if (blockPlayerInput) {
            return false;
        }

        return Input.GetButtonUp ("Jump");
    }

    public bool IsUseItemPressedDown () {
        if (blockPlayerInput) {
            return false;
        }

        return Input.GetButtonDown ("Use Item");
    }

    public Vector2 GetRawMovementInput () {
        if (blockPlayerInput) {
            return Vector2.zero;
        }

        return new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
    }

    public bool IsSprinting () {
        if (blockPlayerInput) {
            return false;
        }

        // Sprint as axis, since the Left and Right Triggers of the Xbox Controller are mapped as triggers
        return Input.GetButton ("Sprint") || Input.GetAxis ("Sprint") != 0;
    }

    public bool IsPausePressedDown () {
        return Input.GetKeyDown (KeyCode.Escape);
    }

    public void BlockPlayerInput (bool value) {
        //if (value) {
        //    print ("Input do player bloqueado");
        //} else {
        //    print ("Input do player desbloqueado");
        //}

        blockPlayerInput = value;
    }

}
