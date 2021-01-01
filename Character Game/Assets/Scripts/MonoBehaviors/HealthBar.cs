using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Reference to the same HitPoints scriptable object that the player prefab refers to
    // This container allows sharing of data between two objects
    public HitPoints hitPoints;

    // Reference to the current Player object to get maxHitPoints
    // Will be set programmatically, instead of through the Unity Editor, so it is hidden in the Inspector window
    [HideInInspector]
    public Player character;

    // reference to the health bar meter
    public Image meterImage;

    //reference to the text in the health bar
    public Text hpText;

    // reference for the max hit points
    float maxHitPoints;

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve and store max hit points for the characters
        maxHitPoints = character.maxHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (character != null)
        {
            meterImage.fillAmount = hitPoints.value / maxHitPoints;
            hpText.text = "HP:" + (meterImage.fillAmount * 100);
        }
    }
}
