using UnityEngine;

/// <summary>
/// Describes a GUI.
/// </summary>
public class UserInterface: MonoBehaviour
{
	/// <summary>
	/// Determines whether the GUI can be "overlapped" to other GUIs.
	/// Overlapping: the currently active GUI gets stored & hidden; the overlapping GUI is shown and, when finally closed,
	/// the stored & hidden GUI gets shown back to the user.
	/// </summary>
	public bool canBeOverlapped = false;

	/// <summary>
	/// Updates all textures.
	/// </summary>
	public virtual void UpdateGUI() {}
}