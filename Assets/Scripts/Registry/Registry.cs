using System;
using System.Collections.Generic;

/// <summary>
/// Contains informations about registered blocks and items.
/// </summary>
public static class Registry
{
	/// <summary>
	/// For each dictionary entry, contains the name of the registry item and the item itself.
	/// </summary>
	private static Dictionary<string, RegistryItem> registryItems = new Dictionary<string, RegistryItem>();

	/// <summary>
	/// Used to create a one-to-one relationship between block/items and their name.
	/// I am assuming no more than 256 blocks will be implemented - System.Byte is thus sufficient.
	/// </summary>
	private static byte incrementalID = 0;

	/// <summary>
	/// Dictionary of item name -> ID associations. Heart of the 1-1 relationship.
	/// </summary>
	private static Dictionary<string, byte> itemIDs = new Dictionary<string, byte>();

	

	/// <summary>
	/// Given an Item or Block name and its explicit type, it registers the string-Type association.
	/// </summary>
	public static void RegisterItem<T>(string itemName)
	{
		RegistryItem item = new RegistryItem();
		item.itemType = typeof(T);

		registryItems[itemName] = item;
	}

	/// <summary>
	/// Given a registered item name, returns an object instance of the stored type. Can be safely casted to either Block or Item.
	/// </summary>
	public static object Instantiate(string itemName)
	{
		if (itemName == null || !registryItems.ContainsKey(itemName))
			return null;

		return Activator.CreateInstance(registryItems[itemName].itemType);
	}

	public static bool IsBlock(string itemName)
	{
		if (!registryItems.ContainsKey(itemName))
			return false;

		return Instantiate(itemName) is Block;
	}
}