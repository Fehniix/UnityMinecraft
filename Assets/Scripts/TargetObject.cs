using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class TargetObject : MonoBehaviour
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
	/// Returns the `Block` or `Item` instance (to be properly casted) of the block that the player is looking at.
	/// If the targeted block is "air", returns null.
	/// </summary>
	public static object Get()
	{
		RaycastHit hit;
		if (!TargetObject.CenterRaycast(out hit))
			return null;

		bool isChunk 	= hit.transform.gameObject.GetComponent<ChunkObject>() != null;
		bool isItem 	= hit.transform.gameObject.GetComponent<ItemObject>() != null;

		if (!isChunk && !isItem)
			return null;

		// `hit.normal` represents the normal vector with respect to the hit mesh.
		// By over-simplifying the chunk mesh to a simple cube, `hit.normal` refers to the hit face's normal vector.
		// This aims at positioning the hit point inside the cube, being then able to get the hit block's coordinates.
		Vector3 objCenterPosition = hit.point - hit.normal * .5f;
		
		ChunkPosition chunkPosition = (
			Mathf.FloorToInt(objCenterPosition.x / 16), 
			Mathf.FloorToInt(objCenterPosition.z / 16)
		);

		Vector3Int objCoords = (
			Mathf.FloorToInt(objCenterPosition.x), 
			Mathf.FloorToInt(objCenterPosition.y), 
			Mathf.FloorToInt(objCenterPosition.z)
		).ToVector3Int();

		if (isChunk)
		{
			if (PCTerrain.GetInstance().blocks.ContainsKey(objCoords))
			{
				return PCTerrain.GetInstance().blocks[objCoords];
			}
			
			string blockName = PCTerrain.GetInstance().chunks[chunkPosition].blocks[
				(int)objCoords.x % 16, 
				(int)objCoords.y, 
				(int)objCoords.z % 16
			].blockName;

			if (blockName == "air")
				return null;

			Block blockInstance = Registry.Instantiate(blockName) as Block;
			blockInstance.coordinates = objCoords;
			
			PCTerrain.GetInstance().blocks[(
				objCoords.x,
				objCoords.y,
				objCoords.z
			).ToVector3()] = blockInstance;

			return blockInstance;
		}
		else
		{
			ItemObject itemObj = hit.transform.gameObject.GetComponent<ItemObject>();

			Item item = Registry.Instantiate(itemObj.itemName) as Item;
			item.prefab = hit.transform.gameObject;
			item.coordinates = Utils.FloorVector3(hit.transform.position - hit.normal / 4);

			return item;
		}
	}
}
