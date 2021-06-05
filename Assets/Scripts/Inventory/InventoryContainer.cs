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

    void Awake()
    {
		// Register inventory in InventoryContainers
		this.id = System.Guid.NewGuid().ToString();
		InventoryContainers.containers[this.id] = this;

        // Get the reference to the inventory grid.
		this.items = new GameObject[this.itemsCount];
    }

    void Update()
    {
        
    }
}
