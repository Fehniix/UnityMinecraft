using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CraftingGridObject : MonoBehaviour
{
	/// <summary>
	/// Defines the number of item slots of the crafting grid.
	/// Set in Unity Editor.
	/// </summary>
	public int slotCount = 1;

	/// <summary>
	/// Reference to the result crafting grid slot.
	/// Set in Unity Editor.
	/// </summary>
	public CraftingResultSlot craftingGridResultSlot;

	void Start()
	{
		this.GetComponent<InventoryContainer>().itemsChangedEvent += this.OnItemsChanged;
	}

	void Destroy()
	{
		this.GetComponent<InventoryContainer>().itemsChangedEvent -= this.OnItemsChanged;
	}

	/// <summary>
	/// Watches for changes within the crafting grid and eventually matches requirements for a recipe.
	/// </summary>
    public void OnItemsChanged()
	{
		string[,] requirements = this.GetComponent<InventoryContainer>().ItemsToCraftingRequirements();

		this.craftingGridResultSlot.resultCraftingItem = CraftingRecipeRegistry.GetCraftingResult(requirements);
		this.craftingGridResultSlot.UpdateTexture();
	}
}
