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

		InventoryContainers.inventory.items[26] = new InventoryItem("rainbowGenerator");

		GUI.hotbar.UpdateGUI();
    }

    // Update is called once per frame
    void Update()
    {
		Vector3Int p = Player.instance.GetVoxelPosition();
		ChunkPosition cp = Player.instance.GetVoxelChunk();
		this.positionText.text = System.String.Format("({0},{1},{2}) ({3},{4})", p.x, p.y, p.z, cp.x, cp.z);
    }

	/// <summary>
	/// Calls the given function after the given delay (in seconds).
	/// </summary>
	public void RunAfterDelay(System.Action callback, float delay)
	{
		StartCoroutine(DelayedRun(callback, delay));
	}

	private System.Collections.IEnumerator DelayedRun(System.Action callback, float delay)
	{
		yield return new WaitForSeconds(delay);

		callback();
	}
}
