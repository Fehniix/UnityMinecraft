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
		this.chunks = new List<List<Chunk>>();
	}

	public PCTerrain GetInstance()
	{
		if (PCTerrain.instance == null)
			PCTerrain.instance = new PCTerrain();

		return PCTerrain.instance;
	}

	/// <summary>
	/// The terrain's chunks organized by (x,y) coordinates.
	/// </summary>
	public List<List<Chunk>> chunks;
}
