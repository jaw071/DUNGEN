using UnityEngine;
using MoreMountains.InventoryEngine;

public class PersistentInventoryLoader : MonoBehaviour
{
    public Inventory inventoryPrefab;

    private void Awake()
    {
        // Check if an inventory already exists
        if (Inventory.GlobalInstance != null)
        {
            Destroy(this.gameObject); // Loader not needed anymore
            return;
        }

        if (inventoryPrefab != null)
        {
            Inventory inventoryInstance = Instantiate(inventoryPrefab);
            inventoryInstance.name = "PersistentInventory"; // optional for debugging
            DontDestroyOnLoad(inventoryInstance.gameObject);
        }
        else
        {
            Debug.LogError("No inventory prefab assigned to PersistentInventoryLoader.");
        }

        DontDestroyOnLoad(this.gameObject); // Keep loader alive too (optional)
    }
}