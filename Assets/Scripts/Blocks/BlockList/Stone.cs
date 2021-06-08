using System.Collections.Generic;

public class Stone: Block
{
    public Stone(): base()
	{
		this.blockName 		= "stone";
		this.hardness 		= 3 * 20;
		this.dropsItself 	= false;

		this.drops = new List<Drop>();
		this.drops.Add(new Drop("cobblestone", 1, 1.0f));
	}
}
