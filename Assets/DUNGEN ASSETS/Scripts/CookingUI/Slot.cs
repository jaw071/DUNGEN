using UnityEngine;
using UnityEngine.UI;
using MoreMountains.InventoryEngine;

public class Slot : MonoBehaviour
{
    public InventoryItem item; // Holds the ingredient
    public int index;          // Used by CookingManager to identify slot position
}