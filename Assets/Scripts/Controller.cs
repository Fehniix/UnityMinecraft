using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GameObject.Find("Player").transform.position = new Vector3(8, 80, 8);

		InventoryManager.hotbarItems[0] = new InventoryItem("torch");
		InventoryManager.hotbarItems[0].placeable = true;

		InventoryManager.hotbarRef.UpdateHotbarItems();
    }

    // Update is called once per frame
    void Update()
    {
		
    }
}
