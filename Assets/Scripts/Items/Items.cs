using System;
using System.Collections.Generic;

public static class Items
{
	/// <summary>
	/// For each dictionary entry, contains the name of the item & the type class associated with it.
	/// </summary>
	private static Dictionary<string, Type> registeredItems = new Dictionary<string, Type>();

	/// <summary>
	/// Registers an item to the local list of available items.
	/// Items can be then retrieved and instantiated.
	/// </summary>
	public static void RegisterItem<T>(string itemName)
	{
		Items.registeredItems[itemName] = typeof(T);
	}

	/// <summary>
	/// Given an item name, returns an Item instance.
	/// </summary>
	public static Item Instantiate(string itemName)
	{
		if (!Items.registeredItems.ContainsKey(itemName))
			return null;

		return (Item)Activator.CreateInstance(Items.registeredItems[itemName]);
	}
}