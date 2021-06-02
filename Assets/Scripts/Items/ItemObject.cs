using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
	/// <summary>
	/// The name of the item represented by this prefab.
	/// </summary>
	public string itemName;

    // Start is called before the first frame update
    void Start()
    {
		this.gameObject.transform.position += new Vector3(0.0f, 0.3f, 0.0f);
        this.gameObject.AddComponent<BoxCollider>();
		this.gameObject.GetComponent<BoxCollider>().size = new Vector3(1.0f, 1.0f, 1.0f);
    }
}
