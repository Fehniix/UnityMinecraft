using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemSlot : MonoBehaviour, IPointerDownHandler
{
	/// <summary>
	/// Describes the index of this item slot.
	/// </summary>
	public int slotIndex = 0;

	/// <summary>
	/// The name of the item.
	/// </summary>
	public string itemName;

	/// <summary>
	/// The type of inventory slot.
	/// This is used by the player inventory.
	/// </summary>
	public InventorySlotType slotType;

	/// <summary>
	/// This method gets called for both source and target slot.
	/// When dragging an item around (source), its name is saved in the InventoryManager.
	/// The target item is thus aware of it and can replace its image with the saved instance.
	/// </summary>
	public void OnPointerDown(PointerEventData data)
	{
		// pointerId: -1 indicates the left mouse button was pressed.
		if (data.pointerId == -1)
			this.OnLeftMouseButtonClick();

		if (data.pointerId == -2)
			this.OnRightMouseButtonClick();
	}

	private void OnLeftMouseButtonClick()
	{
		InventoryContainer inventoryContainer = this.GetComponentInParent<InventoryContainer>();

		if (InventoryContainers.draggingItem == null)
		{
			// * Item picked up.

			if (this.itemName == null)
				return;

			InventoryContainers.draggingItem = inventoryContainer.items[this.slotIndex];
			InventoryContainers.draggingItemObject.SetActive(true);
			InventoryContainers.draggingItemObject.GetComponent<DraggingItem>().UpdateTexture();

			inventoryContainer.items[this.slotIndex] = null;

			inventoryContainer.UpdateGUI();
		}
		else
		{
			// * Item dropped down.

			if (inventoryContainer.items[this.slotIndex] != null)
			{
				// * Target item is not null.

				int draggableQuantity 	= InventoryContainers.draggingItem.quantity;
				int currentSlotQuantity = inventoryContainer.items[this.slotIndex].quantity;
				bool sameName 			= inventoryContainer.items[this.slotIndex].itemName == InventoryContainers.draggingItem.itemName;
				bool tooManyIfCombined 	= currentSlotQuantity + draggableQuantity > inventoryContainer.items[this.slotIndex].maxStack;

				if (!sameName || tooManyIfCombined)
					this.SwapDraggable();

				if (sameName && !tooManyIfCombined)
				{
					InventoryContainers.draggingItemObject.SetActive(false);
					InventoryContainers.draggingItem = null;

					inventoryContainer.items[this.slotIndex].quantity += draggableQuantity;
				}
			}
			else
			{
				// * Target item is null.

				inventoryContainer.items[this.slotIndex] = InventoryContainers.draggingItem;
			
				InventoryContainers.draggingItemObject.SetActive(false);
				InventoryContainers.draggingItem = null;
			}

			inventoryContainer.UpdateGUI();
		}
	}

	/// <summary>
	/// Allows to swap the currently dragging item with the target item slot item.
	/// </summary>
	private void SwapDraggable()
	{
		InventoryContainer inventoryContainer = this.GetComponentInParent<InventoryContainer>();

		InventoryItem tmp 							= inventoryContainer.items[this.slotIndex];
		inventoryContainer.items[this.slotIndex] 	= InventoryContainers.draggingItem;
		InventoryContainers.draggingItem 			= tmp;
		InventoryContainers.draggingItemObject.GetComponent<DraggingItem>().UpdateTexture();
	}

	private void OnRightMouseButtonClick()
	{
		InventoryContainer inventoryContainer = this.GetComponentInParent<InventoryContainer>();
		// ANCHOR Start from here.
		if (inventoryContainer.items[this.slotIndex] != null)
		{
			if (InventoryContainers.draggingItem == null)
			{
				// * Pick up half of the items in the item slot.

				InventoryContainers.draggingItem = inventoryContainer.items[this.slotIndex].Clone();
				InventoryContainers.draggingItem.quantity /= 2;
			}
			else
			{
				// * Put down a single
				inventoryContainer.items[this.slotIndex].quantity++;
				InventoryContainers.draggingItem.quantity--;
			}

			if (inventoryContainer.items[this.slotIndex].quantity == 0)
				inventoryContainer.items[this.slotIndex] = null;
		}
		else
		{

		}

		InventoryContainers.draggingItemObject.GetComponent<DraggingItem>().UpdateTexture();
		inventoryContainer.UpdateGUI();
	}
}
