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

		//! Test.
		InventoryItem i 		= new InventoryItem("grass");
		i.placeable = true;
		i.isBlock 	= true;
		i.quantity	= 3;

		InventoryManager.hotbarItems[4] 	= i;
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

		Vector3 currentPosition = this.activeItemImgReference.transform.position;
		currentPosition.x = itemCellSize * InventoryManager.activeItemIndex + this.activeItemImgFirstX;
		
		this.activeItemImgReference.transform.position = currentPosition;
	}

	/// <summary>
	/// Updates item images.
	/// </summary>
	public void UpdateItems()
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

			string textureName = item.itemName;
			object instantiatedItem = Registry.Instantiate(item.itemName);

			if (instantiatedItem is Block && ((Block)instantiatedItem).hasSidedTextures)
			{
				Block block = (Block)instantiatedItem;

				if (block.textureName != "default")
					textureName = block.textureName;

				if (block.hasSidedTextures)
					if (TextureStitcher.instance.TextureUVs.ContainsKey(System.String.Format("{0}_{1}", textureName, "front")))
						textureName = System.String.Format("{0}_{1}", textureName, "front");
					else
						textureName = System.String.Format("{0}_{1}", textureName, "side");	
			}

			Texture2D tex = CachedResources.Load<Texture2D>(String.Format("Textures/Stitch/{0}", textureName));
			hotbarImage.GetComponent<Image>().sprite = Sprite.Create(
				tex, 
				new Rect(0.0f, 0.0f, tex.width, tex.height), 
				new Vector2(0.5f, 0.5f)
			);

			hotbarImage.GetComponent<Image>().color = Color.white;

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
