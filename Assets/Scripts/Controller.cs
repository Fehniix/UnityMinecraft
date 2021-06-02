using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GameObject.Find("Player").transform.position = new Vector3(8, 80, 8);

		InventoryManager.hotbarRef.UpdateHotbarItems();

		InventoryManager.hotbarItems[0] = new InventoryItem("torch");
		InventoryManager.hotbarItems[0].placeable = true;
		// Texture2D tex = CachedResources.Load<Texture2D>("Textures/Stitch/dirt");
		// GameObject.Find("item0").GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
		
    }
}
