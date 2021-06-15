using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coal: Item
{
    public Coal()
    {
        this.itemName 				= "coal";
		this.itemTextureName 		= "coal";
		this.placeable				= false;
		this.placeableOnlyOnTop 	= false;
		this.placeableOnOtherItems 	= false;
		this.hasGenericMesh			= true;
		this.burnable				= true;
		this.burnTime				= 80 * 20;
		this.LoadPrefab();
    }
}
