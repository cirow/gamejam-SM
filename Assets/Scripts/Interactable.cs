﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    public bool toggable;

    [HideInInspector]
    public bool isOn = false;

    public abstract bool Interact();
    
    void Start()
    {

    }

    void Update()
    {

    }

}