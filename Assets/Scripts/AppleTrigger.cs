using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Animations.Rigging;

// This is a script including the logic of the event trigger that exists in the player's path
public class AppleTrigger : MonoBehaviour
{
    // public Transform eventSpot;
    public List<Transform> apples;
    public List<Transform> appleGrabHandles;
    public Transform playerEndPosition;
    public Transform fawnEndPosition;
    Rig playerRig;
    TwoBoneIKConstraint wavingConstraint;
    TwoBoneIKConstraint appleConstraint;

    void Start() 
    {
        playerRig = GameObject.FindGameObjectWithTag("Runtime Rig").GetComponent<Rig>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fawn"))
        {
            // removing this collider's trigger feature to avoid hitting it again on exit
            Collider thisCollider = gameObject.GetComponent<Collider>();
            thisCollider.isTrigger = false;
            // debug check
            Debug.Log("AppleTrigger.cs OnTriggerEnter(Collider other)");
            // from unity editor we pass the grab handles and apples of each event
            EventManager.Instance.setObjectives(appleGrabHandles, apples, true);
            // passing the positions the player and fawn must interpolate to
            EventManager.Instance.playerTargetPosition = playerEndPosition;
            EventManager.Instance.fawnTargetPosition = fawnEndPosition;
            // finally call activity enter which uses C# event system
            GameEvents.current.GatheringEnter();
        }
        
    }
}

