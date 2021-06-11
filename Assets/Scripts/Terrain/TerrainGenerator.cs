using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System.Threading;

public class TerrainGenerator : MonoBehaviour
{
	/**
	* Terrain is generated based on Simplex & Perlin noise.
	* Based on the "renderDistance" and player position, generate individual chunks and place them in PCTerrain.
	*/

	// Private instance of the FastNoise library by Jordan Peck.
	private FastNoise noise;

	/// <summary>
	/// Stores the previous player position.
	/// </summary>
	private ChunkPosition previousPlayerPosition;

	private int chunkMatrixSize = 2;

	/// <summary>
	/// Used to notify the main thread when a chunk has been generated to rebuild chunk meshes.
	/// </summary>
	private delegate void ChunkGenerated();

	/// <summary>
	/// Handlers that subscribed to the ChunkGenerated event.
	/// </summary>
	private ChunkGenerated chunkGeneratedHandlers;

	void Start()
	{
		this.GenerateTerrain();
		this.previousPlayerPosition = Player.instance.GetVoxelChunk();
		this.chunkGeneratedHandlers += this.ChunkGenerationCompleted;

		Thread test = new Thread(() => {
			this.GenerateChunkBlocks(0,0);
			//this.ChunkGenerationCompleted();
		});

		test.Start();

		// use coroutines to access Unity API reference value stuff & threads for heavy calculations.
	}

	/// <summary>
	/// Called when a thread finished generating a chunk.
	/// </summary>
	void ChunkGenerationCompleted()
	{
		Debug.Log("Hello from other thread!");
	}

	void Update()
	{
		ChunkPosition currentPlayerPosition = Player.instance.GetVoxelChunk();

		if (this.previousPlayerPosition == currentPlayerPosition)
			return;

		List<ChunkPosition> chunksToDeletePositions = new List<ChunkPosition>();
		ChunkPosition diff = currentPlayerPosition - this.previousPlayerPosition;

		if (diff.z == 0)
			// Chunk position changed across the x-axis.
			for (int i = -1; i < 2; i++)
				chunksToDeletePositions.Add(new ChunkPosition(
					currentPlayerPosition.x - diff.x * 3, 
					currentPlayerPosition.z + i
				));
		else
			// Chunk position changed across the z-axis.
			for (int i = -1; i < 2; i++)
				chunksToDeletePositions.Add(new ChunkPosition(
					currentPlayerPosition.x + i, 
					currentPlayerPosition.z - diff.z * 3
				));

		foreach(ChunkPosition cp in chunksToDeletePositions)
		{
			if (!PCTerrain.GetInstance().chunks.ContainsKey(cp))
				continue;

			GameObject.Destroy(PCTerrain.GetInstance().chunks[cp].chunkGameObject);
			PCTerrain.GetInstance().chunks.Remove(cp);
		}

		this.previousPlayerPosition = currentPlayerPosition;
	}

	public void GenerateTerrain()
	{
		foreach(Chunk chunk in PCTerrain.GetInstance().chunks.Values)
			chunk.Destroy();

		this.noise = new FastNoise();

		System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();

		for (int _i = 0; _i < chunkMatrixSize; _i++)
			for (int _k = 0; _k < chunkMatrixSize; _k++)
			{
				Chunk chunk = new Chunk();
				chunk.x = _i;
				chunk.z = _k;

				for (int i = 0; i < Chunk.chunkSize; i++)
					for (int j = 0; j < Chunk.chunkHeight; j++)
						for (int k = 0; k < Chunk.chunkSize; k++)
						{
							BaseBlock block = this.GenerateTerrainBlockType(i + chunk.x * 16, j, k + chunk.z * 16);
							chunk.blocks[i,j,k] = block;

							if (chunk.blocks[i,j,k].stateful)
								PCTerrain.GetInstance().blocks[(i,j,k).ToVector3Int()] = Registry.Instantiate(block.blockName) as Block;
						}
	
				chunk.BuildMesh();

				PCTerrain.GetInstance().chunks[(chunk.x, chunk.z)] = chunk;
			}
		
		stopwatch.Stop();

		Debug.Log("Terrain generated in: " + stopwatch.Elapsed.TotalMilliseconds + "ms");
	}

	private BaseBlock[,,] GenerateChunkBlocks(int x, int z)
	{
		BaseBlock[,,] chunkBlocks = new BaseBlock[Chunk.chunkSize, Chunk.chunkHeight, Chunk.chunkSize];

		for (int i = 0; i < Chunk.chunkSize; i++)
			for (int j = 0; j < Chunk.chunkHeight; j++)
				for (int k = 0; k < Chunk.chunkSize; k++)
				{
					BaseBlock block = this.GenerateTerrainBlockType(i + x * 16, j, k + z * 16);
					chunkBlocks[i,j,k] = block;

					if (chunkBlocks[i,j,k].stateful)
						PCTerrain.GetInstance().blocks[(i,j,k).ToVector3Int()] = Registry.Instantiate(block.blockName) as Block;
				}

		return chunkBlocks;
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
			i * 2.1f,
			k * 2.1f
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

		float baselineLandHeight 	= Chunk.chunkHeight * 0.2f + landSimplex1 + landSimplex2;
		float baselineStoneHeight 	= Chunk.chunkHeight * 0.04f + stoneSimplex1 + stoneSimplex1;
		float baselineCaveHeight 	= Chunk.chunkHeight * 0.066f;

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
