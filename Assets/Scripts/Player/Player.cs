using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	/// <summary>
	/// Obnoxious and unsafe instance of Player.
	/// </summary>
	public static Player instance;

    void Awake()
    {
		Player.instance = this;
    }

	void Update()
	{
		
	}

	/// <summary>
	/// The player's position is described by an (x,y,z) vector referring to the VoxelWorld coordinates.
	/// (0,0,0) represents the origin, (1,0,0) represents the block just to the right.
	/// </summary>
	public Vector3Int GetVoxelPosition()
	{
		return new Vector3Int(
			Mathf.FloorToInt(this.transform.position.x),
			Mathf.FloorToInt(this.transform.position.y),
			Mathf.FloorToInt(this.transform.position.z)
		);
	}

	/// <summary>
	/// Returns the ChunkPosition where the player is currently standing.
	/// </summary>
	public ChunkPosition GetVoxelChunk()
	{
		return new ChunkPosition(
			Mathf.FloorToInt(this.transform.position.x / 16.0f),
			Mathf.FloorToInt(this.transform.position.z / 16.0f)	
		);
	}
}
