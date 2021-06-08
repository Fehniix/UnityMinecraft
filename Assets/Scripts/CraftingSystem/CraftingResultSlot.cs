using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftingResultSlot : MonoBehaviour, IPointerDownHandler
{
	/// <summary>
	/// Reference to the associated crafting grid.
	/// Set in Unity Editor.
	/// </summary>
	public CraftingGridObject craftingGrid;

	/// <summary>
	/// The result item associated with the crafting recipe inside the grid.
	/// </summary>
	public CraftingResult? resultCraftingItem;

	/// <summary>
	/// Handle the item being picked up from the craftin result slot.
	/// </summary>
	public void OnPointerDown(PointerEventData data)
	{
		if (data.pointerId < -2 || this.resultCraftingItem == null)
			return;
		
		CraftingResult result = this.resultCraftingItem.Value;

		if (Input.GetKey(KeyCode.LeftShift))
		{
			while(resultCraftingItem != null && PlayerInventoryManager.AddItem(result.itemName, result.quantity) > 0)
				this.ConsumeRequirementItems();
		}
		else
		{
			this.ConsumeRequirementItems();
			InventoryContainers.draggingItem = new InventoryItem(result.itemName);
			InventoryContainers.draggingItem.quantity = result.quantity;
			InventoryContainers.draggingItemObject.GetComponent<DraggingItem>().UpdateTexture();
			
			this.UpdateTexture();
		}
	}

	/// <summary>
	/// Allows to consume the crafting requirement items and craft the recipe.
	/// </summary>
	private void ConsumeRequirementItems()
	{
		foreach(InventoryItem requirement in this.craftingGrid.gameObject.GetComponent<InventoryContainer>().items)
			if (requirement != null)
				requirement.quantity--;

		if (GUI.isAGUIShown)
			GUI.activeGUI.UpdateGUI();

		this.craftingGrid.OnItemsChanged();
	}

	/// <summary>
	/// Updates the texture of the result crafting item slot.
	/// </summary>
	public void UpdateTexture()
	{
		if (this.resultCraftingItem == null)
		{
			this.gameObject.SetActive(false);
			return;
		}

		this.gameObject.SetActive(true);

		this.gameObject.GetComponentInChildren<Image>().sprite 	= TextureStitcher.instance.GetBlockItemSprite(this.resultCraftingItem?.itemName);
		this.gameObject.GetComponentInChildren<Image>().color 	= Color.white;

		GameObject textObj = this.gameObject.transform.GetChild(0).gameObject;

		if (this.resultCraftingItem?.quantity != 1)
		{
			textObj.SetActive(true);
			textObj.GetComponent<Text>().text = this.resultCraftingItem?.quantity.ToString();
		}
		else
			textObj.SetActive(false);
	}
}
