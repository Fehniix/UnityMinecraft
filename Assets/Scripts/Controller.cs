using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GameObject.Find("Player").transform.position = new Vector3(8, 40, 8);

		InventoryContainers.hotbar.items[0] = new InventoryItem("torch");

		PlayerInventoryManager.hotbarRef.UpdateGUI();

		string[,] requirements = new string[3,3];
		requirements[2,2] = "log";
		Debug.Log(CraftingRecipeRegistry.GetCraftingResult(requirements));
    }

    // Update is called once per frame
    void Update()
    {
		
    }
}
