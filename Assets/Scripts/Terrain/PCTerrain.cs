using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

/// <summary>
/// Holds all loaded chunks.
/// (It's called "PCTerrain" to disambiguate from Unity's Terrain. PC as in "PowerCraft").
/// </summary>
public class PCTerrain
{
	// Singleton instance.
	private static PCTerrain instance;

	private PCTerrain()
	{
		this.chunks = new Dictionary<ChunkPosition, Chunk>();
		this.blocks = new Dictionary<Vector3, Block>();
	}

	public static PCTerrain GetInstance()
	{
		if (PCTerrain.instance == null)
			PCTerrain.instance = new PCTerrain();

		return PCTerrain.instance;
	}

	/// <summary>
	/// Contains instances of targeted blocks.
	/// </summary>
	public Dictionary<Vector3, Block> blocks;

	/// <summary>
	/// The terrain's chunks organized by (x,y) coordinates.
	/// </summary>
	public Dictionary<ChunkPosition, Chunk> chunks;

	/// <summary>
	/// Calls `BuildMesh()` on all chunks.
	/// </summary>
	public void RebuildAllChunks()
	{
		foreach(Chunk chunk in this.chunks.Values)
		{
			chunk.BuildMesh();
		}
	}

	/// <summary>
	/// Removes the block at the supplied position without breaking it.
	/// </summary>
	public void RemoveAt(int x, int y, int z)
	{
		if (this.blocks[(x,y,z).ToVector3()] == null)
			return;

		this.blocks[(x,y,z).ToVector3()] = null;
		this.RemoveBlockFromChunkMesh(x,y,z);
	}

	/// <summary>
	/// Breaks the block at the supplied position and clears the block instance.
	/// </summary>
	public void BreakAt(int x, int y, int z)
	{
		if (this.blocks[(x,y,z).ToVector3()] == null)
			return;

		this.blocks[(x,y,z).ToVector3()].Break();
		this.blocks[(x,y,z).ToVector3()] = null;
		this.RemoveBlockFromChunkMesh(x,y,z);
	}

	/// <summary>
	/// Removes a block given (x,y,z) Voxel coordinates.
	/// </summary>
	private void RemoveBlockFromChunkMesh(int x, int y, int z)
	{
		ChunkPosition chunkPosition = (
			Mathf.FloorToInt(x / 16), 
			Mathf.FloorToInt(z / 16)
		);

		this.chunks[chunkPosition].blocks[x % 16, y, z % 16] = "air";
		this.chunks[chunkPosition].BuildMesh();
	}
}

public struct ChunkPosition
{
	/// <summary>
	/// x-position of the chunk in the world.
	/// </summary>
	public int x;

	/// <summary>
	/// z-position of the chunk in the world.
	/// </summary>
	public int z;

	public ChunkPosition(int x, int z)
	{
		this.x = x;
		this.z = z;
	}

	public static implicit operator ChunkPosition((int x, int z) t) => new ChunkPosition(t.x, t.z);
}