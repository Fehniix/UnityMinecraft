using System.Collections;
using System.Collections.Generic;

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
	}

	public static PCTerrain GetInstance()
	{
		if (PCTerrain.instance == null)
			PCTerrain.instance = new PCTerrain();

		return PCTerrain.instance;
	}

	/// <summary>
	/// The terrain's chunks organized by (x,y) coordinates.
	/// </summary>
	public Dictionary<ChunkPosition, Chunk> chunks;
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