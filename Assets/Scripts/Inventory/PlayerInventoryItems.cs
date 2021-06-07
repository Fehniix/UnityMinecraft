using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryItems : MonoBehaviour
{
	/// <summary>
	/// Reference to the item objects in the grid.
	/// </summary>
	public GameObject[] itemObjects;

	/// <summary>
	/// The number of items the inventory will contain.
	/// </summary>
	[SerializeField]
	private int itemsCount;

    void Awake()
    {
		GridLayoutGroup layoutGroup = this.GetComponent<GridLayoutGroup>();
		layoutGroup.cellSize 		= new Vector2(32, 32);
		layoutGroup.spacing 		= new Vector2(4, 3);

        // Initialize the inventory item objects.
		this.itemObjects = new GameObject[this.itemsCount];

		for (int i = 0; i < this.itemsCount; i++)
			this.itemObjects[i] = this.CreateItemSlotObject(i);

		this.UpdateGUI();
    }

	/// <summary>
	/// Updates the inventory container GUI.
	/// </summary>
	public void UpdateGUI()
	{
		for (int i = 0; i < this.itemsCount; i++)
		{
			if (InventoryContainers.inventory == null)
				return;
				
			InventoryItem item 		= InventoryContainers.inventory.items[i];
			GameObject itemObj 		= this.itemObjects[i];

			if (item == null || item?.quantity == 0)
				InventoryContainers.inventory.items[i] = null;
			
			this.UpdateSingleItem(item, itemObj);
		}
	}

	/// <summary>
	/// Given an `InventoryItem` and its inventory GameObject (object that contains the the image & quantity text),
	/// updates the single item's texture and quantity text.
	/// </summary>
	private void UpdateSingleItem(InventoryItem item, GameObject inventoryItemGameObject)
	{
		Image image				= inventoryItemGameObject.GetComponent<Image>();
		GameObject quantityText	= inventoryItemGameObject.transform.GetChild(0).gameObject;

		if (item == null || item?.quantity == 0) 
		{
			inventoryItemGameObject.GetComponent<InventoryItemSlot>().itemName = null;
			image.color = Color.clear;
			quantityText.SetActive(false);
			return;
		}

		string itemName = item.itemName;

		inventoryItemGameObject.GetComponent<InventoryItemSlot>().itemName = itemName;
		image.sprite = TextureStitcher.instance.GetBlockItemSprite(itemName);
		image.color = Color.white;

		if (item.quantity != 1)
		{
			quantityText.SetActive(true);
			quantityText.GetComponent<Text>().text = item.quantity.ToString();

			int fontSize = item.quantity > 99 ? 18 : 22;
			quantityText.GetComponent<Text>().fontSize = fontSize;
		}
		else
			quantityText.SetActive(false);
	}

	/// <summary>
	/// Allows to create an item slot object to append to the grid.
	/// </summary>
	private GameObject CreateItemSlotObject(int index)
	{
		GameObject itemSlotObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UIItemSlot"));
		itemSlotObject.name = String.Format("item{0}", index);
		itemSlotObject.transform.SetParent(this.transform, false);

		switch(this.name)
		{
			case "Items":
				itemSlotObject.GetComponent<InventoryItemSlot>().slotType = InventorySlotType.INVENTORY;
				break;
			case "Hotbar":
				itemSlotObject.GetComponent<InventoryItemSlot>().slotType = InventorySlotType.HOTBAR;
				break;
			case "Crafting":
				itemSlotObject.GetComponent<InventoryItemSlot>().slotType = InventorySlotType.CRAFTING;
				break;
			default:
				itemSlotObject.GetComponent<InventoryItemSlot>().slotType = InventorySlotType.INVENTORY;
				break;
		}

		itemSlotObject.GetComponent<InventoryItemSlot>().slotIndex = index;

		return itemSlotObject;
	}
}
