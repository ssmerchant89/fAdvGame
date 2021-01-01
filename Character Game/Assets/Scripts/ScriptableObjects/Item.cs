using UnityEngine;

// Create an entry in the "Create submenu" to be able to easily create instances of the object
[CreateAssetMenu(menuName = "Item")]

// Make sure to inherit from the ScriptableObject class
public class Item : ScriptableObject
{
    // Used for debugging, or possibly displaying the name of an item
    public string objectName;

    // Reference to the item's sprite, so it can be displayed
    public Sprite sprite;

    // Quantity of the item
    public int quantity;

    // True = multiple copies of an item can be interacted with simultaneously (keys)
    public bool stackable;

    public enum ItemType
    {
        TREASURE,
        HEALTH,
        KEY
    }

    // Used to identify the item in game logic
    public ItemType itemType;
}
