using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : Singleton<TextController>
{
    public Text textInstructions;
    Image cloudBackdrop;

    public AudioSource voiceInstructions;


    public AudioClip helpDeer;
    public AudioClip attractAttention;
    public AudioClip rotateLeftRight;
    public AudioClip pickApple;
    public AudioClip feedDeer;
    public AudioClip moveForward;
    public AudioClip returnToPath;
    public AudioClip moveToTree;
    public AudioClip waveGoodbye;
    public AudioClip keepMovingGoodbye;


    bool v_Toggle;
    bool v_Play;

    // Start is called before the first frame update
    void Start()
    {
        textInstructions = GetComponentInChildren<Text>();
        cloudBackdrop = GetComponentInChildren<Image>();
        voiceInstructions = GetComponent<AudioSource>();
        TextActive(false);
        v_Play = v_Toggle = false;
    }

    public void TextActive(bool makeActive)
    {
        cloudBackdrop.enabled = makeActive;
        textInstructions.enabled = makeActive;
    }

    public void PlayVoice(string dofCue)
    {
        if (dofCue == "Move Forward")
        {
            voiceInstructions.clip = moveForward;
        }
        else if (dofCue == "moveToTree")
        {
            voiceInstructions.clip = moveToTree;
        }
        else if (dofCue == "moveToPath")
        {
            voiceInstructions.clip = returnToPath;
        }
        else if (dofCue == "Pick Apple")
        {
            voiceInstructions.clip = pickApple;
        }
        else if (dofCue == "Feed Deer")
        {
            voiceInstructions.clip = feedDeer;
        }
        else if (dofCue == "Attract Attention")
        {
            voiceInstructions.clip = attractAttention;
        }
        else if (dofCue == "Wave Goodbye")
        {
            voiceInstructions.clip = waveGoodbye;
        }
        else if (dofCue == "Help Deer")
        {
            voiceInstructions.clip = helpDeer;
        }
        voiceInstructions.Play();
    }
}
