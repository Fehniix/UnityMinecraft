/// <summary>
/// An interactable object can be placed, broken and interacted with.
/// </summary>
public interface IInteractable
{
	/// <summary>
	/// Whether the object is interactable (Interact() available) or not.
	/// </summary>
	bool interactable { get; set; }

	void Place();

	void Break();

	void Interact();
}