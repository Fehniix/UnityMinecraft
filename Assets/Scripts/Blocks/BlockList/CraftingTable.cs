using UnityEngine;

public class CraftingTable: Block
{
    public CraftingTable(): base()
	{
		this.blockName = "craftingTable";
		this.textureName = "crafting_table";
		this.hasSidedTextures = true;
		this.interactable = true;
		this.hardness = 3 * 20;
	}

	public override void Interact()
	{
		base.Interact();

		CraftingTableUI ui = GUI.craftingTableUI;
		ui.gameObject.SetActive(true);
		GUI.ShowGUIElements();
		// NOTE keep working on this.
	}
}
