using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTableUI : UserInterface
{
    // Start is called before the first frame update
    void Start()
    {
		this.canBeOverlapped = true;

        GUI.craftingTableUI = this;

		this.gameObject.SetActive(false);
    }

	/// <summary>
	/// Updates item slots textures.
	/// </summary>
	public override void UpdateGUI()
	{
		this.GetComponentInChildren<PlayerInventoryItems>().UpdateGUI();
		this.GetComponentInChildren<PlayerInventoryHotbar>().UpdateGUI();
		this.transform.Find("CraftingGrid").GetComponent<InventoryContainer>().UpdateGUI();
		this.transform.Find("CraftingResultSlot").GetComponent<CraftingResultSlot>().UpdateTexture();
	}
}
