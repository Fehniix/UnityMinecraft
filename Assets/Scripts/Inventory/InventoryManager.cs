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
	/// </summary>
	public static bool AddItem(string itemName, int quantity = 1)
	{
		InventoryItem[] hotbarItems = InventoryContainers.hotbar.items;
		InventoryItem[] inventoryItems = InventoryContainers.inventory.items;

		int hotbarPosition = GetHotbarPositionForItem(itemName);
		int inventoryPosition = GetInventoryPositionForItem(itemName);

		if (hotbarPosition == -1 && inventoryPosition == -1)
			return false;

		InventoryItem hotbarItem = hotbarItems[hotbarPosition];
		InventoryItem inventoryItem = inventoryItems[inventoryPosition];

		// The highest placement priority is given to any slot that already has the item.
		if ((hotbarItem?.quantity < hotbarItem?.maxStack) || (inventoryItem?.quantity < inventoryItem?.maxStack))
		{
			if (hotbarItems[hotbarPosition] != null)
				hotbarItems[hotbarPosition].quantity++;
			else
				inventoryItems[inventoryPosition].quantity++;
		}
		else
		{
			InventoryItem item = new InventoryItem(itemName);
			
			if (hotbarPosition != -1)
				hotbarItems[hotbarPosition] = item;
			else
				inventoryItems[inventoryPosition] = item;
		}

		hotbarRef.UpdateGUI();
		playerInventoryRef.UpdateGUI();

		return true;
	}

	/// <summary>
	/// Determines whether the item identified by itemName can be placed inside the hotbar.
	/// Returns the position at which the item can be first placed, `-1` if the item cannot be placed.
	/// </summary>
	private static int GetHotbarPositionForItem(string itemName)
	{
		InventoryItem[] hotbarItems = InventoryContainers.hotbar.items;

		int firstAvailable = -1;

		for (int i = 0; i < 9; i++)
		{
			// Save first empty position.
			if (hotbarItems[i] == null && firstAvailable == -1)
			{
				firstAvailable = i;
				continue;
			}

			// Keep iterating: if there is a slot that already has the item in question, adding to it has priority.
			if (hotbarItems[i] != null && hotbarItems[i].itemName == itemName && hotbarItems[i].quantity < hotbarItems[i].maxStack)
				return i;
		}

		return firstAvailable;
	}

	/// <summary>
	/// Determines whether the item identified by itemName can be placed inside the inventory.
	/// Returns the position at which the item can be first placed, `-1` if the item cannot be placed.
	/// </summary>
	private static int GetInventoryPositionForItem(string itemName)
	{
		InventoryItem[] hotbarItems = InventoryContainers.hotbar.items;
		InventoryItem[] inventoryItems = InventoryContainers.inventory.items;

		int firstAvailable = -1;

		for (int i = 0; i < 27; i++)
		{
			// Save first empty position.
			if (inventoryItems[i] == null && firstAvailable == -1)
			{
				firstAvailable = i;
				continue;
			}

			// Keep iterating: if there is a slot that already has the item in question, adding to it has priority.
			if (inventoryItems[i] != null && inventoryItems[i].itemName == itemName && inventoryItems[i].quantity < inventoryItems[i].maxStack)
				return i;
		}

		return firstAvailable;
	}
}
