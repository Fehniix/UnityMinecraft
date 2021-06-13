public class DiamondBlock: Block
{
    public DiamondBlock(): base()
	{
		this.blockName 			= "diamondBlock";
		this.textureName	 	= "diamond_block";
		this.hardness 			= 2 * 20;
		this.maxStack 			= 64;
		this.toolTypeRequired 	= ToolType.PICKAXE;
		this.miningLevel 		= MiningLevel.DIAMOND;
	}
}
