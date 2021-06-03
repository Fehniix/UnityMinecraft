using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	/// <summary>
	/// Represents the (x,y) position of the first item image on the inventory UI.
	/// </summary>
	private Vector2 firstItemPosition;

    void Awake()
    {
		this.firstItemPosition 			= this.GetComponentInChildren<InventoryItemsContainer>().transform.GetChild(0).transform.position;
		InventoryManager.inventoryItems = new InventoryItem[27];
        InventoryManager.inventoryRef 	= this;

		this.gameObject.SetActive(false);
		Debug.Log("Position of (0,0) item: " + this.firstItemPosition);
    }

    void Update()
    {
        
    }
}
