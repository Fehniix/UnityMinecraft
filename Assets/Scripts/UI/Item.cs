using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
	public Item(string name)
	{
		this.itemName = name;
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
}
