using System;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
	/// <summary>
	/// Static instance to the hotbar component.
	/// </summary>
	public static Hotbar instance;

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
		InventoryManager.hotbarItems		= new InventoryItem[9];
		InventoryManager.activeItemIndex 	= 4;
		this.activeItemImgReference 		= this.transform.GetChild(0).gameObject;
		this.activeItemImgFirstX			= this.transform.position.x - itemCellSize * 4;

		InventoryManager.hotbarRef 			= this;
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

		Vector3 currentPosition = this.activeItemImgReference.transform.position;
		currentPosition.x = itemCellSize * InventoryManager.activeItemIndex + this.activeItemImgFirstX;
		
		this.activeItemImgReference.transform.position = currentPosition;
	}

	/// <summary>
	/// Updates item images.
	/// </summary>
	public void UpdateHotbarItems()
	{
		for(int i = 0; i < 9; i++)
		{
			InventoryItem item 		= InventoryManager.hotbarItems[i];
			GameObject hotbarImage 	= GameObject.Find(String.Format("item{0}", i));

			if (item == null) 
			{
				hotbarImage.GetComponent<Image>().color = Color.clear;
				hotbarImage.transform.GetChild(0).gameObject.SetActive(false);
				continue;
			}

			hotbarImage.GetComponent<Image>().sprite 	= TextureStitcher.instance.GetBlockItemSprite(item.itemName);
			hotbarImage.GetComponent<Image>().color 	= Color.white;

			if (item.quantity != 1)
			{
				hotbarImage.transform.GetChild(0).gameObject.SetActive(true);
				hotbarImage.transform.GetChild(0).gameObject.GetComponent<Text>().text = item.quantity.ToString();
			}
			else
				hotbarImage.transform.GetChild(0).gameObject.SetActive(false);
		}
	}
}
