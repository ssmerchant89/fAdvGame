using System.Collections;
using UnityEngine;

// Make the class abstract as it will need to be inherited by a subclass
public abstract class Character : MonoBehaviour
{
    // Properties common to all characters
    public float maxHitPoints;
    public float startingHitPoints;

    public enum CharacterCategory
    {
        PLAYER,
        ENEMY
    }

    public CharacterCategory characterCategory;

    public virtual void KillCharacter()
    {
        //Destroys the current game objects and removes it from the scene.
        Destroy(gameObject);
    }

    // Flickers character color
    public virtual IEnumerator FlickerCharacter(float interval)
    {
        int i = (int)(interval / .2f);
        do
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.1f);
            i--;
        } while (i > 0);
    }

    // Changes layer to prevent collision temporarily
    public virtual IEnumerator IFrames(float interval)
    {
        gameObject.layer = 12; // iFrame
        yield return new WaitForSeconds(interval);
        gameObject.layer = 8; // Blocking
    }

    // Set the character back to it's original state
    public abstract void ResetCharacter();

    //Coroutine to inflict an amount of damage to the character over a period of time
    // interval = 0 to inflict a one-time damage hit
    // interval > 0 to continuously inflict damage at the set interval of time
    public abstract IEnumerator DamageCharacter(int damage, float interval);
}

