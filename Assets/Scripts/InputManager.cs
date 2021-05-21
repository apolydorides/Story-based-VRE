using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager current;

    public bool wPressed {get; private set;}
    public bool wUnlocked;
    public bool gPressed {get; private set;}
    public bool gUnlocked;
    public bool tPressed {get; private set;}
    public bool tUnlocked;

    // store UDP input
    string latestPacket = '';

    // Start is called before the first frame update
    private void Awake()
    {
        current = this;
        wPressed = gPressed = gUnlocked = tPressed = tUnlocked = false;
        wUnlocked = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("w") || String.Compare(getLatestUDPPacket(), '000000100000000'))
        {
            wPressed = true;
            gPressed = false;
            tPressed = false;
        }
        else if (Input.GetKeyDown("g") || String.Compare(getLatestUDPPacket(), '000000010000000'))
        {
            wPressed = false;
            gPressed = true;
            tPressed = false;
        }
        else if (Input.GetKeyDown("t") || String.Compare(getLatestUDPPacket(), '000000001000000'))
        {
            wPressed = false;
            gPressed = false;
            tPressed = true;
        }
        else
        {
            wPressed = false;
            gPressed = false;
            tPressed = false;
        }
    }


}
