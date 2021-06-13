public class OreCoal: Block
{
    public OreCoal(): base()
	{
		this.blockName = "oreCoal";
		this.textureName = "coal_ore";
		this.hardness = 2 * 20;
		this.maxStack = 64;
	}
}
