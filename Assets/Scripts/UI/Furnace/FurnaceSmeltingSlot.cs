using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceSmeltingSlot : SingleInventoryItemSlot
{
	/// <summary>
	/// Reference to the furnace UI.
	/// </summary>
	private FurnaceUI furnaceUI;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

		this.furnaceUI = this.GetComponentInParent<FurnaceUI>();

		this.itemChangedHandlers += this.furnaceUI.TriggerItemUpdate;
    }

	protected override void OnLeftMouseButtonClick()
	{
		IInteractable draggingItem = InventoryContainers.draggingItem?.itemInstance as IInteractable;

		if (this.item != null || draggingItem?.smeltable == true)
			base.OnLeftMouseButtonClick();
	}

	protected override void OnRightMouseButtonClick()
	{
		IInteractable draggingItem = InventoryContainers.draggingItem?.itemInstance as IInteractable;

		if (this.item != null || draggingItem?.smeltable == true)
			base.OnRightMouseButtonClick();
	}
}