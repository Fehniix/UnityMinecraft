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

		GameObject.Find("Player").transform.Translate(new Vector3(512, 70, 512));

		InventoryContainers.hotbar.items[0] = new InventoryItem("furnace");
		InventoryContainers.hotbar.items[1] = new InventoryItem("woodPickaxe");
		InventoryContainers.hotbar.items[2] = new InventoryItem("cobblestone");
		InventoryContainers.hotbar.items[3] = new InventoryItem("stonePickaxe");
		InventoryContainers.hotbar.items[4] = new InventoryItem("ironPickaxe");
		InventoryContainers.hotbar.items[5] = new InventoryItem("goldPickaxe");
		InventoryContainers.hotbar.items[6] = new InventoryItem("diamondPickaxe");
		InventoryContainers.hotbar.items[2].quantity = 64;

		GUI.hotbar.UpdateGUI();
    }

    // Update is called once per frame
    void Update()
    {
		Vector3Int p = Player.instance.GetVoxelPosition();
		ChunkPosition cp = Player.instance.GetVoxelChunk();
		this.positionText.text = System.String.Format("({0},{1},{2}) ({3},{4})", p.x, p.y, p.z, cp.x, cp.z);
    }
}
