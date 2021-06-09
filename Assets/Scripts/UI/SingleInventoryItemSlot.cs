using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SingleInventoryItemSlot : MonoBehaviour, IPointerDownHandler
{
	/// <summary>
	/// The item that the single slot is containing.
	/// </summary>
	public InventoryItem item;

	/// <summary>
	/// Determines whether the player can insert an item in this slot or not.
	/// Can be set in Unity Editor.
	/// </summary>
	public bool canPlayerInputAnItem = true;

	/// <summary>
	/// Delegate designating an ItemChanged event.
	/// </summary>
	protected delegate void ItemChanged();

	/// <summary>
	/// The ItemChanged handlers. Subscribe by using the += operator, unsub with -=. Gets called internally.
	/// </summary>
	protected ItemChanged itemChangedHandlers;

	virtual protected void Start()
	{
		this.UpdateTexture();
	}

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

	/// <summary>
	/// Triggered when the player left clicks the item slot.
	/// </summary>
	protected virtual void OnLeftMouseButtonClick()
	{
		if (InventoryContainers.draggingItem == null)
		{
			// * Item picked up.

			if (this.item == null)
				return;
				
			InventoryContainers.draggingItem = this.item;
			InventoryContainers.draggingItemObject.SetActive(true);
			InventoryContainers.draggingItemObject.GetComponent<DraggingItem>().UpdateTexture();

			this.item = null;
		}
		else
		{
			// * Item dropped down.
			if (!this.canPlayerInputAnItem)
				return;

			if (this.item != null)
			{
				// * Target item is not null.

				int draggableQuantity 	= InventoryContainers.draggingItem.quantity;
				int currentSlotQuantity = this.item.quantity;
				bool sameName 			= this.item.itemName == InventoryContainers.draggingItem.itemName;
				bool tooManyIfCombined 	= currentSlotQuantity + draggableQuantity > this.item.maxStack;

				if (!sameName || tooManyIfCombined)
					this.SwapDraggable();

				if (sameName && !tooManyIfCombined)
				{
					InventoryContainers.draggingItemObject.SetActive(false);
					InventoryContainers.draggingItem = null;

					this.item.quantity += draggableQuantity;
				}
			}
			else
			{
				// * Target item is null.

				this.item = InventoryContainers.draggingItem;
			
				InventoryContainers.draggingItemObject.SetActive(false);
				InventoryContainers.draggingItem = null;
			}
		}

		this.UpdateTexture();
		this.itemChangedHandlers();
	}

	/// <summary>
	/// Triggered when the player right clicks the item slot.
	/// </summary>
	protected virtual void OnRightMouseButtonClick()
	{	
		InventoryItem inventoryItem = this.item;
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
				this.item = draggingItem.Clone();
				this.item.quantity = 1;
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
			this.item = null;

		InventoryContainers.draggingItemObject.GetComponent<DraggingItem>().UpdateTexture();
		this.UpdateTexture();
		this.itemChangedHandlers();
	}

	/// <summary>
	/// Updates the item slot texture & quantity text.
	/// </summary>
	public void UpdateTexture()
	{
		Image image				= this.GetComponent<Image>();
		GameObject quantityText	= this.transform.GetChild(0).gameObject;

		if (item == null || item?.quantity == 0) 
		{
			this.item = null;
			image.color = Color.clear;
			quantityText.SetActive(false);
			return;
		}

		string itemName = item.itemName;

		this.item.itemName = itemName;
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
	/// Allows to swap the currently dragging item with the target item slot item.
	/// </summary>
	private void SwapDraggable()
	{
		InventoryItem tmp 							= this.item;
		this.item 									= InventoryContainers.draggingItem;
		InventoryContainers.draggingItem 			= tmp;
		InventoryContainers.draggingItemObject.GetComponent<DraggingItem>().UpdateTexture();
	}
}
