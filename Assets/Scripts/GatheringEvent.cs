using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class GatheringEvent : MonoBehaviour
{
    Transform Fawn;
    PathCreator fawnPath;
    FawnMotion fawnMotion;
    Transform Player;
    PathCreator playerPath;
    PlayerMotion playerMotion;
    Transform fawnMouth;

    // used for finding the closest return point on the path
    private float closestDistance;
    // variables for event sequence control
    int eventStep = 0;
    bool justUpdated = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Fawn = GameObject.FindGameObjectWithTag("Fawn").transform;
        fawnMotion = Fawn.GetComponent<FawnMotion>();
        fawnPath = fawnMotion.fawnPath;
        Player = GameObject.FindGameObjectWithTag("Main Character").transform;
        playerMotion = Player.GetComponent<PlayerMotion>();
        playerPath = playerMotion.playerPath;

        GameEvents.current.onGatheringEnter += EventStart;
        GameEvents.current.onActivityExit += ActivityDone;
        GameEvents.current.onFeedingExit += EventFinished;
    }

    void Update()
    {
        if (justUpdated)
        {
            switch(eventStep)
            {
                case 1: // deer gets distracted and moves off the path
                    StartCoroutine(FawnApproachTarget(EventManager.Instance.fawnTargetPosition));
                    EventManager.Instance.motionLocked = true;
                    TextController.Instance.TextActive(true);
                    TextController.Instance.textInstructions.text = "The deer appears to be moving out of the trail!";
                    break;
                case 2: // player looks at the deer
                    TextController.Instance.TextActive(false);
                    StartCoroutine(PlayerLookTowardsTarget(Fawn));
                    break;
                case 3: // player looks at the apple tree
                    TextController.Instance.TextActive(true);
                    TextController.Instance.textInstructions.text = "Maybe some apples will gain its attention!";
                    StartCoroutine(PlayerLookTowardsTarget(EventManager.Instance.playerTargetPosition));
                    break;
                case 4: // player prompted to move towards apple tree
                    Debug.Log("Case 4");
                    playerMotion.speed = 0f;
                    TextController.Instance.TextActive(false);
                    EventManager.Instance.eventTransition = true;
                    EventManager.Instance.motionLocked = false;
                    break;
                case 5:  // player completed activity and looks back at deer
                    Debug.Log("Case 5");
                    // also setting up next player and fawn target positions
                    closestDistance = playerPath.path.GetClosestDistanceAlongPath(Player.position);
                    playerMotion.distanceTravelled = closestDistance;
                    EventManager.Instance.playerTargetPosition.position = playerPath.path.GetPointAtDistance(closestDistance);
                    EventManager.Instance.playerTargetPosition.rotation = playerPath.path.GetRotationAtDistance(closestDistance);
                    StartCoroutine(PlayerLookTowardsTarget(Fawn));
                    break;
                case 6:  // deer returns to trail and looks at player
                    Debug.Log("Case 6");
                    closestDistance = fawnPath.path.GetClosestDistanceAlongPath(Fawn.position);
                    fawnMotion.distanceTravelled = closestDistance;
                    EventManager.Instance.fawnTargetPosition.position = fawnPath.path.GetPointAtDistance(closestDistance);
                    EventManager.Instance.fawnTargetPosition.rotation = Quaternion.LookRotation(Player.position - Fawn.position, Vector3.up);;
                    StartCoroutine(FawnApproachTarget(EventManager.Instance.fawnTargetPosition));
                    TextController.Instance.TextActive(true);
                    TextController.Instance.textInstructions.text = "The deer seems to be interested in the apples you are holding!";
                    break;
                case 7:  // activity: player looks at fawn and prompted to feed it the apples
                    Debug.Log("Case 7");
                    StartCoroutine(PlayerLookTowardsTarget(Fawn));
                    TextController.Instance.textInstructions.text = "Try giving it some apples!";
                    break;
                case 8:  // player allowed to feed the deer
                    Debug.Log("Case 8");
                    EventManager.Instance.eventLookObjects[0].gameObject.SetActive(true);
                    InputManager.current.tUnlocked = true;
                    break;
                case 9: // player allowed to return to path (Target position set in case 5)
                    Debug.Log("Case 9");
                    playerMotion.speed = 0f;
                    TextController.Instance.TextActive(false);
                    EventManager.Instance.eventTransition = true;
                    EventManager.Instance.motionLocked = false;
                    break;
                default:
                    break;
            }
            justUpdated = false;
            eventStep += 1;
        }
    }

    void EventStart()
    {
        fawnMotion.DecoupleSpeed = true;
        eventStep = 1;
        justUpdated = true;
    }

    void EventFinished()
    {
        fawnMotion.DecoupleSpeed = false;
        eventStep = 0;
        justUpdated = false;
    }

    // handles non-coroutine events, e.g. 
    void ActivityDone()
    {
        justUpdated = true;
    }

    
    IEnumerator FawnApproachTarget(Transform target)
    {
        // store whole Distance to then rotate at the same rate
        float wholeDistance = Vector3.Distance(Fawn.position, target.position);
        float wholeAngle = Quaternion.Angle(Fawn.rotation, target.rotation);
        float slerpTimer = 0f;
        while (Vector3.Distance(Fawn.position, target.position) > 0.1f)
        {
            slerpTimer += 0.5f * Time.deltaTime;
            float currentDistance = Vector3.Distance(Fawn.position, target.position);
            // handles fawn's speed for animation purposes
            if (currentDistance > 2f)
            { fawnMotion.speed = 2f; }
            else if (currentDistance > 0.5f)
            { fawnMotion.speed = currentDistance; }
            else
            { fawnMotion.speed = 0f; }
            Fawn.position = Vector3.Lerp(Fawn.position, target.position, 0.5f * Time.deltaTime);
            Fawn.rotation = Quaternion.Slerp(Fawn.rotation, target.rotation, 0.05f * slerpTimer);
            yield return null;
        }
        fawnMotion.speed = 0f;
        Fawn.position = target.position;
        Fawn.rotation = target.rotation;
        print("Fawn reached the target position.");
        justUpdated = true;
    }

    IEnumerator FawnLookTowardsTarget(Transform target)
    {
        Vector3 relativePos = target.position - Fawn.position;
        Quaternion lookRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        float slerpTimer = 0f;
        while (Quaternion.Angle(Fawn.rotation, lookRotation) > 2f)
        {
            slerpTimer += Time.deltaTime;
            Fawn.rotation = Quaternion.Slerp(Fawn.rotation, lookRotation, 0.5f * slerpTimer);
            yield return new WaitForSeconds(0.02f);
        }
        Fawn.rotation.SetLookRotation(relativePos, Vector3.up);

        
        print("Fawn looking at target.");
        yield return new WaitForSeconds(1f);
        justUpdated = true;
    }

    IEnumerator PlayerLookTowardsTarget(Transform target)
    {
        Vector3 relativePos = target.position - Player.position;
        Quaternion lookRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        float slerpTimer = 0f;
        while (Quaternion.Angle(Player.rotation, lookRotation) > 2f)
        {
            slerpTimer += Time.deltaTime;
            Player.rotation = Quaternion.Slerp(Player.rotation, lookRotation, 0.5f * slerpTimer);

            yield return new WaitForSeconds(0.02f);
        }
        Player.rotation.SetLookRotation(relativePos, Vector3.up);

        print("Player looking at target.");
        yield return new WaitForSeconds(1f);
        slerpTimer = 0f;
        justUpdated = true;
    }
}
