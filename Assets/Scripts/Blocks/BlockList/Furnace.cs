﻿public class Furnace: Block
{
    public Furnace(): base()
	{
		this.blockName 			= "furnace";
		this.hasSidedTextures 	= true;
		this.interactable 		= true;
		this.hardness 			= 3 * 20;
	}

	public override void Interact()
	{
		base.Interact();

		GUI.ShowFurnaceUI();
	}
}