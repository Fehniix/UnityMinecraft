using UnityEngine;
using UnityEngine.UI;

public static class GUI
{
	/// <summary>
	/// Reference to the screen cover item dropper.
	/// </summary>
	public static ScreenCoverItemDropper screenCoverRef;

	/// <summary>
	/// Reference to the dragging item object.
	/// </summary>
	public static DraggingItem draggingItemRef;

	/// <summary>
	/// Reference to the main hotbar object.
	/// </summary>
	public static Hotbar hotbar;

	/// <summary>
	/// Reference to the player inventory UI.
	/// </summary>
	public static PlayerInventoryUI playerInventoryUI;

	/// <summary>
	/// Reference to the crafting table UI.
	/// </summary>
	public static CraftingTableUI craftingTableUI;

	/// <summary>
	/// Reference to the currently active GUI.
	/// </summary>
	public static UserInterface activeGUI;

	/// <summary>
	/// Reference to the GUI that was active before the current one.
	/// Used for GUIs that can be temporarily accessed (active) and give focus back to the previously active one.
	/// </summary>
	public static UserInterface previouslyActiveGUI;

	/// <summary>
	/// Determines whether a GUI is shown to the player or not.
	/// </summary>
	public static bool isAGUIShown {
		get { return activeGUI != null; }
	}

	/// <summary>
	/// Shows the given user interface.
	/// If the `noOverlapOverride` flag is set to `true`, the currently active GUI will not be saved when overlapped. 
	/// </summary>
	private static void ShowBase(UserInterface gui, bool noOverlapOverride = false)
	{
		if (isAGUIShown)
		{
			activeGUI.gameObject.SetActive(false);

			if (gui.canBeOverlapped && !noOverlapOverride)
				previouslyActiveGUI = activeGUI;
		}

		activeGUI = gui;
		activeGUI.gameObject.SetActive(true);
		activeGUI.UpdateGUI();

		ShowGUIElements();
	}

	/// <summary>
	/// Hides the given user interface.
	/// </summary>
	private static void HideBase(UserInterface gui)
	{
		if (!isAGUIShown)
			return;

		if (activeGUI.canBeOverlapped)
			ShowBase(previouslyActiveGUI, true);
		else {
			activeGUI.gameObject.SetActive(false);
			activeGUI = null;

			HideGUIElements();
		}
	}

	/// <summary>
	/// Shows the player inventory.
	/// </summary>
	public static void ShowPlayerInventory()
	{
		ShowBase(playerInventoryUI);
	}

	/// <summary>
	/// Hides the player inventory.
	/// </summary>
	public static void HidePlayerInventory()
	{
		HideBase(playerInventoryUI);
	}

	/// <summary>
	/// Shows the crafting table UI.
	/// </summary>
	public static void ShowCraftingTableUI()
	{
		ShowBase(craftingTableUI);
	}

	/// <summary>
	/// Hides the crafting table UI.
	/// </summary>
	public static void HideCraftingTableUI()
	{
		HideBase(craftingTableUI);
	}

	/// <summary>
	/// Shows the screen cover & dragging item.
	/// </summary>
	public static void ShowGUIElements()
	{
		screenCoverRef.gameObject.SetActive(true);
		GUI.hotbar.gameObject.SetActive(false);
		
		Cursor.lockState 	= CursorLockMode.None;
		Cursor.visible		= true;
	}

	/// <summary>
	/// Hides the screen cover & dragging item.
	/// </summary>
	public static void HideGUIElements()
	{
		screenCoverRef.gameObject.SetActive(false);

		GUI.hotbar.gameObject.SetActive(true);
		GUI.hotbar.UpdateGUI();
		
		Cursor.lockState 	= CursorLockMode.Locked;
		Cursor.visible		= false;
	}
}