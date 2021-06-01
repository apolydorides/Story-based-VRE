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
    public AudioClip extension;
    public AudioClip flexion;
    public AudioClip moveForward;
    public AudioClip returnToPath;
    public AudioClip moveToTree;
    public AudioClip rotateGoodbye;
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
        if (dofCue == "moveForward")
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
        else if (dofCue == "extension")
        {
            voiceInstructions.clip = extension;
        }
        else if (dofCue == "flexion")
        {
            voiceInstructions.clip = flexion;
        }
        else if (dofCue == "attractAttention")
        {
            voiceInstructions.clip = moveForward;
        }
        else if (dofCue == "waveGoodbye")
        {
            voiceInstructions.clip = moveForward;
        }
    }
}
