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
		this.draggingItem 				= this.transform.Find("InventoryBG/DraggingItem").gameObject;
		InventoryManager.inventoryItems = new InventoryItem[27];
        InventoryManager.inventoryRef 	= this;

		this.gameObject.SetActive(false);
    }

    void Update()
    {
		/**
		* A bit of an explanation is due - the following code doesn't forgive.
		* REVIEW needs to be cleaned.
		* `this.draggingItem` is the GameObject that contains the UI.Image GameObject that represents the item following the cursor.
		* If an item is currently being dragged (fetched from the static InventoryManager),
		* have it follow Input.mousePosition and, if still inactive, initialize its image and quantity text.
		* Otherwise, if the an item is not being dragged any longer and `this.draggingItem` is still active, deactivate it.
		*/
		if (InventoryManager.draggingItem != null)
		{
			if (!this.draggingItem.activeSelf)
			{
				this.draggingItem.SetActive(true);
				
				this.draggingItem.GetComponentInChildren<Image>().sprite 	= TextureStitcher.instance.GetBlockItemSprite(InventoryManager.draggingItem.itemName);
				this.draggingItem.GetComponentInChildren<Image>().color 	= Color.white;

				GameObject textObj = this.draggingItem.transform.GetChild(1).gameObject;

				if (InventoryManager.draggingItem.quantity != 1)
				{
					textObj.SetActive(true);
					textObj.GetComponent<Text>().text = InventoryManager.draggingItem.quantity.ToString();
				}
				else
					textObj.SetActive(false);
			}

			this.draggingItem.transform.position = Input.mousePosition;
		}
		else if (this.draggingItem.activeSelf)
			this.draggingItem.SetActive(false);
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
			inventoryItemGameObject.GetComponent<InventoryItemImage>().itemName = null;
			image.color = Color.clear;
			quantityText.SetActive(false);
			return;
		}

		string itemName = item.itemName;

		inventoryItemGameObject.GetComponent<InventoryItemImage>().itemName = itemName;
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
