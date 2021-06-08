using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceUI : UserInterface
{
    // Start is called before the first frame update
    void Start()
    {
        GUI.furnaceUI = this;

		this.gameObject.SetActive(false);
    }

	/// <summary>
	/// Updates item slots textures.
	/// </summary>
	public override void UpdateGUI()
	{
		this.GetComponentInChildren<PlayerInventoryItems>().UpdateGUI();
		this.GetComponentInChildren<PlayerInventoryHotbar>().UpdateGUI();
	}
}
