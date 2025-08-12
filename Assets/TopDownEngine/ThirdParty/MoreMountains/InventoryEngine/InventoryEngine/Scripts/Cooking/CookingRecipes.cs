using MoreMountains.InventoryEngine;
using UnityEngine;

namespace TopDownEngine.ThirdParty.MoreMountains.InventoryEngine.InventoryEngine.Scripts.Cooking
{
    [CreateAssetMenu(fileName = "New Cooking Recipe", menuName = "Cooking/Recipe")]
    public class CookingRecipe : ScriptableObject
    {
        [Header("Ingredients (2 items needed)")]
        public string ingredient1ID;
        public string ingredient2ID;

        [Header("Result Item")]
        public InventoryItem resultItem; // Drag the item asset here

        // Matches ingredients regardless of order
        public bool Matches(string idA, string idB)
        {
            return (idA == ingredient1ID && idB == ingredient2ID) ||
                   (idA == ingredient2ID && idB == ingredient1ID);
        }
    }
}