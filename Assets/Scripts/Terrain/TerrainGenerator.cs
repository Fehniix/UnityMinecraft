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
		this.noise = new FastNoise();

		Chunk test = new Chunk();

		// for (int i = 0; i < 256; i++)
		// 	test.blocks[Random.Range(0, 16), Random.Range(0, 32), Random.Range(0, 16)] = "stone";
		test.blocks[1,1,1] = "cobblestone";
		test.blocks[2,2,1] = "bedrock";
		
		test.x = 0;
		test.z = 0;
		test.BuildMesh();

		PCTerrain.GetInstance().chunks[(test.x, test.z)] = test;
	}

	void Update()
	{
		
	}
}
