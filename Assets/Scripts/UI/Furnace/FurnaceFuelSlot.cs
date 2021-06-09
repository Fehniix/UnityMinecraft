using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FurnaceFuelSlot : SingleInventoryItemSlot
{
	/// <summary>
	/// Reference to the furnace UI.
	/// </summary>
	private FurnaceUI furnaceUI;

    // Start is called before the first frame update
    override protected void Start()
    {
		base.Start();

		this.furnaceUI = this.GetComponentInParent<FurnaceUI>();

		this.itemChangedHandlers += this.furnaceUI.TriggerItemUpdate;
    }

	protected override void OnLeftMouseButtonClick()
	{
		IInteractable draggingItem = InventoryContainers.draggingItem?.itemInstance as IInteractable;

		if (this.item != null || draggingItem?.burnable == true)
			base.OnLeftMouseButtonClick();
	}

	protected override void OnRightMouseButtonClick()
	{
		IInteractable draggingItem = InventoryContainers.draggingItem?.itemInstance as IInteractable;

		if (this.item != null || draggingItem?.burnable == true)
			base.OnRightMouseButtonClick();
	}
}
