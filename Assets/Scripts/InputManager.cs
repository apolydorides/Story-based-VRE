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
    public bool dPressed {get; private set;}
    public bool dUnlocked;
    public bool aPressed {get; private set;}
    public bool aUnlocked;

    // store UDP input
    public int popularLabel = 0;
    public string testLabel = "check";

    // Start is called before the first frame update
    private void Awake()
    {
        current = this;
        wPressed = gPressed = gUnlocked = tPressed = tUnlocked = dPressed = dUnlocked = aPressed = aUnlocked = false;
        wUnlocked = true;
    }

    // Update is called once per frame
    void Update()
    {
        // legacy: String.Equals(latestPacket, "0000003000000", StringComparison.OrdinalIgnoreCase)
        // to check label directly from padded packet
        
        if ((Input.GetKeyDown("w") || (popularLabel== 2) || (popularLabel== 3)) && wUnlocked)
        {
            wPressed = true;
            gPressed = false;
            tPressed = false;
            dPressed = false;
            aPressed = false;
        }
        else if ((Input.GetKeyDown("g") || (popularLabel == 4)) && gUnlocked)
        {
            wPressed = false;
            gPressed = true;
            tPressed = false;
            dPressed = false;
            aPressed = false;
        }
        else if ((Input.GetKeyDown("t") || (popularLabel == 5)) && tUnlocked)
        {
            wPressed = false;
            gPressed = false;
            tPressed = true;
            dPressed = false;
            aPressed = false;
        }
        else if (Input.GetKeyDown("d") || (popularLabel == 6))
        {
            wPressed = false;
            gPressed = false;
            tPressed = false;
            dPressed = true;
            aPressed = false;
        }
        else if (Input.GetKeyDown("a") || (popularLabel == 7))
        {
            wPressed = false;
            gPressed = false;
            tPressed = false;
            dPressed = false;
            aPressed = true;
        }
        else
        {
            wPressed = false;
            gPressed = false;
            tPressed = false;
            aPressed = false;
            dPressed = false;
        }
    }

}
