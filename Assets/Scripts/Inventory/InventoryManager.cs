using UnityEngine;
using Extensions;

public static class InventoryManager
{
	/// <summary>
	/// The hotbar items.
	/// </summary>
	public static InventoryItem[] hotbarItems;

	/// <summary>
	/// Reference to the inventory items. Order matters.
	/// </summary>
	public static InventoryItem[] inventoryItems;

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
	public static Inventory inventoryRef;

	/// <summary>
	/// Tries to place or use the currently active item.
	/// </summary>
	public static bool ConsumeActive()
	{
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

		InventoryManager.hotbarRef.UpdateHotbarItems();

		return true;
	}

	/// <summary>
	/// Determines whether the currently active item is consumable (usable or placeable) or not.
	/// </summary>
	public static bool IsActiveItemConsumable()
	{
		Debug.Log(hotbarItems[activeItemIndex].placeable + " " + hotbarItems[activeItemIndex].usable);
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
	public static void AddItem(string itemName, int quantity = 1)
	{
		int hotbarPosition = GetHotbarPositionForItem(itemName);

		if (hotbarPosition != -1)
		{
			if (hotbarItems[hotbarPosition] == null)
				hotbarItems[hotbarPosition] = new InventoryItem(itemName);
			else
				hotbarItems[hotbarPosition].quantity++;

			hotbarRef.UpdateHotbarItems();
		}

		//TODO implement inventory.
	}

	/// <summary>
	/// Determines whether the item identified by itemName can be placed inside the hotbar.
	/// Returns the position at which the item can be first placed, `-1` if the item cannot be placed.
	/// </summary>
	private static int GetHotbarPositionForItem(string itemName)
	{
		int firstAvailable = -1;

		for (int i = 0; i < 9; i++)
		{
			if (hotbarItems[i] == null && firstAvailable == -1)
			{
				firstAvailable = i;
				continue;
			}

			if (hotbarItems[i] != null && hotbarItems[i].itemName == itemName)
				return i;
		}

		return firstAvailable;
	}
}
