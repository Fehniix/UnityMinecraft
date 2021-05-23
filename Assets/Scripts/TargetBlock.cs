using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class TargetBlock : MonoBehaviour
{
	/// <summary>
	/// Contains instances of targeted blocks.
	/// </summary>
	private static Dictionary<Vector3, Block> blocks;

	void Awake()
	{
		TargetBlock.blocks = new Dictionary<Vector3, Block>();
	}

	/// <summary>
	/// Shoots a Raycast from the center of the screen (relative to the camera)
	/// </summary>
	/// <param name="hit">Reference parameter to the GameObject that was hit.</param>
	/// <returns>`true` if the Raycast hit a GameObject, `false` otherwise.</returns>
	private static bool CenterRaycast(out RaycastHit hit)
	{
		Camera _camera = Camera.main;
		Vector3 cameraCenter = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);

		Ray ray = _camera.ScreenPointToRay(cameraCenter);
	
		if (Physics.Raycast(ray, out hit))
			return true;
		return false;
	}

	/// <summary>
	/// Returns the `Block` instance of the block that the player is looking at.
	/// If the targeted block is "air", returns null.
	/// </summary>
	public static Block Get()
	{
		RaycastHit hit;
		if (!TargetBlock.CenterRaycast(out hit))
			return null;

		// `hit.normal` represents the normal vector with respect to the hit mesh.
		// By over-simplifying the chunk mesh to a simple cube, `hit.normal` refers to the hit face's normal vector.
		// This aims at positioning the hit point inside the cube, being then able to get the hit block's coordinates.
		Vector3 blockCenterPosition = hit.point - hit.normal * .5f;
		
		ChunkPosition chunkPosition = (
			Mathf.FloorToInt(blockCenterPosition.x / 16), 
			Mathf.FloorToInt(blockCenterPosition.z / 16)
		);

		Vector3 blockCoords = (
			Mathf.FloorToInt(blockCenterPosition.x), 
			Mathf.FloorToInt(blockCenterPosition.y), 
			Mathf.FloorToInt(blockCenterPosition.z)
		).ToVector3();

		// The block's (x,y,z) coordinates with respect to the Voxel world.
		Vector3 blockWCoords = (
			blockCoords.x + chunkPosition.x,
			blockCoords.y,
			blockCoords.z + chunkPosition.z
		).ToVector3();

		if (TargetBlock.blocks.ContainsKey(blockWCoords))
			return TargetBlock.blocks[blockWCoords];
		
		string blockName = PCTerrain.GetInstance().chunks[chunkPosition].blocks[
			(int)blockCoords.x, 
			(int)blockCoords.y, 
			(int)blockCoords.z
		];

		Block instance = Blocks.Instantiate(blockName);
		TargetBlock.blocks[(
			blockCoords.x + chunkPosition.x,
			blockCoords.y,
			blockCoords.z + chunkPosition.z).ToVector3()
		] = instance;

		return instance;
	}

	/// <summary>
	/// Removes the block at the supplied position without breaking it.
	/// </summary>
	public static void RemoveAt(int x, int y, int z)
	{
		if (TargetBlock.blocks[(x,y,z).ToVector3()] == null)
			return;

		TargetBlock.blocks[(x,y,z).ToVector3()] = null;
	}

	/// <summary>
	/// Breaks the block at the supplied position and clears the block instance.
	/// </summary>
	public void BreakAt(int x, int y, int z)
	{
		if (TargetBlock.blocks[(x,y,z).ToVector3()] == null)
			return;

		TargetBlock.blocks[(x,y,z).ToVector3()].Break();
		TargetBlock.blocks[(x,y,z).ToVector3()] = null;
	}
}
