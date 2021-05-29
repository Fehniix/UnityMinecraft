using System;

public struct RegistryItem
{
	/// <summary>
	/// Contains the type of the registry item.
	/// </summary>
    public Type itemType;

	/// <summary>
	/// Determines whether registry item is an Item or a Block.
	/// </summary>
	public bool IsBlock()
	{
		return this.itemType == typeof(Block);
	}
}
