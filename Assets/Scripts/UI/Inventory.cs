using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
	/// <summary>
	/// The image object with the item that is currently being dragged around.
	/// </summary>
	public GameObject draggingItem;

    void Awake()
    {
		this.gameObject.SetActive(false);
    }

	/// <summary>
	/// Redraws the inventory images.
	/// </summary>
	public void UpdateInventoryItems()
	{
		InventoryItemsContainer inventoryContainer 	= this.GetComponentInChildren<InventoryItemsContainer>();
		HotbarItemsContainer hotbarContainer 		= this.GetComponentInChildren<HotbarItemsContainer>();
		CraftingItemsContainer craftingContainer 	= this.GetComponentInChildren<CraftingItemsContainer>();

		for(int i = 0; i < inventoryContainer.transform.childCount; i++)
		{
			InventoryItem item 		= InventoryManager.inventoryItems[i];
			GameObject itemObj 		= inventoryContainer.gameObject.transform.GetChild(i).gameObject;
			
			this.UpdateSingleItem(item, itemObj);
		}

		for (int i = 0; i < hotbarContainer.transform.childCount; i++)
		{
			InventoryItem item 		= InventoryManager.hotbarItems[i];
			GameObject itemObj 		= hotbarContainer.gameObject.transform.GetChild(i).gameObject;

			this.UpdateSingleItem(item, itemObj);
		}
	}

	/// <summary>
	/// Given an `InventoryItem` and its inventory GameObject (object that contains the the image & quantity text),
	/// updates the single item's texture and quantity text.
	/// </summary>
	private void UpdateSingleItem(InventoryItem item, GameObject inventoryItemGameObject)
	{
		Image image				= inventoryItemGameObject.GetComponentInChildren<Image>();
		GameObject quantityText	= inventoryItemGameObject.transform.GetChild(1).gameObject;

		if (item == null) 
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
}
