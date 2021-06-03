using UnityEngine;
using PathCreation;
using System.Collections;
using UnityEngine.Animations.Rigging;

public class PlayerMotion : MonoBehaviour
{
    public PathCreator playerPath;
    public float speed
    {get; set;}
    public float distanceTravelled
    {get; set;}

    float timer = 0f;

    Rig playerRig;

    private void Start()
    {
        distanceTravelled = 0f;
        speed = 0f;
        playerRig = GameObject.FindGameObjectWithTag("Runtime Rig").GetComponent<Rig>();
    }
    
    private int caseSwitch = 0;
    private void Update()
    {
        // Case 1: motion along the path
        if (!EventManager.Instance.eventTransition && !EventManager.Instance.motionLocked) {caseSwitch = 1;}
        // Case 2: motion off the path to target positions
        else if (EventManager.Instance.eventTransition && !EventManager.Instance.motionLocked) {caseSwitch = 2;}
        // Case 3: activity occuring and no motion should take place
        else if (!EventManager.Instance.eventTransition && EventManager.Instance.motionLocked) {caseSwitch = 3;}
        // will throw log message
        else {caseSwitch = 0;}

        switch(caseSwitch)
        {
            case 1:
                if (!InputManager.current.wUnlocked)
                {
                    InputManager.current.wUnlocked = true;
                }

                if (InputManager.current.wPressed)
                {
                    Debug.Log("W Key Pressed!");
                    speed = 2f;
                }
                else if (speed > 0f)
                {
                    speed -= 0.95f * Time.deltaTime;
                    if (speed < 0.1f)
                        speed = 0;
                }
                break;
            case 2:
                if (!InputManager.current.wUnlocked)
                {
                    InputManager.current.wUnlocked = true;
                }

                if (playerRig.weight != 0)
                {
                    playerRig.weight = 0;
                }

                if (InputManager.current.wPressed)
                {
                    Debug.Log("W Key Pressed!");
                    speed = 2f;
                }
                else if (speed > 0f)
                {
                    speed -= 0.95f * Time.deltaTime;
                    if (speed < 0.1f)
                        speed = 0;
                }
                break;
            case 3:
                if (InputManager.current.wUnlocked)
                {
                    InputManager.current.wUnlocked = false;
                }

                speed = 0f;
                if (playerRig.weight != 1 && EventManager.Instance.activityOccuring)
                {
                    playerRig.weight = 1;
                }
                break;
            default:
                Debug.Log("Motion error: @PlayerMotion.cs not properly detecting motion cases");
                break;
        }
    }

    private void FixedUpdate()
    {
        switch(caseSwitch)
        {
            case 1:
                distanceTravelled += speed * Time.deltaTime;
                transform.position = playerPath.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
                transform.rotation = playerPath.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
                break;
            case 2:
                if (Vector3.Distance(transform.position, EventManager.Instance.playerTargetPosition.position) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, EventManager.Instance.playerTargetPosition.position, speed*Time.deltaTime);
                    transform.rotation = Quaternion.Slerp(transform.rotation, EventManager.Instance.playerTargetPosition.rotation, speed * Time.deltaTime / 2);
                }
                else
                {
                    transform.position = EventManager.Instance.playerTargetPosition.position;
                    transform.rotation = EventManager.Instance.playerTargetPosition.rotation;
                    EventManager.Instance.playerReachedTargetPosition();
                }
                break;
            case 3:
                break;
            default:
                break;
        }
    }

}
