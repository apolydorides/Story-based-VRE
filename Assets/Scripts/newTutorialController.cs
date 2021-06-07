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
            TextController.Instance.textInstructions.text = "Open and close your palm to move forward";
            if (InputManager.current.wPressed)
            {
                timer = 0f;
                TextController.Instance.TextActive(false);
                firstVoice = true;
            }
        }
        else if (InputManager.current.gUnlocked)
        {
            voiceString = "Pick Apple";
            TextController.Instance.textInstructions.text = "Open and close your palm to gather an apple";
            if (InputManager.current.wPressed)
            {
                timer = 0f;
                TextController.Instance.TextActive(false);
                firstVoice = true;
            }
        }
        else if (InputManager.current.tUnlocked)
        {
            voiceString = "Feed Deer";
            TextController.Instance.textInstructions.text = "Open and close your palm to feed the deer";
            if (InputManager.current.wPressed)
            {
                timer = 0f;
                TextController.Instance.TextActive(false);
                firstVoice = true;
            }
        }
        else if (InputManager.current.aUnlocked || InputManager.current.dUnlocked)
        {
            TextController.Instance.textInstructions.text = "Open and close your palm to wave";
            if ((InputManager.current.aUnlocked && InputManager.current.aPressed) || (InputManager.current.dUnlocked && InputManager.current.dPressed))
            {
                timer = 0f;
                TextController.Instance.TextActive(false);
                firstVoice = true;
            }
        }


        if (InputManager.current.wUnlocked || InputManager.current.gUnlocked || InputManager.current.tUnlocked || InputManager.current.aUnlocked || InputManager.current.dUnlocked)
        {
            handModel.enabled = true;
            tutorialHandAnimator.SetBool("Grip", true);
            TextController.Instance.TextActive(true);
            
            if (firstVoice == true) { timerThreshold = 6f; }
            else { timerThreshold = 8f; }

            if (!TextController.Instance.voiceInstructions.isPlaying && timer >= timerThreshold)
            {
                timer = 0f;
                if (!InputManager.current.aUnlocked && !InputManager.current.dUnlocked)
                {
                    TextController.Instance.PlayVoice(voiceString);
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
            handModel.enabled = false;
        }

        
    }
}
