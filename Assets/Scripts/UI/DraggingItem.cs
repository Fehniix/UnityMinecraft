using UnityEngine;
using UnityEngine.UI;

public class DraggingItem : MonoBehaviour
{
	void Awake()
	{
		InventoryContainers.draggingItemObject = this.gameObject;
	}

    // Update is called once per frame
    void Update()
    {
		/**
		* A bit of an explanation is due - the following code doesn't forgive.
		* REVIEW needs to be cleaned.
		* `this.draggingItem` is the GameObject that contains the UI.Image GameObject that represents the item following the cursor.
		* If an item is currently being dragged (fetched from the static InventoryManager),
		* have it follow Input.mousePosition and, if still inactive, initialize its image and quantity text.
		* Otherwise, if the an item is not being dragged any longer and `this.draggingItem` is still active, deactivate it.
		*/
		
        if (InventoryContainers.draggingItem != null)
		{
			if (!this.gameObject.activeSelf)
			{
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

			this.transform.position = Input.mousePosition;
		}
		else if (this.gameObject.activeSelf)
			this.gameObject.SetActive(false);
    }
}
