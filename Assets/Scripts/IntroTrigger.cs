using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IntroTrigger : MonoBehaviour
{
    Rig playerRig;
    Transform wrist;
    TwoBoneIKConstraint wavingConstraint;
    TwoBoneIKConstraint appleConstraint;
    int waveCounter = 0;
    int wavesCompleted = 0;
    bool wavingOccuring = false;
    string waveDirection = "null";

    void Start()
    {
        // get player's 'wrist' - it's actually the effector as skeleton couldn't be moved due to animation controller
        wrist = GameObject.FindGameObjectWithTag("WavingEffector").GetComponent<Transform>();
        // access player's rig from here so that coroutine can transition it smoothly
        playerRig = GameObject.FindGameObjectWithTag("Runtime Rig").GetComponent<Rig>();
        wavingConstraint = playerRig.GetComponentsInChildren<TwoBoneIKConstraint>()[0];
        wavingConstraint.weight = 0;
        appleConstraint = playerRig.GetComponentsInChildren<TwoBoneIKConstraint>()[1];
        appleConstraint.weight = 0;
        InputManager.current.aUnlocked = false;
        InputManager.current.dUnlocked = false;
    }

    void Update()
    {
        if (waveCounter >= 10 && wavesCompleted == 10)
        {
            waveCounter = -1;
        }

        if (wavesCompleted == 10)
        {
            InputManager.current.dUnlocked = false;
            InputManager.current.aUnlocked = false;
            StartCoroutine("LowerHand");
        }

        if ((wavingConstraint.weight == 1f) && waveCounter >= 0)
        {
            if (InputManager.current.dPressed && !InputManager.current.aUnlocked)
            {
                InputManager.current.aUnlocked = true;
                InputManager.current.dUnlocked = false;
                waveCounter++;
                Debug.Log(waveCounter);
                waveDirection = "right";
            }
            else if (InputManager.current.aPressed && !InputManager.current.dUnlocked)
            {
                InputManager.current.dUnlocked = true;
                InputManager.current.aUnlocked = false;
                waveCounter++;
                Debug.Log(waveCounter);
                waveDirection = "left";
            }
        }

        if (!wavingOccuring && waveCounter > wavesCompleted)
        {
            wavingOccuring = true;
            IEnumerator coroutine = WaveHand(waveDirection);
            StartCoroutine(coroutine);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // removing this collider's trigger feature to avoid hitting it again on exit
        Collider thisCollider = gameObject.GetComponent<Collider>();
        thisCollider.isTrigger = false;
        EventManager.Instance.motionLocked = true; // will go into case 3 of playermotion script (i.e. player stops)
        // access plaayer's rig to make him wave
        GameObject Player = other.gameObject;
        StartCoroutine("RaiseHand");
    }

    IEnumerator RaiseHand()
    {
        while (wavingConstraint.weight < 0.99f)
        {
            wavingConstraint.weight += 0.7f*(Time.deltaTime + (Time.deltaTime * (1 - wavingConstraint.weight)));
            yield return new WaitForFixedUpdate();
        }
        wavingConstraint.weight = 1;
        yield return null;
    }
    IEnumerator LowerHand()
    {
        while (wavingConstraint.weight > 0.01f)
        {
            wavingConstraint.weight -= 0.7f*(Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        wavingConstraint.weight = 0;
        int counter = 0;
        foreach(Transform child in playerRig.transform)
        {
            if (counter == 0)
            {
                child.gameObject.SetActive(false);
            }
            else if (counter == 1)
            {
                child.gameObject.SetActive(true);
            }
            counter++;
        }
        playerRig.weight = 0;
        appleConstraint.weight = 1;
        EventManager.Instance.motionLocked = false;
        GameObject.FindGameObjectWithTag("Fawn").GetComponent<FawnMotion>().DecoupleSpeed = false;
        yield return null;
    }

    IEnumerator WaveHand(string direction)
    {
        float x = wrist.rotation.eulerAngles.x;
        float y = wrist.rotation.eulerAngles.y;
        float z = wrist.rotation.eulerAngles.z;
        float angleRotation = 0f;
        while (angleRotation < 24.95f)
        {
            angleRotation += 50f*Time.deltaTime;
            if (direction == "right")
            {
                wrist.rotation = Quaternion.Euler( x - angleRotation, y, z);
            }
            else if (direction == "left")
            {
                wrist.rotation = Quaternion.Euler( x + angleRotation, y, z);
            }
        
            yield return new WaitForEndOfFrame();
        }

        while (angleRotation > 0.05f)
        {
            angleRotation -= 50f*Time.deltaTime;
            if (direction == "right") 
            {
                wrist.rotation = Quaternion.Euler( x - angleRotation, y, z);
            }
            else if (direction == "left")
            {
                wrist.rotation = Quaternion.Euler( x + angleRotation, y, z);
            }

            yield return new WaitForEndOfFrame();
        }
        wavesCompleted++;
        if (waveCounter > wavesCompleted)
        {
            if (direction == "right")
            {
                IEnumerator myCoroutine1 = WaveHand("left");
                StartCoroutine(myCoroutine1);
            }
            else if (direction == "left")
            {
                IEnumerator myCoroutine2 = WaveHand("right");
                StartCoroutine(myCoroutine2);
            }
        }
        else if (waveCounter == wavesCompleted)
        {
            wavingOccuring = false;
        }

        yield return null;
    }



}
