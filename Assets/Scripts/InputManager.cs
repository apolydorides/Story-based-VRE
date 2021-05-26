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
    public int popularLabel = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        current = this;
        wPressed = gPressed = gUnlocked = tPressed = tUnlocked = false;
        wUnlocked = true;
        foreach (int i in packets)
        {
            packets[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // legacy: String.Equals(latestPacket, "0000003000000", StringComparison.OrdinalIgnoreCase)
        // to check label directly from padded packet
        
        if (Input.GetKeyDown("w") || popularLabel == 1)
        {
            wPressed = true;
            gPressed = false;
            tPressed = false;
        }
        else if (Input.GetKeyDown("g") || popularLabel == 2)
        {
            wPressed = false;
            gPressed = true;
            tPressed = false;
        }
        else if (Input.GetKeyDown("t") || popularLabel = 3)
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

