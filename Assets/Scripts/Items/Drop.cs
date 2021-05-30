/// <summary>
/// Contains an item to drop at a specific probability.
/// </summary>
public struct Drop
{
	/// <summary>
	/// The name of the item to drop.
	/// </summary>
	public string itemName;

	/// <summary>
	/// The number of registry items to drop.
	/// </summary>
	public int quantity;

	/// <summary>
	/// The probability at which the item is dropped.
	/// </summary>
    public float probability;

	/// <summary>
	/// The registry item (either Item or Block) to drop.
	/// </summary>
	public RegistryItem itemToDrop;
}
