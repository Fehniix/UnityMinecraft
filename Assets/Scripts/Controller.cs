using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GameObject.Find("Player").transform.position = new Vector3(8, 80, 8);

		InventoryContainers.hotbar.items[0] = new InventoryItem("torch");

		PlayerInventoryManager.hotbarRef.UpdateGUI();
    }

    // Update is called once per frame
    void Update()
    {
		
    }
}
