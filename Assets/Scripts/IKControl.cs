using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class IKControl : MonoBehaviour
{

    protected Animator animator;

    public bool ikActive = false;
    public Transform rightHandObj = null;
    public Transform lookObj = null;
    private bool targetObjectGrabbed = false;

    float state = 0f;
    float elapsedTime = 0f;
    // change this to slow down or speed up animation
    public float timeReaction = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rightHandObj = null;
        lookObj = null;
        animator = GetComponent<Animator>();
        state = 0f;
    }

    // A callback for calculating IK
    void OnAnimatorIK() 
    {        
        if (animator)
        {
            // if on idle stance and activation enough then initiate IK and start timer (for gradual motion)
            // but only if at correct position for event
            if (InputManager.current.gPressed && InputManager.current.gUnlocked && (state == 0f || state == 1f))
            {
                if (state == 1f)
                {
                    ikActive = false;
                    targetObjectGrabbed = true;
                    GameEvents.current.ItemGrab();
                }
                else if (state == 0f)
                {
                    ikActive = true;
                }
                rightHandObj = EventManager.Instance.eventGrabHandles[EventManager.Instance.completedTasks];
                lookObj = EventManager.Instance.eventLookObjects[EventManager.Instance.completedTasks];
            }

            // if the IK is active set the position and rotation directly to the goal
            if (ikActive)
            {
                // Set the right hand target position and rotation, if one has already been assigned
                // but do it gradually so that motion seems natural and not snappy
                if (rightHandObj != null)
                {
                    if (state < 1.0f)
                    {
                        // adding the time between each frame so that motion is not faster on higher-end systems achieving high frames per second
                        elapsedTime += Time.deltaTime;
                        state = Mathf.Lerp( 0, 1, elapsedTime / timeReaction);
                    }
                    else
                    {
                        state = 1.0f;
                        elapsedTime = 0f;
                    }

                    // Set the look target position, if one has been assigned
                    if (lookObj != null)
                    {
                        animator.SetLookAtWeight(state);
                        animator.SetLookAtPosition(lookObj.position);
                    }

                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, state);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, state);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                }

            }

            // else if the IK is not active, set the position and rotation of the head back to the original position
            else
            {
                if (state > 0f)
                {
                    elapsedTime += Time.deltaTime;
                    state = Mathf.Lerp( 0, 1, elapsedTime / timeReaction);
                    state = 1 - state;

                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, state);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, state);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                }
                else
                {
                    state = 0;
                    elapsedTime = 0f;

                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                    animator.SetLookAtWeight(0);

                    // if just arrived, flip boolean (dual purpose) and call that an event's task has been completed
                    if (targetObjectGrabbed)
                    {
                        targetObjectGrabbed = false;
                        EventManager.Instance.taskCompleted();
                    }
                    
                }

                
            }

        }        
    }
}
