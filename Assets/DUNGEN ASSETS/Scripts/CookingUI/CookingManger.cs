using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.InventoryEngine;

public class CookingManger : MonoBehaviour
{
    public Slot[] cookingSlot;
    public Slot resultSlot;
    public InventoryItem[] recipeResults;
    public string[] recipes;

    public InventoryItem[] itemList;
    public InventoryItem currentItem;
    public Image customCursor;

    private void Awake()
    {
        itemList = new InventoryItem[cookingSlot.Length];
        customCursor.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && currentItem != null)
        {
            customCursor.gameObject.SetActive(false);

            Slot nearestSlot = null;
            float closestDistance = float.MaxValue;

            foreach (Slot slot in cookingSlot)
            {
                float distance = Vector2.Distance(Input.mousePosition, slot.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestSlot = slot;
                }
            }

            if (nearestSlot != null)
            {
                nearestSlot.gameObject.SetActive(true);
                nearestSlot.GetComponent<Image>().sprite = currentItem.Icon;
                nearestSlot.item = currentItem;
                itemList[nearestSlot.index] = currentItem;
                CheckForCreatedRecipes();
            }

            currentItem = null;
        }

        if (currentItem != null)
        {
            customCursor.transform.position = Input.mousePosition;
        }
    }

    public void OnMouseDownItem(InventoryItem item)
    {
        if (currentItem == null)
        {
            currentItem = item;
            customCursor.gameObject.SetActive(true);
            customCursor.sprite = item.Icon;
        }
    }

    public void OnClickSlot(Slot slot)
    {
        itemList[slot.index] = null;
        slot.item = null;
        slot.gameObject.SetActive(false);
        CheckForCreatedRecipes();
    }

    public void CheckForCreatedRecipes()
    {
        // Reset result slot
        resultSlot.gameObject.SetActive(false);
        resultSlot.item = null;

        // Build the current recipe string
        string currentRecipeString = "";

        foreach (InventoryItem item in itemList)
        {
            if (item != null)
            {
                currentRecipeString += item.ItemID;
            }
            else
            {
                currentRecipeString += "null";
            }
        }

        Debug.Log("Current recipe: " + currentRecipeString);

        // Check against known recipes
        for (int i = 0; i < recipes.Length; i++)
        {
            if (recipes[i] == currentRecipeString)
            {
                Debug.Log("Recipe matched: " + recipes[i]);

                // Show result in result slot
                resultSlot.gameObject.SetActive(true);
                resultSlot.GetComponent<Image>().sprite = recipeResults[i].Icon;
                resultSlot.item = recipeResults[i];

                Debug.Log("Crafted: " + recipeResults[i].ItemName + ", Icon: " + recipeResults[i].Icon);

                // Add crafted item to global inventory
                Inventory.GlobalInstance.AddItem(recipeResults[i], 1);

                Debug.Log("Crafted item added to inventory: " + recipeResults[i].ItemID);

                // Remove used ingredients from inventory and clear slots
                for (int j = 0; j < itemList.Length; j++)
                {
                    if (itemList[j] != null)
                    {
                        Inventory.GlobalInstance.RemoveItemByID(itemList[j].ItemID, 1);
                        itemList[j] = null;

                        cookingSlot[j].item = null;
                        cookingSlot[j].gameObject.SetActive(false);
                    }
                }

                break;
            }
        }
    }
}