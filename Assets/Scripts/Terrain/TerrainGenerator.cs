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

	private int chunkMatrixSize = 5;

	private int chunkRenderDistance = 5;

	/// <summary>
	/// Used to notify the main thread when a chunk has been generated to rebuild chunk meshes.
	/// </summary>
	private delegate void ChunkBatchGenerated();

	/// <summary>
	/// Handlers that subscribed to the ChunkGenerated event.
	/// </summary>
	private ChunkBatchGenerated chunkBatchGeneratedHandlers;

	void Start()
	{
		this.noise = new FastNoise();

		this.GenerateStartingTerrain();
		this.previousPlayerPosition = Player.instance.GetVoxelChunk();
		this.chunkBatchGeneratedHandlers += this.ChunkGenerationCompleted;
	}

	void Update()
	{
		ChunkPosition currentPlayerPosition = Player.instance.GetVoxelChunk();

		if (this.previousPlayerPosition == currentPlayerPosition)
			return;

		GameObject[] chunks = GameObject.FindGameObjectsWithTag("chunk");
		foreach (GameObject _chunk in chunks)
		{
			if (
				Mathf.Abs(_chunk.transform.position.x / Chunk.chunkSize - currentPlayerPosition.x) >= this.chunkRenderDistance ||
				Mathf.Abs(_chunk.transform.position.z / Chunk.chunkSize - currentPlayerPosition.z) >= this.chunkRenderDistance
			)
			{
				ChunkPosition chunkPos = new ChunkPosition(
					(int)_chunk.transform.position.x / Chunk.chunkSize, 
					(int)_chunk.transform.position.z / Chunk.chunkSize
				);

				if (PCTerrain.GetInstance().chunks.ContainsKey(chunkPos))
					PCTerrain.GetInstance().chunks.Remove(chunkPos);

				GameObject.Destroy(_chunk);
			}
		}

		List<ChunkPosition> chunksToAdd = new List<ChunkPosition>();
		for (int i = -this.chunkRenderDistance / 2; i < this.chunkRenderDistance / 2; i++)
			for (int k = -this.chunkRenderDistance / 2; k < this.chunkRenderDistance / 2; k++)
			{
				ChunkPosition chunkPos = new ChunkPosition(currentPlayerPosition.x - i, currentPlayerPosition.z - k);
				if (!PCTerrain.GetInstance().chunks.ContainsKey(chunkPos))
					chunksToAdd.Add(chunkPos);
			}

		StartCoroutine(this.GenerateChunkPoolAsync(chunksToAdd.ToArray()));

		this.previousPlayerPosition = currentPlayerPosition;
	}

	/// <summary>
	/// Called when a thread finished generating a chunk.
	/// </summary>
	private void ChunkGenerationCompleted() {}

	public void GenerateStartingTerrain()
	{
		foreach(Chunk chunk in PCTerrain.GetInstance().chunks.Values)
			chunk.Destroy();

		System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();

		ChunkPosition playerChunkPos = Player.instance.GetVoxelChunk();
		int halvedChunkSize = chunkMatrixSize / 2;

		for (int _i = playerChunkPos.x - halvedChunkSize; _i < playerChunkPos.x + halvedChunkSize; _i++)
			for (int _k = playerChunkPos.z - halvedChunkSize; _k < playerChunkPos.z + halvedChunkSize; _k++)
			{
				Chunk chunk = new Chunk();
				chunk.x = _i;
				chunk.z = _k;

				BaseBlock[,,] blocks;
				this.GenerateChunkBlocks(_i, _k, out blocks);

				chunk.blocks = blocks;

				StartCoroutine(chunk.BuildMeshAsync());

				PCTerrain.GetInstance().chunks[(chunk.x, chunk.z)] = chunk;
			}
		
		stopwatch.Stop();

		Debug.Log("Terrain generated in: " + stopwatch.Elapsed.TotalMilliseconds + "ms");
	}

	/// <summary>
	/// 
	/// </summary>
	public IEnumerator GenerateChunkPoolAsync(ChunkPosition[] chunkPositions, System.Action callback = null)
	{
		List<Coroutine> coroutinePool = new List<Coroutine>();

		foreach(ChunkPosition chunkPos in chunkPositions)
			coroutinePool.Add(StartCoroutine(this.GenerateChunkAsync(chunkPos.x, chunkPos.z)));

		foreach(Coroutine c in coroutinePool)
			yield return c;

		if (this.chunkBatchGeneratedHandlers != null)
			this.chunkBatchGeneratedHandlers();

		if (callback != null)
			callback();
	}

	private enum ThreadStatus { STARTED, FINISHED };
	private Dictionary<ChunkPosition, ThreadStatus> status = new Dictionary<ChunkPosition, ThreadStatus>();
	private IEnumerator GenerateChunkAsync(int x, int z)
	{
		Chunk chunk = new Chunk();
		chunk.x = x;
		chunk.z = z;

		ChunkPosition chunkPos = new ChunkPosition(x,z);
		this.status[chunkPos] = ThreadStatus.STARTED;
		
		BaseBlock[,,] blocks = null;

		Thread chunkBlocksGenerationThread = new Thread(() => {
			this.GenerateChunkBlocks(x, z, out blocks);
			this.status[chunkPos] = ThreadStatus.FINISHED;
		});
		chunkBlocksGenerationThread.Start();

		while(this.status[chunkPos] == ThreadStatus.STARTED)
			yield return new WaitForEndOfFrame();
		
		chunk.blocks = blocks;

		StartCoroutine(chunk.BuildMeshAsync());

		PCTerrain.GetInstance().chunks[(chunk.x, chunk.z)] = chunk;
	}

	/// <summary>
	/// Generates a chunk's blocks. The chunk is identified by an (x,y) position. Generated blocks are inserted into the `blocks` out parameter.
	/// </summary>
	private void GenerateChunkBlocks(int x, int z, out BaseBlock[,,] blocks)
	{
		blocks = new BaseBlock[Chunk.chunkSize, Chunk.chunkHeight, Chunk.chunkSize];

		for (int i = 0; i < Chunk.chunkSize; i++)
			for (int j = 0; j < Chunk.chunkHeight; j++)
				for (int k = 0; k < Chunk.chunkSize; k++)
				{
					BaseBlock block = this.GenerateTerrainBlockType(i + x * 16, j, k + z * 16);
					blocks[i,j,k] = block;

					if (blocks[i,j,k].stateful)
						PCTerrain.GetInstance().blocks[(i,j,k).ToVector3Int()] = Registry.Instantiate(block.blockName) as Block;
				}
	}

	/// <summary>
	/// Given the (i,j,k) space coordinates, generates the single seeded BaseBlock.
	/// </summary>
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
