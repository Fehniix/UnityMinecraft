using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronPickaxe: Item
{
    public IronPickaxe()
    {
        this.itemName 				= "ironPickaxe";
		this.itemTextureName 		= "iron_pickaxe";
		this.placeable				= false;
		this.miningLevel			= MiningLevel.IRON;
		this.toolType				= ToolType.PICKAXE;
		this.breakingSpeedModifier	= 2.0f;
		this.LoadPrefab();
    }
}
