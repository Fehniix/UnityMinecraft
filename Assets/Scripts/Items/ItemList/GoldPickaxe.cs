using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickaxe: Item
{
    public GoldPickaxe()
    {
        this.itemName 				= "goldPickaxe";
		this.itemTextureName 		= "golden_pickaxe";
		this.placeable				= false;
		this.miningLevel			= MiningLevel.IRON;
		this.toolType				= ToolType.PICKAXE;
		this.breakingSpeedModifier	= 2.5f;
		this.LoadPrefab();
    }
}
