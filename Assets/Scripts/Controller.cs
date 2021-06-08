using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GameObject.Find("Player").transform.position = new Vector3(8, 40, 8);

		for (int i = 0; i < 9; i++)
			InventoryContainers.hotbar.items[i] = new InventoryItem("torch");

		for (int i = 0; i < 26; i++)
			InventoryContainers.inventory.items[i] = new InventoryItem("torch");

		InventoryContainers.hotbar.items[0] = new InventoryItem("craftingTable");

		GUI.hotbar.UpdateGUI();

		string[,] requirements = new string[3,3] {
				{"torch", "torch", null},
				{"torch", "torch", null},
				{null, null, null}
		};
		
		CraftingRecipeRegistry.RegisterRecipe(new CraftingRecipe(requirements, new CraftingResult("cobblestone", 64)));
    }

    // Update is called once per frame
    void Update()
    {
		
    }
}
