using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newTutorialController : MonoBehaviour
{
    protected Animator tutorialHandAnimator;
    public float timer;
    float timerThreshold;
    string voiceString;
    SkinnedMeshRenderer handModel;
    AudioSource voiceInstructions;
    bool firstVoice;

    // Start is called before the first frame update
    void Start()
    {
        handModel = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        handModel.enabled = false;
        timer = 0f;
        firstVoice = true;

        tutorialHandAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // This handles which voice clip and text message to deliver to the user as well as handles the resets when the correct input is given
        if (InputManager.current.wUnlocked)
        {
            voiceString = "Move Forward";
            TextController.Instance.textInstructions.text = "Close your grip to move forward";
            tutorialHandAnimator.SetBool("Grip", true);
            if (InputManager.current.wPressed)
            {
                timer = 0f;
                TextController.Instance.TextActive(false);
                firstVoice = true;
                tutorialHandAnimator.SetBool("Extension", false);
            }
        }
        else if (InputManager.current.gUnlocked)
        {
            voiceString = "Pick Apple";
            TextController.Instance.textInstructions.text = "Extend your wrist to gather an apple";
            tutorialHandAnimator.SetBool("Extension", true);
            if (InputManager.current.wPressed)
            {
                timer = 0f;
                TextController.Instance.TextActive(false);
                firstVoice = true;
                tutorialHandAnimator.SetBool("Grip", false);            
            }
        }
        else if (InputManager.current.tUnlocked)
        {
            voiceString = "Feed Deer";
            TextController.Instance.textInstructions.text = "Flex your wrist to feed the deer";
            tutorialHandAnimator.SetBool("Flexion", true);
            if (InputManager.current.wPressed)
            {
                timer = 0f;
                TextController.Instance.TextActive(false);
                firstVoice = true;
                tutorialHandAnimator.SetBool("Flexion", false);
            }
        }
        else if (InputManager.current.aUnlocked || InputManager.current.dUnlocked)
        {
            TextController.Instance.textInstructions.text = "Rotate your wrist laterally to wave at the deer";
            tutorialHandAnimator.SetBool("RadialUlnarDeviation", true);
            if ((InputManager.current.aUnlocked && InputManager.current.aPressed) || (InputManager.current.dUnlocked && InputManager.current.dPressed))
            {
                timer = 0f;
                TextController.Instance.TextActive(false);
                firstVoice = true;
                tutorialHandAnimator.SetBool("RadialUlnarDeviation", false);
            }
        }


        if (InputManager.current.wUnlocked || InputManager.current.gUnlocked || InputManager.current.tUnlocked || InputManager.current.aUnlocked || InputManager.current.dUnlocked)
        {
            handModel.enabled = true;
            TextController.Instance.TextActive(true);
            
            if (firstVoice == true) { timerThreshold = 6f; }
            else { timerThreshold = 8f; }

            if (!TextController.Instance.voiceInstructions.isPlaying && timer >= timerThreshold)
            {
                timer = 0f;
                if (!InputManager.current.aUnlocked && !InputManager.current.dUnlocked)
                {
                    //TextController.Instance.PlayVoice(voiceString);
                }
                firstVoice = false;
            }
            else if (TextController.Instance.voiceInstructions.isPlaying)
            {
                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            tutorialHandAnimator.SetBool("Grip", false);
            tutorialHandAnimator.SetBool("Extension", false);
            tutorialHandAnimator.SetBool("Flexion", false);
            tutorialHandAnimator.SetBool("RadialUlnarDeviation", false);
            handModel.enabled = false;
        }

        
    }
}
