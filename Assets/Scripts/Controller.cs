using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class Controller : MonoBehaviour
{
	public Text positionText;

    // Start is called before the first frame update
    void Start()
    {
		this.positionText = GameObject.Find("UI/StaticWrapper/Position").GetComponent<Text>();

		GameObject.Find("Player").transform.position = new Vector3(8, 40, 8);

		for (int i = 0; i < 9; i++)
			InventoryContainers.hotbar.items[i] = new InventoryItem("cobblestone");

		for (int i = 0; i < 26; i++)
			InventoryContainers.inventory.items[i] = new InventoryItem("torch");

		InventoryContainers.hotbar.items[0] = new InventoryItem("furnace");
		InventoryContainers.hotbar.items[2].quantity = 64;
		InventoryContainers.hotbar.items[8] = new InventoryItem("craftingTable");

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
		Vector3Int p = Player.instance.GetVoxelPosition();
		ChunkPosition cp = Player.instance.GetVoxelChunk();
		this.positionText.text = System.String.Format("({0},{1},{2}) ({3},{4})", p.x, p.y, p.z, cp.x, cp.z);
    }
}
