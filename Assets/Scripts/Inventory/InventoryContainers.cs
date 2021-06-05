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
}
