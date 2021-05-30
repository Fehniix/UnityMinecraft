using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
	/// <summary>
	/// The size of each item cell in the hotbar.
	/// </summary>
	private const float itemCellSize = 40.0f;

	/// <summary>
	/// The active item image reference, to move left or right upon mouse scroll.
	/// </summary>
	private GameObject activeItemImgReference;

	/// <summary>
	/// Represents the center x-coordinate of the active item image.
	/// </summary>
	private float activeItemImgFirstX;

    void Awake()
    {
		InventoryManager.hotbarItems		= new Item[9];
		InventoryManager.activeItemIndex 	= 4;
		this.activeItemImgReference 		= this.transform.GetChild(0).gameObject;
		this.activeItemImgFirstX			= this.transform.position.x - itemCellSize * 4;

		//! Test.
		Item i 		= new Item("dirt");
		i.placeable = true;
		i.isBlock 	= true;

		InventoryManager.hotbarItems[0] 	= i;
		//! End test.
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
			this.UpdateActiveItem();
    }

	/// <summary>
	/// Updates the currently active item and translates the active item image.
	/// </summary>
	void UpdateActiveItem()
	{
		int scrollDirection = Input.mouseScrollDelta.y > 0 ? 1 : -1;
		InventoryManager.activeItemIndex += scrollDirection;

		if (InventoryManager.activeItemIndex == -1)
			InventoryManager.activeItemIndex = 8;

		InventoryManager.activeItemIndex %= 9;

		Vector3 currentPosition = this.activeItemImgReference.transform.localPosition;
		currentPosition.x = itemCellSize * InventoryManager.activeItemIndex + this.activeItemImgFirstX;
		
		this.activeItemImgReference.transform.localPosition = currentPosition;
	}

	
}
