using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemImage : MonoBehaviour, IPointerDownHandler
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
	/// </summary>
	public InventorySlotType slotType;

	void Awake()
	{
		Regex regex = new Regex("[0-9]{1,2}$");
		Match match = regex.Match(this.transform.gameObject.name);

		if (match.Value == null)
			return;

		this.slotIndex = System.Int32.Parse(match.Value);

		switch(this.transform.parent.name)
		{
			case "Items":
				this.slotType = InventorySlotType.INVENTORY;
				break;
			case "Hotbar":
				this.slotType = InventorySlotType.HOTBAR;
				break;
			case "Crafting":
				this.slotType = InventorySlotType.CRAFTING;
				break;
			default:
				this.slotType = InventorySlotType.INVENTORY;
				break;
		}
	}

	/// <summary>
	/// This method gets called for both source and target slot.
	/// When dragging an item around (source), its name is saved in the InventoryManager.
	/// The target item is thus aware of it and can replace its image with the saved instance.
	/// </summary>
	public void OnPointerDown(PointerEventData data)
	{
		// pointerId: -1 indicates the left mouse button was pressed.
		if (data.pointerId != -1)
			return;

		if (InventoryManager.sourceDraggingItemImage == null)
		{
			// Item picked up.

			if (this.itemName == null)
				return;

			Debug.Log("Started dragging " + this.slotType + " " + this.itemName + " " + this.slotIndex);
			InventoryManager.sourceDraggingItemImage = this;
			
			switch(this.slotType)
			{
				case InventorySlotType.INVENTORY:
					InventoryManager.draggingItem = InventoryManager.inventoryItems[this.slotIndex];
					InventoryManager.inventoryItems[this.slotIndex] = null;
					break;
				case InventorySlotType.HOTBAR:
					InventoryManager.draggingItem = InventoryManager.hotbarItems[this.slotIndex];
					InventoryManager.hotbarItems[this.slotIndex] = null;
					break;
				case InventorySlotType.CRAFTING:
					InventoryManager.hotbarItems[this.slotIndex] = InventoryManager.craftingGrid[this.slotIndex];
					InventoryManager.craftingGrid[this.slotIndex] = null;
					break;
			}

			InventoryManager.inventoryRef.UpdateInventoryItems();
		}
		else
		{
			// Item dropped down.

			Debug.Log("Item dropped down: " + this.slotType + " " + this.itemName + " " + this.slotIndex);

			switch(this.slotType)
			{
				case InventorySlotType.INVENTORY:
					InventoryManager.inventoryItems[this.slotIndex] = InventoryManager.draggingItem;
					InventoryManager.draggingItem = null;
					break;
				case InventorySlotType.HOTBAR:
					InventoryManager.hotbarItems[this.slotIndex] = InventoryManager.draggingItem;
					InventoryManager.draggingItem = null;
					break;
				case InventorySlotType.CRAFTING:
					InventoryManager.craftingGrid[this.slotIndex] = InventoryManager.draggingItem;
					InventoryManager.draggingItem = null;
					break;
			}

			InventoryManager.sourceDraggingItemImage = null;

			InventoryManager.inventoryRef.UpdateInventoryItems();
		}
	}
}
