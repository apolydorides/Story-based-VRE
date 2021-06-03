using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandController : MonoBehaviour
{
    protected Animator tutorialHandAnimator;
    public float timer;
    SkinnedMeshRenderer handModel;
    AudioSource voiceInstructions;

    // Start is called before the first frame update
    void Start()
    {
        handModel = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        handModel.enabled = false;

        tutorialHandAnimator = GetComponent<Animator>();
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // timer progression and reset
        if (InputManager.current.wUnlocked || InputManager.current.gUnlocked || InputManager.current.tUnlocked || InputManager.current.aUnlocked || InputManager.current.dUnlocked)
        {
            timer += Time.deltaTime;
        }
        else if (timer > 0f)
        {
            timer = 0f;
            tutorialHandAnimator.SetBool("Grip", false);
            tutorialHandAnimator.SetBool("Extension", false);
            tutorialHandAnimator.SetBool("Flexion", false);
            tutorialHandAnimator.SetBool("RadialUlnarDeviation", false);
            handModel.enabled = false;
            TextController.Instance.TextActive(false);
        }
        // timer reset on correct user input
        if (InputManager.current.wPressed && InputManager.current.wUnlocked)
        {
            timer = 0;
            tutorialHandAnimator.SetBool("Grip", false);
            handModel.enabled = false;
        }
        else if (InputManager.current.gPressed && InputManager.current.gUnlocked)
        {
            timer = 0;
            tutorialHandAnimator.SetBool("Extension", false);
            handModel.enabled = false;
        }
        else if (InputManager.current.tPressed && InputManager.current.tUnlocked)
        {
            timer = 0;
            tutorialHandAnimator.SetBool("Flexion", false);
            handModel.enabled = false;
        }
        else if ((InputManager.current.aPressed && InputManager.current.aUnlocked) || (InputManager.current.dPressed && InputManager.current.dUnlocked))
        {
            timer = 0;
            tutorialHandAnimator.SetBool("RadialUlnarDeviation", false);
            handModel.enabled = false;
        }

        // displaying appropriate tutorial hand animation if not already displayed
        if (timer >= 3 && handModel.enabled == false && !TextController.Instance.voiceInstructions.isPlaying)
        {
            handModel.enabled = true;
            if (InputManager.current.wUnlocked)
            {
                TextController.Instance.PlayVoice("Move Forward");
                tutorialHandAnimator.SetBool("Grip", true);
                TextController.Instance.textInstructions.text = "Open and close your palm to move forward";
            }
            else if (InputManager.current.gUnlocked)
            {
                tutorialHandAnimator.SetBool("Extension", true);
                TextController.Instance.PlayVoice("Pick Apple");
                TextController.Instance.textInstructions.text = "Open and close your palm to gather an apple";
            }
            else if (InputManager.current.tUnlocked)
            {
                tutorialHandAnimator.SetBool("Flexion", true);
                TextController.Instance.PlayVoice("Feed Deer");
                TextController.Instance.textInstructions.text = "Open and close your palm to feed the deer";
            }
            else if (InputManager.current.aUnlocked || InputManager.current.dUnlocked)
            {
                tutorialHandAnimator.SetBool("RadialUlnarDeviation", true);
                TextController.Instance.textInstructions.text = "Open and close your palm to wave";
            }
            TextController.Instance.TextActive(true);
        }
    }
}
