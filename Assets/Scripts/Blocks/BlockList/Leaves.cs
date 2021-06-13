using System.Collections.Generic;

public class Leaves: Block
{
    public Leaves(): base()
	{
		this.blockName 			= "leaves";
		this.textureName 		= "oak_leaves";
		this.hardness 			= 1 * 15;
		this.dropsItself		= false;
		
		this.drops = new List<Drop>();
		this.drops.Add(new Drop("stick", (int)new System.Random().NextDouble() * 4, 1.0f));
	}
}
