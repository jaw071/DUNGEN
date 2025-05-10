using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;

public class CookingManger : MonoBehaviour
{
    private Item currentItem;
    public Image CustomCursor;

    public Slot[] cookingSlot;

    public List<Item> itemList;
    public string[] recipes;
    public Item[] recipeResults;
    public Slot resultSlot;

    [Header("Assign the tutorial panel UI GameObject")]
    public GameObject HelpPanel;

    private bool isVisible = false;

    //Puts the food into the slot the player placed them
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (currentItem != null)
            {
                CustomCursor.gameObject.SetActive(false);
                Slot nearestSlot = null;
                float shortestDistance = float.MaxValue;

                //Goes through all cooking slots
                foreach (Slot slot in cookingSlot) 
                {
                    float dist = Vector2.Distance(Input.mousePosition, slot.transform.position);

                    if (dist < shortestDistance)
                    {
                        shortestDistance = dist;
                        nearestSlot = slot;
                    }
                }
                nearestSlot.gameObject.SetActive(true);
                nearestSlot.GetComponent<Image>().sprite = currentItem.GetComponent<Image>().sprite;
                nearestSlot.item = currentItem;
                itemList[nearestSlot.index] = currentItem;


                currentItem = null;

                CheckForCreatedRecipes();
            }
        }
    }


    // Call this from your button OnClick event
    public void ToggleTutorial()
    {
        isVisible = !isVisible;
        HelpPanel.SetActive(isVisible);
    }

    // Optional: Start with the panel hidden
    private void Start()
    {
        if (HelpPanel != null)
        {
            HelpPanel.SetActive(false);
            isVisible = false;
        }
    }


        //Checks to see if the player has completed a recipe
        void CheckForCreatedRecipes()
    {
        resultSlot.gameObject.SetActive(false);
        resultSlot.item = null;

        string currentRecipeString = "";
        //Forming the recipe string
        foreach (Item item in itemList)
        {
            if (item != null)
            {
                currentRecipeString += item.itemName;
            }
            else
            {
                currentRecipeString += null;
            }
        }

        for (int i = 0; i < recipes.Length; i++)
        {
            if (recipes[i] == currentRecipeString)
            {
                resultSlot.gameObject.SetActive(true);
                resultSlot.GetComponent<Image>().sprite = recipeResults[i].GetComponent<Image>().sprite;
                resultSlot.item = recipeResults[i];
            }
        }
        
    }


    //Removes item from slot by clicking it
    public void OnClickSlot(Slot slot)
    {
        slot.item = null;
        itemList[slot.index] = null;
        slot.gameObject.SetActive(false);
        CheckForCreatedRecipes();
    }


    public void OnMouseDownItem(Item item)
    {
        if (currentItem == null)
        {
            currentItem = item;
            CustomCursor.gameObject.SetActive(true);
            CustomCursor.sprite = currentItem.GetComponent<Image>().sprite;
        }
    }
}
