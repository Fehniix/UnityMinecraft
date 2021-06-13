public class Planks: Block
{
    public Planks(): base()
	{
		this.blockName 			= "planks";
		this.textureName 		= "oak_planks";
		this.hardness 			= 1 * 20;
		this.burnable 			= true;
		this.burnTime 			= 300;
	}
}
