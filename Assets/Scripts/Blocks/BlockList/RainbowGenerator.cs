public class RainbowGenerator: Block
{
    public RainbowGenerator(): base()
	{
		this.blockName 			= "rainbowGenerator";
		this.textureName 		= "rainbow_generator";
		this.hardness 			= 1 * 20;
		this.toolTypeRequired	= ToolType.PICKAXE;
		this.miningLevel		= MiningLevel.DIAMOND;
	}

	public override void Place()
	{
		base.Place();

		UnityEngine.Debug.Log("Game won!");
	}
}
