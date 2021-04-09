using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleController : MonoBehaviour
{
    Transform rightHand;
    Transform feedingTarget;
    Transform fawnMouth;
    public bool reachedFor = false;
    protected bool trackingHand = false;
    bool collected = false;
    bool trackingMouth = false;
    // tracks stage of event to handle trigger collisions - see OnTriggerEnter for how it works
    public int stage = 0;

    // Start is called before the first frame update
    private void Start()
    {
        GameEvents.current.onItemGrab += OnAppleGrabbed;
        GameEvents.current.onItemComplete += OnGathered;
        GameEvents.current.onAppleEaten += OnEaten;
        rightHand = GameObject.FindGameObjectWithTag("Right Hand Slot").transform;
        feedingTarget = GameObject.FindGameObjectWithTag("Feeding Target").transform;
        fawnMouth = GameObject.FindGameObjectWithTag("Fawn Mouth").transform;
    }

    private void Update()
    {
        if (InputManager.current.tPressed && InputManager.current.tUnlocked && collected)
        {
            InputManager.current.tUnlocked = false;
            StartCoroutine(AppleThrown(feedingTarget));
            Debug.Log("T pressed and coroutine should have started!");
        }
        else if (InputManager.current.tUnlocked && collected)
        {
            transform.position = rightHand.position;
        }
    }

    private void FixedUpdate()
    {
        if (trackingHand && reachedFor)
        {
            Debug.Log("Apple is tracking the hand!");
            gameObject.transform.SetPositionAndRotation(rightHand.position, rightHand.transform.rotation);
        }
        // else if (trackingMouth)
        // {
        //     Debug.Log("Apple is tracking the mouth!");
        //     gameObject.transform.SetPositionAndRotation(fawnMouth.position, fawnMouth.rotation);
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (stage == 0 && other.gameObject.CompareTag("Right Hand"))
        {
            // even though we have bool trackingHand that will actually trigger on all apples since they are subscribed to that event
            reachedFor = true;
            stage++;
            print("collided with right hand!");
        }
        else if (stage == 1 && other.gameObject.CompareTag("Fawn Mouth"))
        {
            // trackingMouth = true;
            stage++;
            Debug.Log("collided with the fawn's mouth");
            // on next animation end eating and destroy apple
        }
    }

    private void OnAppleGrabbed()
    {
        // Collider will trigger before state of hand is 1 (maximum reach to match transforms)
        // and collider also detects for each apple separately
        if (reachedFor)
        {
            // so we only start tracking a hand if reach is maximum and its collider has been triggered
            print("OnAppleGrab() called.");
            trackingHand = true;
        }
    }

    private void OnGathered()
    {
        if (reachedFor && trackingHand)
        {
            reachedFor = false;
            trackingHand = false;
            gameObject.SetActive(false);
            Collider thisCollider = gameObject.GetComponent<Collider>();
            thisCollider.isTrigger = true;
            collected = true;
        }
    }

    IEnumerator AppleThrown(Transform target)
    {
        float xDifference = Mathf.Abs(target.position.x - transform.position.x);
        float zDifference = Mathf.Abs(target.position.z - transform.position.z);
        float horizontalDifference = Mathf.Sqrt(Mathf.Pow( xDifference, 2) + Mathf.Pow( zDifference, 2));
        float yDifference = Mathf.Abs(target.position.y - transform.position.y);
        float xzRatio = (xDifference / zDifference);
        float cumulativeHorizontal = 0f;
        Vector3 inAirPos = transform.position;
        while (transform.position.y - target.position.y > 0.05f)
        {
            float zIncrement = Mathf.Sqrt(Mathf.Pow(6f * Time.deltaTime, 2) / (Mathf.Pow(xzRatio, 2) + 1));
            float xIncrement = zIncrement * xzRatio;
            float hIncrement = Mathf.Sqrt(Mathf.Pow( xIncrement, 2) + Mathf.Pow( zIncrement, 2));
            cumulativeHorizontal += hIncrement;
            float newY = Mathf.Sqrt( ( ( horizontalDifference - cumulativeHorizontal ) * Mathf.Pow(yDifference, 2) ) / (horizontalDifference) ); 
            Debug.Log("Inside while loop of AppleThrown coroutine...");
            if (!(newY >= 0))
            {
                newY = 0;
            }
            inAirPos.Set(transform.position.x + xIncrement, target.position.y + newY, transform.position.z + zIncrement);
            transform.position = inAirPos;
            if (transform.position.y - target.position.y < 0.05f)
            {
                Debug.Log("Vertical offset is: " + (transform.position.y - target.position.y).ToString());
            }
            yield return null;
        }
        transform.position = target.position;
        print("Apple reached the target position.");
        GameObject Fawn = GameObject.FindGameObjectWithTag("Fawn");
        Fawn.GetComponent<Animator>().SetBool("eating", true);
        trackingMouth = true;
    }

    // called from GameEvents.cs action, results from OnStateExit() of EatingBehavior.cs
    private void OnEaten()
    {
        if (trackingMouth)
        {
            trackingMouth = false;
            EventManager.Instance.taskCompleted();
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.onItemGrab -= OnAppleGrabbed;
        GameEvents.current.onItemComplete -= OnGathered;
    }
}