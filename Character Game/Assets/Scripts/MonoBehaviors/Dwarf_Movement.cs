using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Dwarf_Movement : MonoBehaviour
{
    //Speed at which the enemy persues the player
    public float pursuitSpeed;

    //General wandering speed
    public float wanderSpeed;

    //How often the enemy should change wandering directions
    public float directionChangeInterval;

    //curent speed
    float currentSpeed;

    public bool followPlayer;

    Coroutine moveCoroutine;
    //Components attached to the game object
    Rigidbody2D rb2d;
    Animator animator;

    //The player's transform
    Transform targetTransform = null;

    Vector3 endPosition;

    //Angle is used to generate a vector which becomes the destination
    float currentAngle = 0;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        currentSpeed = wanderSpeed;
        StartCoroutine(WanderRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        // Exits the game if the user pushes the escape key.
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

    }

    public IEnumerator WanderRoutine()
    {
        while (true)
        {
            ChooseNewEndpoint();

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));

            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    private void ChooseNewEndpoint()
    {
        //Choose a random angle between 0 and 360 for new direction to travel
        currentAngle += UnityEngine.Random.Range(0, 360);

        //Keeps currentAngle between 0 and 360
        currentAngle = Mathf.Repeat(currentAngle, 360);

        //Convert angle to a vector3 and add result to end position
        endPosition += Vector3FromAngle(currentAngle);
    }


    //Takes an angle in degrees, converts it to radians, and returns a direction vector
    private Vector3 Vector3FromAngle(float inputAngleDegrees)
    {


        //Convert angle degrees to radians
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;

        //Create a normalized directional vector for the enemy direction
        return new Vector3(Mathf.Cos(inputAngleRadians), Mathf.Sin(inputAngleRadians), 0);

    }

    private IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {

        //Retrieve the rough distance remaining between the current enemey position and the destination
        // Magnitude is a unity function to return the length of the vector
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;

        while (remainingDistance > float.Epsilon)
        {
            if (targetTransform != null)
            {

                // If targetTransform is set, then its position is the players position
                //This moves the enemy toward the player instead of the original endPosition
                endPosition = targetTransform.position;
            }


            if (rigidBodyToMove != null)
            {

                //Calculates the movement for a RigidBody2D
                //To make sure the object speed is independent of frame rate, multiply the speed by Time.deltaTime
                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed * Time.deltaTime);

                //Move the RigidBody2D
                rb2d.MovePosition(newPosition);

                //Update the distance remaining
                remainingDistance = (transform.position - endPosition).sqrMagnitude;

            }


            //Pause execution until the next fixed frame update
            yield return new WaitForFixedUpdate();

        }



    }

    //Called when player enters the circle collider for the enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {

        //See if the object that the enemy has collided with is the player and
        //that the enemy is supposed to be following the player
        if (collision.gameObject.CompareTag("Player") && followPlayer)
        {
            //Update the current speed
            currentSpeed = pursuitSpeed;
            animator.SetBool("dwarfAttack", true);
            //Set the targetTransform to be the players
            targetTransform = collision.gameObject.transform;
        }

        //If enemy is moving stop it
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        //Start the move routine with the updated info
        // i.e to follow the player at new speed
        moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //See if the object that the enemy is no longer colliding with is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("dwarfAttack", false);
            //slow the speed down
            currentSpeed = wanderSpeed;

            //If enemy is moving, stop it
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            //Set to null since enemy is no longer following player
            targetTransform = null;
        }

    }
}
