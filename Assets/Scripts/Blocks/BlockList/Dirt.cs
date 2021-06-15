public class Dirt: Block
{
    public Dirt(): base()
	{
		this.blockName = "dirt";
		this.hardness = 2 * 20;
		this.maxStack = 64;
		this.soundType = BlockSoundType.DIRT;
	}
}
