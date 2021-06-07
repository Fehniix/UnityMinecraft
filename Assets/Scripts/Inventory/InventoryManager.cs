using UnityEngine;
using Extensions;

public static class PlayerInventoryManager
{
	/// <summary>
	/// Represents the index of the currently active item.
	/// </summary>
	public static int activeItemIndex;

	/// <summary>
	/// Reference to the hotbar component.
	/// </summary>
	public static Hotbar hotbarRef;

	/// <summary>
	/// Reference to the inventory component.
	/// </summary>
	public static PlayerInventory playerInventoryRef;

	/// <summary>
	/// Tries to place or use the currently active item.
	/// </summary>
	public static bool ConsumeActive()
	{
		InventoryItem[] hotbarItems = InventoryContainers.hotbar.items;
		InventoryItem activeItem = hotbarItems[activeItemIndex];

		if (!activeItem.placeable)
			return false;

		object instantiatedObject 	= Registry.Instantiate(activeItem.itemName);
		Block block 				= instantiatedObject as Block;
		Item item 					= instantiatedObject as Item;

		// Active item is an item.
		if (block == null)
		{
			if (!item.placeable)
				return false;

			bool hitOtherItem;
			Vector4? placement 		= GetPlacementCoordinates(out hitOtherItem);

			if ((placement == null || placement.Value.w == 0) && item.placeableOnlyOnTop)
				return false;

			if (placement.Value.w == 1 && !item.placeableOnOtherItems && hitOtherItem)
				return false;

			item.coordinates = placement.Value;
			item.Place();
		}
		else
		{
			if (!block.placeable)
				return false;

			block.Place();
		}

		hotbarItems[activeItemIndex].quantity--;
		if (hotbarItems[activeItemIndex].quantity == 0)
			hotbarItems[activeItemIndex] = null;

		PlayerInventoryManager.hotbarRef.UpdateGUI();

		return true;
	}

	/// <summary>
	/// Determines whether the currently active item is consumable (usable or placeable) or not.
	/// </summary>
	public static bool IsActiveItemConsumable()
	{
		InventoryItem[] hotbarItems = InventoryContainers.hotbar.items;

		if (hotbarItems[activeItemIndex] == null)
			return false;
			
		return hotbarItems[activeItemIndex].placeable || hotbarItems[activeItemIndex].usable;
	}

	/// <summary>
	/// Returns the position of the Voxel block the player is looking at when the right mouse button gets clicked.
	/// Returns `null` if nothing was hit.
	/// The w-coordinate of the Vector4 is either 0 or 1 depending on whether the hit point belongs to a top surface.
	/// </summary>
	public static Vector4? GetPlacementCoordinates(out bool hitItem)
	{
		RaycastHit hit;
		bool didHit = Physics.Raycast(Camera.main.ScreenPointToRay((
			Camera.main.pixelWidth / 2,
			Camera.main.pixelHeight / 2,
			0
		).ToVector3()), out hit);

		if (hit.transform.gameObject.GetComponent<ItemObject>() != null)
			hitItem = true;
		else
			hitItem = false;

		if (!didHit)
			return null;

		// Returns the (0,y,0) coordinate of the block that the player is currently looking at.
		Vector3 placementCoordinates = Utils.FloorVector3(hit.point + hit.normal / 2.0f);

		// (x,y,z) correspond to placementCoordinates. The w-coordinate is either 0 or 1 depending on
		// whether the hit point belongs to a top surface.
		Vector4 placementCoordinates4D = placementCoordinates;
		
		if (Vector3.Dot(hit.normal, Vector3.up) == 1)
			placementCoordinates4D.w = 1;
		else
			placementCoordinates4D.w = 0;

		return placementCoordinates4D;
	}

