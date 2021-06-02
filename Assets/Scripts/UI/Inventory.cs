using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		InventoryManager.inventoryItems = new InventoryItem[27];
        InventoryManager.inventoryRef 	= this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
