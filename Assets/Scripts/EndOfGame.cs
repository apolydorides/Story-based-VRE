using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndOfGame : MonoBehaviour
{
    public Image endScreen;
    public TMP_Text thankYou;
    public Transform tutorialHandDisplay;

    void Start()
    {
        endScreen.color = new Color(0.08877714f, 0.2022645f, 0.3301887f, 0f);
        thankYou.color = new Color(1f, 1f, 1f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // removing this collider's trigger feature to avoid hitting it again on exit
        Collider thisCollider = gameObject.GetComponent<Collider>();
        thisCollider.isTrigger = false;
        EventManager.Instance.motionLocked = true; // will go into case 3 of playermotion script (i.e. player stops)
        StartCoroutine("FadeToBlack");
        tutorialHandDisplay.gameObject.SetActive(false);
    }
    
    IEnumerator FadeToBlack()
    {
        float opacity = 0f;
        while (opacity < 0.99f)
        {
            opacity += Time.deltaTime/2f;
            endScreen.color = new Color(0.08877714f, 0.2022645f, 0.3301887f, opacity);
            thankYou.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForEndOfFrame();
        }
        endScreen.color = new Color(0.08877714f, 0.2022645f, 0.3301887f, 1f);
        thankYou.color = new Color(1f, 1f, 1f, 1f);

        yield return null;
    }
}
