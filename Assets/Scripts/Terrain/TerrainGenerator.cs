using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

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
							BaseBlock block = this.GenerateTerrainBlockType(i + chunk.x * 16, j, k + chunk.z * 16);
							chunk.blocks[i,j,k] = block;

							if (chunk.blocks[i,j,k].stateful)
								PCTerrain.GetInstance().blocks[(i,j,k).ToVector3Int()] = Registry.Instantiate(block.blockName) as Block;
						}
	
				chunk.BuildMesh();

				PCTerrain.GetInstance().chunks[(chunk.x, chunk.z)] = chunk;
			}
	}

	private BaseBlock GenerateTerrainBlockType(int i, int j, int k)
	{
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

		float stoneSimplex1 = this.noise.GetSimplex(
			i * 2f,
			k * 2f
		) * 10;

		float stoneSimplex2 = (this.noise.GetSimplex(
			i * 5.1f,
			k * 5.1f
		) + .7f) * 20 * this.noise.GetSimplex(
			i * .2f,
			k * .2f
		);

		float caveFractal = this.noise.GetPerlinFractal(
			i * 5f,
			j * 3f,
			k * 5f
		) * 1.1f;

		float caveFractalMask = this.noise.GetSimplex(
			i * .4f,
			k * .4f	
		) * .3f;

		float baselineLandHeight = 256 * 0.1f + landSimplex1 + landSimplex2;
		float baselineStoneHeight = 256 * 0.04f + stoneSimplex1 + stoneSimplex1;
		float baselineCaveHeight = 256 * 0.066f;

		string blockType = "air";

		if (j <= baselineLandHeight)
		{
			blockType = "dirt";

			if (j == Mathf.FloorToInt(baselineLandHeight))
				blockType = "grass";
		}

		if (j <= baselineStoneHeight)
			blockType = "stone";

		if (caveFractal > Mathf.Max(.2f, caveFractalMask) && j <= baselineCaveHeight)
			blockType = "air";

		if (j <= 2)
			blockType = "bedrock";

		return Registry.Instantiate(blockType) as BaseBlock;
	}
}
