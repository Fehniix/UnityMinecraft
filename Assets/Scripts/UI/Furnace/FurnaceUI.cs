using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceUI : UserInterface
{
	/// <summary>
	/// Reference to the fuel progress component.
	/// </summary>
	public Progress fuelProgress;

	/// <summary>
	/// Reference to the item smelting progress.
	/// </summary>
	public Progress smeltingProgress;

	/// <summary>
	/// Whether the furnace is actively smelting something or not.
	/// </summary>
	public bool isSmelting = false;

	/// <summary>
	/// The time it takes for a single smelting operation.
	/// </summary>
	public int singleOperationTime {
		get { return 100; }
	}

	/// <summary>
	/// The fuel item.
	/// </summary>
	private FurnaceFuelSlot fuelSlot;

	/// <summary>
	/// The item being smelted.
	/// </summary>
	private FurnaceSmeltingSlot smeltingSlot;

	/// <summary>
	/// The resulting smelted item.
	/// </summary>
	private FurnaceSmeltedSlot smeltedSlot;

	/// <summary>
	/// Determines the current smelting progress.
	/// </summary>
	private int smeltingTicksElapsed = 0;

	/// <summary>
	/// The initial fuel value. Used to determine the current burning progress.
	/// </summary>
	private int initialFuelValue = 0;

	/// <summary>
	/// The amount of fuel remaining.
	/// </summary>
	private int remainingFuel = 0;

	/// <summary>
	/// Returns the current burning progress as a fuelRemaining / initialFuelValue proportion.
	/// The number returned stands between 0 and 100, both ends included.
	/// </summary>
	private int fuelPercentage {
		get 
		{
			if (this.initialFuelValue == 0)
				return 0;

			return (int)(this.remainingFuel / (float)this.initialFuelValue * 100); 
		}
	}

	/// <summary>
	/// Returns the current smelting progress as a smeltingTicksElapsed / singleOperationTime proportion.
	/// The number returned stands between 0 and 100, both ends included.
	/// </summary>
	private int smeltingPercentage {
		get 
		{ 
			if (this.singleOperationTime == 0)
				return 0;

			return (int)(this.smeltingTicksElapsed / (float)this.singleOperationTime * 100); 
		}
	}

	/// <summary>
	/// Retrieves the IInteractable instance from the fuelSlot.
	/// </summary>
	private IInteractable fuelItem {
		get { return this.fuelSlot.item?.itemInstance as IInteractable; }
	}

	/// <summary>
	/// Retrieves the IInteractable instance from the smeltingSlot.
	/// </summary>
	private IInteractable smeltingItem {
		get { return this.smeltingSlot.item?.itemInstance as IInteractable; }
	}

    // Start is called before the first frame update
    void Start()
    {
		this.fuelProgress 		= this.transform.Find("BurningIconMask").GetComponent<Progress>();
		this.smeltingProgress 	= this.transform.Find("ProgressIconMask").GetComponent<Progress>();
		
		this.fuelSlot 			= this.GetComponentInChildren<FurnaceFuelSlot>();
		this.smeltingSlot		= this.GetComponentInChildren<FurnaceSmeltingSlot>();
		this.smeltedSlot		= this.GetComponentInChildren<FurnaceSmeltedSlot>();

        GUI.furnaceUI 			= this;
		Clock.instance.AddTickDelegate(this.ClockTicked);

		this.gameObject.SetActive(false);
    }

	/// <summary>
	/// Allows the furnace UI to get noticed about an item update within it.
	/// </summary>
	public void TriggerItemUpdate()
	{
		bool isFuelBurnable 	= this.fuelItem?.burnable == true;
		bool isItemSmeltable 	= this.smeltingItem?.smeltable == true;
			
		if (isFuelBurnable && isItemSmeltable && this.remainingFuel == 0)
			this.ConsumeFuelItem();

		if (!isItemSmeltable)
			this.ResetSmeltingProgress();

		if ((isFuelBurnable && isItemSmeltable) || (isItemSmeltable && this.remainingFuel > 0))
			this.isSmelting = true;
	}

	/// <summary>
	/// Called by the clock every 1/20th of a second.
	/// </summary>
	private void ClockTicked()
	{
		if (this.remainingFuel > 0)
		{
			// Keep consuming fuel no matter whether we're smelting an item or not.
			this.remainingFuel--;
			this.UpdateProgressElementsUI();
		}
		else if (this.fuelSlot.item != null && this.fuelSlot.item.quantity > 0 && this.isSmelting)
			// We're smelting an item. Previous fuel source got depleted, let's use another one.
			this.ConsumeFuelItem();
		else
			// Fuel is equal to zero, there's no more burnable items usable in the fuel slot:
			this.isSmelting = false;

		if (!this.isSmelting)
			return;

		this.smeltingTicksElapsed++;
		
		if (this.smeltingTicksElapsed >= this.singleOperationTime)
		{
			this.isSmelting = false;

			this.smeltingSlot.item.quantity--;
			
			if (this.smeltedSlot.item == null)
			{
				this.smeltedSlot.item = new InventoryItem(this.smeltingItem.smeltedResult.Value.itemName);
				this.smeltedSlot.item.quantity = this.smeltingItem.smeltedResult.Value.quantity;
			}
			else
				this.smeltedSlot.item.quantity++;

			this.ResetSmeltingProgress();
			this.UpdateGUI();

			if (this.smeltingSlot.item?.quantity > 0 && this.smeltedSlot.item?.quantity < this.smeltedSlot.item?.maxStack)
				this.isSmelting = true;
		}
	}

	/// <summary>
	/// Consumes the fuel item.
	/// </summary>
	private void ConsumeFuelItem()
	{
		this.initialFuelValue 	= this.fuelItem.burnTime;
		this.remainingFuel		= this.fuelItem.burnTime;

		this.fuelSlot.item.quantity--;
		this.fuelSlot.UpdateTexture();
	}

	/// <summary>
	/// Updates the progress elements' UIs.
	/// </summary>
	public void UpdateProgressElementsUI()
	{
		this.fuelProgress.UpdateProgress(this.fuelPercentage);
		this.smeltingProgress.UpdateProgress(this.smeltingPercentage);
	}

	/// <summary>
	/// Resets the current progress.
	/// </summary>
	private void ResetSmeltingProgress()
	{
		this.isSmelting = false;
		this.smeltingTicksElapsed = 0;
	}

	/// <summary>
	/// Updates item slots textures.
	/// </summary>
	public override void UpdateGUI()
	{
		this.GetComponentInChildren<PlayerInventoryItems>().UpdateGUI();
		this.GetComponentInChildren<PlayerInventoryHotbar>().UpdateGUI();
		this.fuelSlot.UpdateTexture();
		this.smeltingSlot.UpdateTexture();
		this.smeltedSlot.UpdateTexture();
		this.UpdateProgressElementsUI();
	}
}
