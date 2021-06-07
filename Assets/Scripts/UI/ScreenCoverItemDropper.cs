using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCoverItemDropper : MonoBehaviour
{
	/// <summary>
	/// Contains the Rect size of the inventory BG. Used to check whether the mouse click occurred within the inventory.
	/// </summary>
	private Rect inventoryBGRect;

	void Start()
	{
		Vector3[] corners = new Vector3[4];
		GameObject.Find("InventoryBG").GetComponent<RectTransform>().GetWorldCorners(corners);
		this.inventoryBGRect = new Rect(corners[0], corners[2] - corners[0]);
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && InventoryContainers.draggingItem != null && !this.inventoryBGRect.Contains(Input.mousePosition))
		{
			InventoryItem draggingItem = InventoryContainers.draggingItem;
			Dropper.DropItem(draggingItem.itemName, Player.instance.transform.position, draggingItem.quantity, new Vector3(
				0,
				4.0f,
				0
			) + Player.instance.transform.forward * 8, true);
			InventoryContainers.draggingItem = null;
			InventoryContainers.draggingItemObject.GetComponent<DraggingItem>().UpdateTexture();
		}
    }
}
