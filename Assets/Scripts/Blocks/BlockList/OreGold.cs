public class OreGold: Block
{
    public OreGold(): base()
	{
		this.blockName 			= "oreGold";
		this.textureName 		= "gold_ore";
		this.hardness 			= 2 * 20;
		this.maxStack 			= 64;
		this.dropsItself 		= true;
		this.smeltable 			= true;
		this.smeltedResult 		= new CraftingResult("goldIngot", 1);
		this.toolTypeRequired 	= ToolType.PICKAXE;
		this.miningLevel		= MiningLevel.IRON;
	}
}
