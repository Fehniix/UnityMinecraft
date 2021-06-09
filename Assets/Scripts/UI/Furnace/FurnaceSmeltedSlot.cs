using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceSmeltedSlot : SingleInventoryItemSlot
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
}
