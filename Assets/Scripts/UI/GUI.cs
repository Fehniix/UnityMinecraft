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
	/// Reference to the crafting table UI.
	/// </summary>
	public static CraftingTableUI craftingTableUI;

	/// <summary>
	/// Shows the screen cover & dragging item.
	/// </summary>
	public static void ShowGUIElements()
	{
		GameState.inventoryOpen = true;
		screenCoverRef.gameObject.SetActive(true);
		PlayerInventoryManager.hotbarRef.gameObject.SetActive(false);
		
		Cursor.lockState 	= CursorLockMode.None;
		Cursor.visible		= true;
	}

	/// <summary>
	/// Hides the screen cover & dragging item.
	/// </summary>
	public static void HideGUIElements()
	{
		GameState.inventoryOpen = false;
		screenCoverRef.gameObject.SetActive(false);

		PlayerInventoryManager.hotbarRef.gameObject.SetActive(true);
		PlayerInventoryManager.hotbarRef.UpdateGUI();
		
		Cursor.lockState 	= CursorLockMode.Locked;
		Cursor.visible		= false;
	}
}