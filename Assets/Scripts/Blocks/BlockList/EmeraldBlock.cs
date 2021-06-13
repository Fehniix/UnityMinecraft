public class EmeraldBlock: Block
{
    public EmeraldBlock(): base()
	{
		this.blockName 			= "emeraldBlock";
		this.textureName	 	= "emerald_block";
		this.hardness 			= 2 * 20;
		this.maxStack 			= 64;
		this.toolTypeRequired 	= ToolType.PICKAXE;
		this.miningLevel 		= MiningLevel.DIAMOND;
	}
}
