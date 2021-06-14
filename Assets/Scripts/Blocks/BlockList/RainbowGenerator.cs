using UnityEngine;

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

	public override Vector3? Place()
	{
		Vector3? placementCoords = base.Place();

		if (placementCoords == null)
			return null;

		ChunkPosition position 		= Player.instance.GetVoxelChunk();
		Chunk chunk 				= PCTerrain.GetInstance().chunks[position];
		AudioSource source			= AudioManager.Create3DSound("challenge_complete");
		
		source.gameObject.transform.parent 			= chunk.chunkGameObject.transform;
		source.gameObject.transform.localPosition 	= placementCoords.Value;

		source.Play();

		return placementCoords;
	}
}
