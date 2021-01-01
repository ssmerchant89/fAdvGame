using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float movementSpeed = 1.5f;

    Vector2 movement = new Vector2();

    //Holds reference to the animator component in the game object
    Animator animator;

    string animationState = "AnimationState";

    Rigidbody2D rb2D;


    //enumerated constants to correspond to the values assigned to the animations
    enum CharStates
    {
        walkRight = 1,
        walkLeft = 2,
        idle = 3
    }

    // Start is called before the first frame update
    void Start()
    {
        // get component so it doesnt have to be called over and over
        rb2D = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // update the animation state machine
        UpdateState();
    }
    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void UpdateState()
    {
        if (movement.x > 0)
        {
            animator.SetInteger(animationState, (int)CharStates.walkRight);
        }
        else if (movement.x < 0)
        {
            animator.SetInteger(animationState, (int)CharStates.walkLeft);
        }
        else if (movement.y > 0)
        {
            animator.SetInteger(animationState, (int)CharStates.walkRight);
        }
        else if (movement.y < 0)
        {
            animator.SetInteger(animationState, (int)CharStates.walkLeft);
        }
        else
            animator.SetInteger(animationState, (int)CharStates.idle);
    }

    private void MoveCharacter()
    {
        // get user input
        // GetAxisRaw parameter allows us to specify which axis we're interested in
        // Returns 1 = right key or "d"
        //        -1 = left key or "a"
        //         0 = no key pressed
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // keeps playing moving at the same rate of speed, no matter which direction they are moving in
        movement.Normalize();

        rb2D.velocity = movement * movementSpeed;
    }

}