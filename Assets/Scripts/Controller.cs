using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GameObject.Find("Player").transform.position = new Vector3(8, 80, 8);

		InventoryManager.hotbarRef.UpdateItems();
		// Texture2D tex = CachedResources.Load<Texture2D>("Textures/Stitch/dirt");
		// GameObject.Find("item0").GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
		
    }
}
