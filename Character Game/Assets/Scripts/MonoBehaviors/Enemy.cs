using UnityEngine;
using System.Collections;

public class Enemy : Character
{
    float hitPoints;

    // Amount of damage the enemy will inflict when it runs into the player
    public int damageStrength;

    // Ref. to a runnning coroutine
    Coroutine damageCoroutine;

    private void OnEnable()
    {
        ResetCharacter();
    }

    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
    }

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        // Continously inflict damage until loop breaks
        while (true)
        {
            // Inflict damage
            hitPoints = hitPoints - damage;

            // Player is dead, kill game object and exit loop
            if (hitPoints <= 0)
            {
                KillCharacter();
                break;
            }

            if (interval > 0)
            {
                // Wait a specified amount of seconds & inflict more damage
                yield return new WaitForSeconds(interval);
            }
            else
            {
                // Interval = 0; inflict one-time damage and exit loop
                break;
            }
        }
    }

    // Called by the Unity engine when the current enemy object's Collider2D makes contact with another object's collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // See if the enemy has collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get a reference to the colliding player object
            Player player = collision.gameObject.GetComponent<Player>();

            // If coroutine is not currently executing
            if (damageCoroutine == null)
            {
                // Start the coroutine to inflict damage to the player every 1 second
                damageCoroutine = StartCoroutine(player.DamageCharacter(damageStrength, 1.0f));
            }
        }

    }

    // Called by the Unity engine when the current enemy object stops touchign another object's collider
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // If coroutine is currently executing
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }
}
