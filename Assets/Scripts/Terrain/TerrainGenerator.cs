using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
	/**
	* Terrain is generated based on Simplex & Perlin noise.
	* Based on the "renderDistance" and player position, generate individual chunks and place them in PCTerrain.
	*/

	// Private instance of the FastNoise library by Jordan Peck.
	private FastNoise noise;

	void Start()
	{
		this.GenerateTerrain();
	}

	void Update()
	{
		
	}

	public void GenerateTerrain()
	{
		foreach(var chunk in PCTerrain.GetInstance().chunks)
		{
			chunk.Value.Destroy();
		}

		this.noise = new FastNoise();

		for (int _i = 0; _i < 4; _i++)
			for (int _k = 0; _k < 4; _k++)
			{
				Chunk chunk = new Chunk();
				chunk.x = _i;
				chunk.z = _k;

				for (int i = 0; i < 16; i++)
					for (int j = 0; j < 256; j++)
						for (int k = 0; k < 16; k++)
						{
							chunk.blocks[i,j,k] = this.GenerateBlockType(i + chunk.x * 16, j, k + chunk.z * 16);
						}
	
				chunk.BuildMesh();

				PCTerrain.GetInstance().chunks[(chunk.x, chunk.z)] = chunk;
			}
	}

	private Block GenerateBlockType(int i, int j, int k)
	{
		string blockType = "air";

		float landSimplex1 = this.noise.GetSimplex(
			i * 0.8f, 
			k * 0.8f
		) * 10f;

		float landSimplex2 = this.noise.GetSimplex(
			i * 5f, 
			k * 5f
		) * 10f * (this.noise.GetSimplex(
			i * .5f,
			k * .5f
		) + .3f);

		float baselineLandHeight = 256 * .1f + landSimplex1 + landSimplex2;
		float baselineStoneHeight = 256 * .05f + landSimplex1 + landSimplex2;

		if (j <= baselineLandHeight)
		{
			blockType = "dirt";

			if (j == Mathf.FloorToInt(baselineLandHeight))
				blockType = "grass";
		}

		if (j <= baselineStoneHeight)
			blockType = "stone";

		return Blocks.Instantiate(blockType);
	}
}
