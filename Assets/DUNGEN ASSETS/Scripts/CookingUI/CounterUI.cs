using UnityEngine.UI;
using MoreMountains.InventoryEngine;
using UnityEngine;

public class CounterUI : MonoBehaviour
{
    //public Inventory inventory = Inventory.GlobalInstance;
    public Text BurkenMeatText;

    void Update()
    {
        if (Inventory.GlobalInstance == null)
        {
            Debug.LogWarning("Inventory not available yet!");
            return;
        }

        BurkenMeatText.text = "Burken Meat: " + Inventory.GlobalInstance.GetQuantity("BurkenMeat"); //Copy this for other foods    

    }
}
