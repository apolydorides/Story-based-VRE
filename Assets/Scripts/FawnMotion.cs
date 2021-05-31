using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class FawnMotion : MonoBehaviour
{
    Animator animator;
    public PathCreator fawnPath;
    public float speed;
    float playerSpeed;
    public float distanceTravelled
    {get; set;}
    // Fawn's speed needs a reference to the player's speed\
    // when it needs to follow it should follow at the same speed
    GameObject ThePlayer;
    // access motion script on player
    PlayerMotion playerMotion;
    // for when fawn speed does not track the player's
    public bool DecoupleSpeed
    { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        // this will allows control of the fawn's animations
        animator = GetComponent<Animator>();
        // accessing the speed of the main character
        ThePlayer = GameObject.FindGameObjectWithTag("Main Character");
        playerMotion = ThePlayer.GetComponent<PlayerMotion>();
        playerSpeed = playerMotion.speed;
        DecoupleSpeed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!DecoupleSpeed) // normal path motion
        {
            // accessing the speed of the player  within the update function as it refreshes faster than Fixed Update
            playerSpeed = playerMotion.speed;
            speed = playerSpeed;
        }
        
        animator.SetFloat("velocity", speed);
    }

    private void FixedUpdate() 
    {
        // performing movement changes within FixedUpdate for smoother motion (matches output frame rate)
        // only when speed is coupled to player's does the fawn move along the path
        if (!DecoupleSpeed)
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = fawnPath.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = fawnPath.path.GetRotationAtDistance(distanceTravelled);
        }
    }
}
