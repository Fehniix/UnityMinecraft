using UnityEngine;
using Extensions;

public static class InventoryManager
{
	/// <summary>
	/// The hotbar items.
	/// </summary>
	public static Item[] hotbarItems;

	/// <summary>
	/// Represents the index of the currently active item.
	/// </summary>
	public static int activeItemIndex;

	/// <summary>
	/// Tries to place or use the currently active item.
	/// </summary>
	public static bool Consume()
	{
		Item activeItem = hotbarItems[activeItemIndex];

		if (!activeItem.placeable)
			return false;

		Block block = Blocks.Instantiate(activeItem.itemName);

		if (!block.placeable)
			return false;

		PlaceBlock();

		hotbarItems[activeItemIndex] = null;
		
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
}
