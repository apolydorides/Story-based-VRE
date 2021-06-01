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
            TextController.Instance.TextActive(false);
        }
        else if (InputManager.current.gPressed && InputManager.current.gUnlocked)
        {
            timer = 0;
            tutorialHandAnimator.SetBool("Extension", false);
            handModel.enabled = false;
            TextController.Instance.TextActive(false);
        }
        else if (InputManager.current.tPressed && InputManager.current.tUnlocked)
        {
            timer = 0;
            tutorialHandAnimator.SetBool("Flexion", false);
            handModel.enabled = false;
            TextController.Instance.TextActive(false);
        }
        else if ((InputManager.current.aPressed && InputManager.current.aUnlocked) || (InputManager.current.dPressed && InputManager.current.dUnlocked))
        {
            timer = 0;
            tutorialHandAnimator.SetBool("RadialUlnarDeviation", false);
            handModel.enabled = false;
            TextController.Instance.TextActive(false);
        }

        // displaying appropriate tutorial hand animation if not already displayed
        if (timer >= 3 && handModel.enabled == false)
        {
            handModel.enabled = true;
            if (InputManager.current.wUnlocked)
            {
                tutorialHandAnimator.SetBool("Grip", true);
                TextController.Instance.textInstructions.text = "Open and close your palm to move forward";
            }
            else if (InputManager.current.gUnlocked)
            {
                tutorialHandAnimator.SetBool("Extension", true);
                TextController.Instance.textInstructions.text = "Extend your hand upwards to move gather an apple";
            }
            else if (InputManager.current.tUnlocked)
            {
                tutorialHandAnimator.SetBool("Flexion", true);
                TextController.Instance.textInstructions.text = "Flex your hand down to feed the deer";
            }
            else if (InputManager.current.aUnlocked || InputManager.current.dUnlocked)
            {
                tutorialHandAnimator.SetBool("RadialUlnarDeviation", true);
                TextController.Instance.textInstructions.text = "Rotate your wrist laterally to wave";
            }
            TextController.Instance.TextActive(true);
        }
    }
}
