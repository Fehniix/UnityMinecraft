using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class Item
{
	/// <summary>
	/// The name of the item.
	/// </summary>
    public string itemName = "default";

	/// <summary>
	/// The 2D texture name of the item.
	/// </summary>
	public string itemTextureName = "default";

	/// <summary>
	/// The prefab associated with the item.
	/// </summary>
	public GameObject prefab;

	/// <summary>
	/// The Voxel world coordinates of the item to be placed.
	/// </summary>
	public Vector3 coordinates;

	/// <summary>
	/// Whether the item emits light or not.
	/// </summary>
	public bool emitsLight = false;

	/// <summary>
	/// Loads the prefab associated to the item name.
	/// </summary>
	public void LoadPrefab()
	{
		// Load the prefab.
		this.prefab = Resources.Load<GameObject>(System.String.Format("Prefabs/{0}", this.itemName));
	}

	/// <summary>
	/// Places the item on the world.
	/// </summary>
	public void Place()
	{
		this.prefab.transform.position = this.coordinates + (0.5f, 0.0f, 0.5f).ToVector3();
		GameObject.Instantiate(this.prefab);

		// Recalculate normals to show light changes!
		ChunkPosition chunkPosition = new ChunkPosition((int)this.coordinates.x / 16, (int)this.coordinates.z / 16);
		PCTerrain.GetInstance().chunks[chunkPosition].RecalculateNormals();
	}
}
