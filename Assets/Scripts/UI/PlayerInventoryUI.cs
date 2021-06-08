using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInventoryUI : UserInterface
{
	/// <summary>
	/// Reference to the crafting container. 
	/// Set in Unity Editor.
	/// </summary>
	public InventoryContainer craftingGrid;

	/// <summary>
	/// Reference to the crafting result object. 
	/// Set in Unity Editor.
	/// </summary>
	public GameObject craftingResultObject;

    void Start()
    {
		this.canBeOverlapped = false;

		GUI.playerInventoryUI = this;
		
		this.gameObject.SetActive(false);
    }

	/// <summary>
	/// Updates the inventory container GUI.
	/// </summary>
	public override void UpdateGUI()
	{
		InventoryContainers.hotbar.UpdateGUI();
		InventoryContainers.inventory.UpdateGUI();
		this.craftingGrid.UpdateGUI();
	}
}
