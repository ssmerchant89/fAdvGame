using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // A reference to the slot prefab object; attached in the unity editor
    public GameObject slotPrefab;

    // Number of slots that the inventory bar contains
    public const int numSlots = 1;

    // An array to hold the image components
    Image[] itemImages = new Image[numSlots];

    // Holds references to the actual item (scriptable object) that the player picked up
    Item[] items = new Item[numSlots];

    // Holds references to slot prefabs
    GameObject[] slots = new GameObject[numSlots];

    // Start is called before the first frame update
    void Start()
    {
        CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Initialize all of the slots
    public void CreateSlots()
    {
        // make sure that the slot prefab has been set in the unity editor
        if (slotPrefab != null)
        {
            for (int i = 0; i < numSlots; i++)
            {
                // Create a new Slot game object and give it a name
                GameObject newSlot = Instantiate(slotPrefab);
                newSlot.name = "ItemSlot_" + i;

                // This script will be attached to InventoryObject, so gameObject == InventoryObject
                // InventoryObject has one child, so GetChild(0) == Inventory
                // The newSlot's parent will be Inventory so hierarchy is: InventoryObject --> Inventory --> newSlot
                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);

                slots[i] = newSlot;

                // Gets a reference to the ItemImage component in the Slot object
                // Slot --> Background is index (0) and Slot --> ItemImage is index (1)
                itemImages[i] = newSlot.transform.GetChild(1).GetComponent<Image>();
            }
        }
    }

    /**
     * Adds an item to inventory
     * itemToAdd: the item to be added to the inventory
     * return: true if the item was successfully added
     */
    public bool AddItem(Item itemToAdd)
    {
        for (int i = 0; i < items.Length; i++)
        {
            // check to see if the current item in the index, if one exists, is of the same type the player
            // wants to add, and see if it is a stackable item
            if (items[i] != null && items[i].itemType == itemToAdd.itemType && itemToAdd.stackable == true)
            {
                // Adding to existing slot

                // Increase the quantity since it is a stackable item
                items[i].quantity = items[i].quantity + 1;

                // Grab a reference to the script that's in the slot (which contains a reference to the QtyText)
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                Text quantityText = slotScript.qtyText;

                // Enable the text and set the text to be the quantity of stackable items
                quantityText.enabled = true;
                quantityText.text = items[i].quantity.ToString();

                return true;
            }

            if (items[i] == null)
            {
                // Adding to empty slot
                // This is the first item added to the array of a particular itemType, or the item isn't stackable

                // Copy item & add to inventory.  Copying so we don't change original scriptable object
                items[i] = Instantiate(itemToAdd);
                items[i].quantity = 1;
                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;

                return true;
            }
        }

        // Inventory is full; cannot add new item
        return false;
    }
}
