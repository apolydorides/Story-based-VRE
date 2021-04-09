using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    protected EventManager() {}

    // Explanation of eventTransition, motionLocked and activityCanOccur booleans
    // upon triggering event eventTransition -> true and smooth transition to event spot
    // upon arrival to event spot motionLocked + activityCanOccur -> true and when event ends activityCanOccur -> false 
    // after smooth return to saved position (on event trigger) motionLocked->false
    public bool motionLocked = false;
    public bool eventTransition = false;
    public bool activityOccuring = false;
    
    public Transform playerTargetPosition
    {get; set;}
    // set this to 1 or 2 in Event scripts depending on whether:
    // player reached event target ( 1 )
    // or player returned back to path ( 2 )
    private bool targetWasEvent = true;
    public void playerReachedTargetPosition()
    {
        eventTransition = false;
        if (targetWasEvent)
        {
            activityOccuring = true;
            motionLocked = true;
            InputManager.current.gUnlocked = true;
        }
        else 
        {
            GameEvents.current.FeedingExit();
        }
        targetWasEvent = !targetWasEvent;
        
    }
    public Transform fawnTargetPosition
    {get; set;}

    public List<Transform> eventLookObjects;
    public List<Transform> eventGrabHandles;
    public void setObjectives(List<Transform> grabHandles, List<Transform> lookObjects, bool repeated)
    {
        eventGrabHandles = grabHandles.ToList();
        eventLookObjects = lookObjects.ToList();
        completedTasks = 0;
        if (repeated)
        {
            totalTasks = eventGrabHandles.Count*2;
        }
        else
        {
            totalTasks = eventGrabHandles.Count;
        }
    }

    public int totalTasks = 0;
    public int completedTasks = 0;
    // Each activity/event has its own tasks (e.g. apple picking activity that requires picking 5 apples is considered to have 5 tasks)
    public void taskCompleted()
    {
        Debug.Log("Task Completed!");
        completedTasks += 1;
        GameEvents.current.ItemComplete();
        if (completedTasks == totalTasks)
        {
            Debug.Log("Calling activity completed function!");
            activityCompleted();
        }
        else if ( completedTasks*2 == totalTasks )
        {
            InputManager.current.gUnlocked = false;
            GameEvents.current.ActivityExit();
        }
        // e.g. after every completed task over half and less than maximum totalTasks reset tUnlocked
        else if ( completedTasks > totalTasks/2)
        {
            eventLookObjects[completedTasks-(totalTasks/2)].gameObject.SetActive(true);
            InputManager.current.tUnlocked = true;
        }
    }

    public void activityCompleted()
    {
        activityOccuring = false;
        eventGrabHandles.Clear();
        eventLookObjects.Clear();
        InputManager.current.gUnlocked = false;
        InputManager.current.tUnlocked = false;
        GameEvents.current.ActivityExit();
    }
}
