using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTableUI : UserInterface
{
    // Start is called before the first frame update
    void Start()
    {
		this.canBeOverlapped = true;
		
        GUI.craftingTableUI = this;

		this.gameObject.SetActive(false);
    }
}