	/// <summary>
	/// Given the item name, instantiates a new item from the registry and adds it to the first available slot in inventory.
	/// Returns the number of items that could be added to the player inventory.
	/// </summary>
	public static int AddItem(string itemName, int quantity = 1)
	{
		InventoryItem[] hotbarItems = InventoryContainers.hotbar.items;
		InventoryItem[] inventoryItems = InventoryContainers.inventory.items;

		(int position, int quantity, bool sameItem) hotbarPlacement = GetPositionForItemInContainer(hotbarItems, itemName, quantity);
		(int position, int quantity, bool sameItem) inventoryPlacement = GetPositionForItemInContainer(inventoryItems, itemName, quantity);

		if (hotbarPlacement.position == -1 && inventoryPlacement.position == -1)
			return 0;

		System.Func<InventoryItem[], int, InventoryItem> NormalizeItem = (InventoryItem[] container, int position) => {
			if (position == -1)
				return null;
			return container[position];
		}; 

		InventoryItem hotbarItem 	= NormalizeItem(hotbarItems, hotbarPlacement.position);
		InventoryItem inventoryItem = NormalizeItem(inventoryItems, inventoryPlacement.position);

		int quantityPlaced = 0;

		/**
		* Placement truth table.
		* Hotbar	Inventory	Result placement
		* null		null		return 0;
		* null		new			inv;
		* null		same		inv;
		* new		null		hot;
		* new		new			hot;
		* new		same		inv;
		* same		null		hot;
		* same		new			hot;
		* same		same		hot;
		*/

		if (hotbarPlacement.position == -1 && inventoryPlacement.position == -1)
			return 0;

		if (
			(hotbarPlacement.position == -1 && inventoryPlacement.position != -1) || 
			(!hotbarPlacement.sameItem && inventoryPlacement.sameItem)
		)
		{
			// Place in inventory
			quantityPlaced = inventoryPlacement.quantity;

			if (!inventoryPlacement.sameItem)
			{
				inventoryItems[inventoryPlacement.position] = new InventoryItem(itemName);
				inventoryItems[inventoryPlacement.position].quantity = quantityPlaced;
			}
			else
				inventoryItems[inventoryPlacement.position].quantity += quantityPlaced;
		}
		else
		{
			// Place in hotbar
			quantityPlaced = hotbarPlacement.quantity;

			if (!hotbarPlacement.sameItem)
			{
				hotbarItems[hotbarPlacement.position] = new InventoryItem(itemName);
				hotbarItems[hotbarPlacement.position].quantity = quantityPlaced;
			}
			else
				hotbarItems[hotbarPlacement.position].quantity += quantityPlaced;
		}

		hotbarRef.UpdateGUI();
		playerInventoryRef.UpdateGUI();

		return quantityPlaced;
	}

	/// <summary>
	/// Determines whether the item identified by itemName can be placed inside the container.
	/// Returns a tuple made of three integers:
	/// 1. The position at which the item can be placed (-1 is the item cannot be placed),
	/// 2. The quantity of items that can be placed,
	/// 3. Whether placement was prioritary or not (same item aggregation).
	/// </summary>
	private static (int,int,bool) GetPositionForItemInContainer(InventoryItem[] container, string itemName, int quantity)
	{
		int firstEmptyPosition = -1;

		int sameItemPosition = -1;
		int quantityPlaced = 0;

		for (int i = 0; i < container.Length; i++)
		{
			InventoryItem item = container[i];

			// Save the first empty position.
			// Keep iterating: if there is another item that shares the same name *AND* 
			// has enough room for at least on more, give priority to aggregating it.
			if (item == null && firstEmptyPosition == -1)
			{
				int maxStack = new InventoryItem(itemName).maxStack;

				firstEmptyPosition = i;
				quantityPlaced = quantity > maxStack ? maxStack : quantity;
				continue;
			}

			// Aggregate here.
			if (IsSameItemPlaceableAt(container, i, itemName))
			{
				sameItemPosition = i;
				
				if (item.quantity + quantity > item.maxStack)
					quantityPlaced = item.maxStack - item.quantity;
				else
					quantityPlaced = quantity;

				break;
			}
		}

		if (sameItemPosition != -1)
			return (sameItemPosition, quantityPlaced, true);
		else
			return (firstEmptyPosition, quantityPlaced, false);
	}

	/// <summary>
	/// Determines whether an item is placeable at the position-th position of the provided container.
	/// </summary>
	private static bool IsSameItemPlaceableAt(InventoryItem[] container, int position, string itemName)
	{
		return 	container[position] != null &&
				container[position].itemName == itemName &&
				container[position].quantity != container[position].maxStack;
	}
}
