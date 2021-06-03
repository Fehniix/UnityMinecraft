using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemImage : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
			this.HandleLeftMouseClick();
    }

	/// <summary>
	/// Allows to handle the left mouse click to start dragging the item.
	/// </summary>
	void HandleLeftMouseClick()
	{
		RaycastHit hit;

		if (!Physics.Raycast(Input.mousePosition, Vector3.forward, out hit))
			return;

		if (hit.transform.GetComponent<InventoryItemImage>() == null)
			return;

		Debug.Log(hit.transform.gameObject);
	}
}
