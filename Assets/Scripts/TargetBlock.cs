using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class TargetBlock : MonoBehaviour
{
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

		if (hit.transform.gameObject.GetComponent<ChunkObject>() == null)
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
		Vector3Int blockWCoords = (
			(int)blockCoords.x + chunkPosition.x,
			(int)blockCoords.y,
			(int)blockCoords.z + chunkPosition.z
		).ToVector3Int();

		if (PCTerrain.GetInstance().blocks.ContainsKey(blockWCoords))
			return PCTerrain.GetInstance().blocks[blockWCoords];
		
		string blockName = PCTerrain.GetInstance().chunks[chunkPosition].blocks[
			(int)blockCoords.x, 
			(int)blockCoords.y, 
			(int)blockCoords.z
		];

		if (blockName == "air")
			return null;

		Block blockInstance = Blocks.Instantiate(blockName);
		blockInstance.coordinates = blockWCoords;
		
		PCTerrain.GetInstance().blocks[(
			blockCoords.x + chunkPosition.x,
			blockCoords.y,
			blockCoords.z + chunkPosition.z).ToVector3()
		] = blockInstance;

		return blockInstance;
	}
}
