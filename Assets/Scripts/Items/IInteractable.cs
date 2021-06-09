/// <summary>
/// An interactable object can be placed, broken and interacted with.
/// </summary>
public interface IInteractable
{
	/// <summary>
	/// Whether the object is interactable (Interact() available) or not.
	/// </summary>
	bool interactable { get; set; }

	/// <summary>
	/// Whether the item can be smelted or not.
	/// </summary>
	bool smeltable { get; set; }

	CraftingResult? smeltedResult { get; set; }

	bool burnable { get; set; }

	/// <summary>
	/// The number of ticks it takes to deplete the item used as a fuel source.
	/// </summary>
	int burnTime { get; set; }

	void Place();

	void Break();

	void Interact();
}