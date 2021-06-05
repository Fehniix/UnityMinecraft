using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Contains static references to all inventory containers throughout the game.
/// </summary>
public static class InventoryContainers
{
	/// <summary>
	/// The inventory containers. Registered and accessible by UUID.
	/// </summary>
    public static Dictionary<string, InventoryContainer> containers = new Dictionary<string, InventoryContainer>();

	/// <summary>
	/// The item reference that is currently being dragged.
	/// </summary>
	public static InventoryItem draggingItem;

	/// <summary>
	/// Reference to the item being dragged.
	/// </summary>
	public static UnityEngine.GameObject draggingItemObject;
}
