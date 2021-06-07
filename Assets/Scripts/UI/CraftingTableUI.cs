using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTableUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GUI.craftingTableUI = this;

		this.gameObject.SetActive(false);
    }
}
