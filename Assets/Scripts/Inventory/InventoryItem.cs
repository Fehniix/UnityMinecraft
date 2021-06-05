using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
	public InventoryItem(string name)
	{
		this.itemName = name;

		if (Registry.IsBlock(itemName))
		{
			Block block 	= Registry.Instantiate(itemName) as Block;
			this.isBlock 	= true;
			this.placeable 	= block.placeable;
			this.maxStack	= block.maxStack;
		}
		else
		{
			Item item		= Registry.Instantiate(itemName) as Item;
			this.isBlock	= false;
			this.placeable	= item.placeable;
			this.maxStack	= item.maxStack;
		}
	}

	/// <summary>
	/// The name of the item.
	/// </summary>
	public string itemName = "default";

	/// <summary>
	/// Determines whether the item is a placeable block or not.
	/// </summary>
    public bool placeable = false;

	/// <summary>
	/// Determines whether the item is usable or not.
	/// </summary>
	public bool usable = false;

	/// <summary>
	/// Determines whether the item is a block or not.
	/// </summary>
	public bool isBlock = false;

	/// <summary>
	/// The number of items.
	/// </summary>
	public int quantity = 1;

	/// <summary>
	/// Maximum amount of items that can be aggregated in a single item slot.
	/// </summary>
	public int maxStack = 64;
}
