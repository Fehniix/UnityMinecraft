using System.Collections.Generic;

public class OreDiamond: Block
{
    public OreDiamond(): base()
	{
		this.blockName 			= "oreDiamond";
		this.textureName 		= "diamond_ore";
		this.hardness 			= 2 * 20;
		this.maxStack 			= 64;
		this.dropsItself 		= false;
		this.toolTypeRequired 	= ToolType.PICKAXE;
		this.miningLevel		= 3;
		
		this.drops = new List<Drop>();
		this.drops.Add(new Drop("diamond", UnityEngine.Random.Range(1, 2), 1.0f));
	}
}
