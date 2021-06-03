using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class Item: IInteractable
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
	/// Whether the item is placeable or not.
	/// </summary>
	public bool placeable = false;

	/// <summary>
	/// Whether the item can be placed only on the top face of a block.
	/// </summary>
	public bool placeableOnlyOnTop = false;

	/// <summary>
	/// Determines whether this item can be placed on top of other items or not.
	/// </summary>
	public bool placeableOnOtherItems = false;

	/// <summary>
	/// Whether the item is non-empty and can be thus walked through.
	/// Non-empty items cannot be placed on top of other non-empty items.
	/// </summary>
	public bool nonEmpty = true;

	/// <summary>
	/// Whether the block drops itself when broken.
	/// </summary>
	public bool dropsItself = true;

	private bool _interactable;

	/// <summary>
	/// Whether the item is interactable or not.
	/// </summary>
	public bool interactable { 
		get { return this._interactable; } 
		set{ this._interactable = value; }
	}

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
	public virtual void Place()
	{
		// Place the block in the middle.
		this.prefab.transform.position = this.coordinates + (0.5f, 0.0f, 0.5f).ToVector3();

		this.prefab.GetComponent<ItemObject>().itemName = this.itemName;

		// And instantiate the associated prefab.
		GameObject.Instantiate(this.prefab);
	}

	/// <summary>
	/// Breaks the item and removes it from the world.
	/// </summary>
	public virtual void Break()
	{
		if (this.dropsItself)
			Dropper.DropItem(this.itemName, this.coordinates);

		GameObject.Destroy(this.prefab);
	}

	public virtual void Interact()
	{

	}
}
