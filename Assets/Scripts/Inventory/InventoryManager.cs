using UnityEngine;
using Extensions;

public static class InventoryManager
{
	/// <summary>
	/// The hotbar items.
	/// </summary>
	public static InventoryItem[] hotbarItems;

	/// <summary>
	/// Reference to the inventory items.
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
	/// Tries to place or use the currently active item.
	/// </summary>
	public static bool ConsumeActive()
	{
		InventoryItem activeItem = hotbarItems[activeItemIndex];

		if (!activeItem.placeable)
			return false;

		Block block = Registry.Instantiate(activeItem.itemName) as Block;

		if (!block.placeable)
			return false;

		PlaceBlock();

		hotbarItems[activeItemIndex].quantity--;
		if (hotbarItems[activeItemIndex].quantity == 0)
			hotbarItems[activeItemIndex] = null;

		InventoryManager.hotbarRef.UpdateItems();
		
		return true;
	}

	/// <summary>
	/// Determines whether the currently active item is consumable (usable or placeable) or not.
	/// </summary>
	public static bool IsActiveItemConsumable()
	{
		if (hotbarItems[activeItemIndex] == null)
			return false;
			
		return hotbarItems[activeItemIndex].placeable || hotbarItems[activeItemIndex].usable;
	}

	/// <summary>
	/// Allows the player to place a placeable block from the currently active item in the hotbar.
	/// </summary>
	public static void PlaceBlock()
	{
		// This process would have to first get the active item in the hotbar, check whether it's placeable and onl then place it.
		string blockName = hotbarItems[activeItemIndex].itemName;

		RaycastHit hit;
		bool didHit = Physics.Raycast(Camera.main.ScreenPointToRay((
			Camera.main.pixelWidth / 2,
			Camera.main.pixelHeight / 2,
			0
		).ToVector3()), out hit);

		if (!didHit)
			return;
		
		Vector3Int placingBlockCoordinates = Utils.ToVectorInt(hit.point + hit.normal / 2.0f);
		Vector3Int playerPosition = Player.instance.GetVoxelPosition();

		if (
			placingBlockCoordinates == playerPosition || 
			placingBlockCoordinates == new Vector3Int(playerPosition.x, playerPosition.y + 1, playerPosition.z)
		)
			return;
		
		PCTerrain.GetInstance().PlaceAt(blockName, placingBlockCoordinates);
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
		}

		//TODO implement inventory.

		hotbarRef.UpdateItems();
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
