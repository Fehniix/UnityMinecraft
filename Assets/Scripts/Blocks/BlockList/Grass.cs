﻿public class Grass: Block
{
    public Grass(): base()
	{
		this.blockName = "grass";
		this.textureName = "grass_block";
		this.hardness = 3 * 20;
		this.hasSidedTextures = true;
	}
}