using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandController : MonoBehaviour
{
    protected Animator tutorialHandAnimator;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        tutorialHandAnimator = GetComponent<Animator>();
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.current.wUnlocked || InputManager.current.gUnlocked || InputManager.current.tUnlocked)
        {
            timer += Time.deltaTime;
        }
        else if (timer > 0f)
        {
            timer = 0f;
        }

        if (InputManager.current.wPressed && InputManager.current.wUnlocked)
        {
            timer = 0;
        }
        else if (InputManager.current.gPressed && InputManager.current.gUnlocked)
        {
            timer = 0;
        }
        else if (InputManager.current.tPressed && InputManager.current.tUnlocked)
        {
            timer = 0;
        }
    }
}
