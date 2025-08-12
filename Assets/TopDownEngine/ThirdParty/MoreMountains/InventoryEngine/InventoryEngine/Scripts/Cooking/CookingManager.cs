using System.Collections.Generic;
using MoreMountains.InventoryEngine;
using UnityEngine;

namespace TopDownEngine.ThirdParty.MoreMountains.InventoryEngine.InventoryEngine.Scripts.Cooking
{
    public class CookingManager : MonoBehaviour
    {
        public List<CookingRecipe> recipes;   // recipe assets (ResultItem should be InventoryItem)
        public Inventory playerInventory;     // assign in inspector

        public bool TryCook(string firstItemID, string secondItemID)
        {
            foreach (var recipe in recipes)
            {
                if (!recipe.Matches(firstItemID, secondItemID))
                    continue;

                // Quick availability check
                var slotsA = playerInventory.InventoryContains(firstItemID);
                var slotsB = playerInventory.InventoryContains(secondItemID);
                if (slotsA.Count == 0 || slotsB.Count == 0)
                {
                    Debug.Log("Cook Failed: Missing ingredients.");
                    return false;
                }

                // Remove first ingredient
                playerInventory.RemoveItem(slotsA[0], 1);

                // Remove second ingredient
                if (firstItemID == secondItemID)
                {
                    // If same ID, re-query to make sure there's still another unit available
                    var slotsAgain = playerInventory.InventoryContains(secondItemID);
                    if (slotsAgain.Count == 0)
                    {
                        Debug.Log("Cook Failed: Not enough of the ingredient.");
                        return false;
                    }
                    playerInventory.RemoveItem(slotsAgain[0], 1);
                }
                else
                {
                    // Re-query because indices may have changed after the first removal
                    var slotsBNew = playerInventory.InventoryContains(secondItemID);
                    if (slotsBNew.Count == 0)
                    {
                        Debug.Log("Cook Failed: second ingredient vanished unexpectedly.");
                        return false;
                    }
                    playerInventory.RemoveItem(slotsBNew[0], 1);
                }

                // Add the result: pass the InventoryItem asset and an int quantity (not playerID)
                if (recipe.resultItem == null)
                {
                    Debug.LogError($"Recipe '{recipe.name}' has no ResultItem assigned.");
                    return false;
                }

                bool added = playerInventory.AddItem(recipe.resultItem, 1);
                if (!added)
                {
                    Debug.LogWarning("Cooked item couldn't be added (inventory full?). Ingredients already removed.");
                    // TODO optional: implement rollback here if you want to return removed ingredients
                    return false;
                }

                Debug.Log("Cooked " + recipe.resultItem.ItemID);
                return true;
            }

            Debug.Log("Cook Failed: No Matching Recipes Found.");
            return false;
        }
    }
}
