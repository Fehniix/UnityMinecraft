using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPickaxe: Item
{
    public DiamondPickaxe()
    {
        this.itemName 				= "diamondPickaxe";
		this.itemTextureName 		= "diamond_pickaxe";
		this.placeable				= true;
		this.placeableOnlyOnTop 	= true;
		this.placeableOnOtherItems 	= false;
		this.miningLevel			= 3;
		this.toolType				= ToolType.PICKAXE;
		this.breakingSpeedModifier	= 3f;
		this.LoadPrefab();
    }
}
