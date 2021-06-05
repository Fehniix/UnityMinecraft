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
		if (data.pointerId != -1)
			return;

		InventoryContainer inventoryContainer = this.GetComponentInParent<InventoryContainer>();

		if (InventoryContainers.draggingItem == null)
		{
			// Item picked up.

			if (this.itemName == null)
				return;

			Debug.Log("Started dragging " + this.slotType + " " + this.itemName + " " + this.slotIndex);

			inventoryContainer.items[this.slotIndex] = null;

			InventoryContainers.draggingItemObject.gameObject.SetActive(true);
			InventoryContainers.draggingItemObject = this.gameObject;
			InventoryContainers.draggingItem = inventoryContainer.items[this.slotIndex];

			inventoryContainer.UpdateGUI();
		}
		else
		{
			// Item dropped down.

			Debug.Log("Item dropped down: " + this.slotType + " " + this.itemName + " " + this.slotIndex);

			inventoryContainer.items[this.slotIndex] = InventoryContainers.draggingItem;
			
			InventoryContainers.draggingItemObject = null;
			InventoryContainers.draggingItemObject.gameObject.SetActive(false);
			InventoryContainers.draggingItem = null;

			inventoryContainer.UpdateGUI();
		}
	}
}
