using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cobblestone: Block
{
    public Cobblestone(): base()
	{
		this.blockName 		= "cobblestone";
		this.hardness 		= 3 * 20;
		this.smeltable 		= true;
		this.smeltedResult	= new CraftingResult("stone", 1);
		this.burnable 		= true;
		this.burnTime		= 300;
	}
}
