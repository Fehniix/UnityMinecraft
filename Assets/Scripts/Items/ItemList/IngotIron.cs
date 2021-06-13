using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngotIron: Item
{
    public IngotIron()
    {
        this.itemName 				= "ironIngot";
		this.itemTextureName 		= "iron_ingot";
		this.placeable				= false;
		this.placeableOnlyOnTop 	= false;
		this.placeableOnOtherItems 	= false;
		this.hasGenericMesh			= true;
		this.LoadPrefab();
    }
}
