public class GoldBlock: Block
{
    public GoldBlock(): base()
	{
		this.blockName 			= "goldBlock";
		this.textureName	 	= "gold_block";
		this.hardness 			= 2 * 20;
		this.maxStack 			= 64;
		this.toolTypeRequired 	= ToolType.PICKAXE;
		this.miningLevel 		= MiningLevel.IRON;
	}
}
