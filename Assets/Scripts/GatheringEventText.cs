using UnityEngine;
using UnityEngine.UI;

public class GatheringEventText : Singleton<GatheringEventText>
{
    public GameObject GatheringInstructions;
    public Text Instructions;

    // Start is called before the first frame update
    void Start()
    {
        GatheringInstructions = GameObject.FindWithTag("Gathering Instructions");
        GatheringInstructions.SetActive(false);
        Instructions = GatheringInstructions.GetComponent<Text>();
    }

}
