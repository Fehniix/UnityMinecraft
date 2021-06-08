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

	public Drop(string itemName, int quantity, float probability)
	{
		this.itemName = itemName;
		this.quantity = quantity;
		this.probability = probability;
	}
}
