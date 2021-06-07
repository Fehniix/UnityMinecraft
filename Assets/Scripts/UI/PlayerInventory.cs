using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInventory : MonoBehaviour
{
	/// <summary>
	/// Reference to the crafting container. Set in Editor.
	/// </summary>
	public InventoryContainer craftingGrid;

	/// <summary>
	/// Reference to the crafting result object. Set in Editor.
	/// </summary>
	public GameObject craftingResultObject;

    void Start()
    {
		PlayerInventoryManager.playerInventoryRef = this;
		
		this.gameObject.SetActive(false);
    }

	/// <summary>
	/// Updates the inventory container GUI.
	/// </summary>
	public void UpdateGUI()
	{
		InventoryContainers.hotbar.UpdateGUI();
		InventoryContainers.inventory.UpdateGUI();
	}
}
