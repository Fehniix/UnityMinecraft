public struct CraftingResult 
{
	/// <summary>
	/// The item of the crafting recipe result
	/// </summary>
	public string itemName;

	/// <summary>
	/// The quantity of items that the crafting recipe produces
	/// </summary>
	public int quantity;

	public CraftingResult(string itemName, int quantity)
	{
		this.itemName = itemName;
		this.quantity = quantity;
	}

	public override string ToString()
	{
		return System.String.Format("CraftingResult[{0}, x{1}]", this.itemName, this.quantity);
	}
}