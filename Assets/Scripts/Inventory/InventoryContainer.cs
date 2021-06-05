using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryContainer : MonoBehaviour
{
	/// <summary>
	/// The UUID associated with the inventory container.
	/// </summary>
	private string id;

	/// <summary>
	/// Reference to the items grid.
	/// </summary>
	private GameObject[] items;

	/// <summary>
	/// The number of items the inventory will contain.
	/// </summary>
	[SerializeField]
	private int itemsCount;

	/// <summary>
	/// Whether all the items contained are draggable.
	/// </summary>
	public bool itemsDraggable = true;

    void Awake()
    {
		// Register inventory in InventoryContainers
		this.id = System.Guid.NewGuid().ToString();
		InventoryContainers.containers[this.id] = this;

		GridLayoutGroup layoutGroup = this.GetComponent<GridLayoutGroup>();
		layoutGroup.cellSize 		= new Vector2(32, 32);
		layoutGroup.spacing 		= new Vector2(4, 3);

        // Get the reference to the inventory grid.
		this.items = new GameObject[this.itemsCount];

		for (int i = 0; i < this.itemsCount; i++)
			this.items[i] = this.CreateItemSlotObject(i);
    }

    void Update()
    {
        
    }

	/// <summary>
	/// Allows to create an item slot object to append to the grid.
	/// </summary>
	private GameObject CreateItemSlotObject(int index)
	{
		GameObject itemSlotObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UIItemSlot"));
		itemSlotObject.name = String.Format("item{0}", index);
		itemSlotObject.transform.SetParent(this.transform, false);

		return itemSlotObject;
	}
}
