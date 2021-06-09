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
	/// The inventory container.
	/// </summary>
	private InventoryContainer inventoryContainer;

	/// <summary>
	/// This method gets called for both source and target slot.
	/// When dragging an item around (source), its name is saved in the InventoryManager.
	/// The target item is thus aware of it and can replace its image with the saved instance.
	/// </summary>
	public void OnPointerDown(PointerEventData data)
	{
		Hotbar hotbar = this.gameObject.GetComponentInParent<Hotbar>();
		if (hotbar?.itemSlotsClickable == false)
			return;
			
		// pointerId: -1 indicates the left mouse button was pressed.
		if (data.pointerId == -1)
			this.OnLeftMouseButtonClick();

		if (data.pointerId == -2)
			this.OnRightMouseButtonClick();
	}

	private void OnLeftMouseButtonClick()
	{
		this.InitializeInventoryContainer();

		if (InventoryContainers.draggingItem == null)
		{
			// * Item picked up.

			if (this.itemName == null)
				return;

			InventoryContainers.draggingItem = inventoryContainer.items[this.slotIndex];
			InventoryContainers.draggingItemObject.SetActive(true);
			InventoryContainers.draggingItemObject.GetComponent<DraggingItem>().UpdateTexture();

			this.inventoryContainer.items[this.slotIndex] = null;
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

					this.inventoryContainer.items[this.slotIndex].quantity += draggableQuantity;
				}
			}
			else
			{
				// * Target item is null.

				this.inventoryContainer.items[this.slotIndex] = InventoryContainers.draggingItem;
			
				InventoryContainers.draggingItemObject.SetActive(false);
				InventoryContainers.draggingItem = null;
			}
		}

		this.inventoryContainer.UpdateGUI();
		this.inventoryContainer.TriggerItemsChangedEvent();
	}

	private void OnRightMouseButtonClick()
	{
		this.InitializeInventoryContainer();
		
		InventoryItem inventoryItem = inventoryContainer.items[this.slotIndex];
		InventoryItem draggingItem 	= InventoryContainers.draggingItem;

		if (inventoryItem == null && draggingItem == null)
			return;

		if (draggingItem == null)
		{
			if (inventoryItem.quantity == 1)
				this.SwapDraggable();
			else
			{
				int quantity = inventoryItem.quantity;

				inventoryItem.quantity = quantity / 2 + quantity % 2;
				InventoryContainers.draggingItem = inventoryItem.Clone();
				InventoryContainers.draggingItem.quantity -= quantity % 2;
			}
		}
		else
		{
			if (inventoryItem == null)
			{
				this.inventoryContainer.items[this.slotIndex] = draggingItem.Clone();
				this.inventoryContainer.items[this.slotIndex].quantity = 1;
				draggingItem.quantity--;
			}
			else 
			{
				// * Both items are not null. Check for same name and max item stack.
			
				int currentSlotQuantity 	= inventoryItem.quantity;
				bool sameName 				= inventoryItem.itemName == draggingItem.itemName;
				bool tooManyIfCombined 		= currentSlotQuantity + 1 > inventoryItem.maxStack;

				if (sameName && !tooManyIfCombined)
				{
					inventoryItem.quantity++;
					draggingItem.quantity--;
				}
			}
		}

		if (draggingItem?.quantity == 0)
			InventoryContainers.draggingItem = null;

		if (inventoryItem?.quantity == 0)
			this.inventoryContainer.items[this.slotIndex] = null;

		InventoryContainers.draggingItemObject.GetComponent<DraggingItem>().UpdateTexture();
		this.inventoryContainer.UpdateGUI();

		this.inventoryContainer.TriggerItemsChangedEvent();
	}

	/// <summary>
	/// Allows to swap the currently dragging item with the target item slot item.
	/// </summary>
	private void SwapDraggable()
	{
		this.InitializeInventoryContainer();

		InventoryItem tmp 							= inventoryContainer.items[this.slotIndex];
		this.inventoryContainer.items[this.slotIndex] 	= InventoryContainers.draggingItem;
		InventoryContainers.draggingItem 			= tmp;
		InventoryContainers.draggingItemObject.GetComponent<DraggingItem>().UpdateTexture();
	}

	/// <summary>
	/// Initializes the inventory container for this item slot.
	/// </summary>
	private void InitializeInventoryContainer()
	{
		if (this.inventoryContainer != null)
			return;

		this.inventoryContainer = this.GetComponentInParent<InventoryContainer>();

		if (this.GetComponentInParent<PlayerInventoryItems>() != null)
			this.inventoryContainer = InventoryContainers.inventory;

		if (this.GetComponentInParent<PlayerInventoryHotbar>() != null)
			this.inventoryContainer = InventoryContainers.hotbar;
	}
}
