using UnityEngine;
using UnityEngine.UI;

public class DraggingItem : MonoBehaviour
{
	void Awake()
	{
		InventoryContainers.draggingItemObject = this.gameObject;
		this.gameObject.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {	
        this.transform.position = Input.mousePosition;
	}

	/// <summary>
	/// Updates the dragging item's texture.
	/// </summary>
	public void UpdateTexture()
	{
		if (InventoryContainers.draggingItem == null)
		{
			this.gameObject.SetActive(false);
			return;
		}

		this.gameObject.SetActive(true);

		this.gameObject.GetComponentInChildren<Image>().sprite 	= TextureStitcher.instance.GetBlockItemSprite(InventoryContainers.draggingItem.itemName);
		this.gameObject.GetComponentInChildren<Image>().color 	= Color.white;

		GameObject textObj = this.transform.GetChild(1).gameObject;

		if (InventoryContainers.draggingItem.quantity != 1)
		{
			textObj.SetActive(true);
			textObj.GetComponent<Text>().text = InventoryContainers.draggingItem.quantity.ToString();
		}
		else
			textObj.SetActive(false);
	}
}
