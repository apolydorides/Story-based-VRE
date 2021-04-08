using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    
    public static GameEvents current;

    private void Awake()
        {
            current = this;
        }
    

    // start of whole gathering/feeding event
    public event Action onGatheringEnter;
    public void GatheringEnter()
    {
        if (onGatheringEnter != null)
        {
            onGatheringEnter();
        }
    }

    // end of gathering/feeding event
    public event Action onFeedingExit;
    public void FeedingExit()
    {
        if (onFeedingExit != null)
        {
            onFeedingExit();
        }
    }

    public event Action onActivityExit;
    public void ActivityExit()
    {
        if (onActivityExit != null)
        {
            onActivityExit();
        }
    }

    // when item picked up and would need to then track hand etc. (e.g. apple picked and on its way to be stored)
    public event Action onItemGrab;
    public void ItemGrab()
    {
        if (onItemGrab != null)
        {
            onItemGrab();
        }
    }

    // This will handle the end of each iteraction with items (e.g. when apple has been picked up and then stored)
    public event Action onItemComplete;
    public void ItemComplete()
    {
        if (onItemComplete != null)
        {
            onItemComplete();
        }
    }

    public event Action onAppleEaten;
    public void AppleEaten()
    {
        if (onAppleEaten != null)
        {
            onAppleEaten();
        }
    }
}
