public class IronBlock: Block
{
    public IronBlock(): base()
	{
		this.blockName 			= "ironBlock";
		this.textureName	 	= "iron_block";
		this.hardness 			= 2 * 20;
		this.maxStack 			= 64;
		this.toolTypeRequired 	= ToolType.PICKAXE;
		this.miningLevel 		= MiningLevel.IRON;
	}
}
