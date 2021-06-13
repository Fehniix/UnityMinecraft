using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonePickaxe: Item
{
    public StonePickaxe()
    {
        this.itemName 				= "stonePickaxe";
		this.itemTextureName 		= "stone_pickaxe";
		this.placeable				= false;
		this.miningLevel			= MiningLevel.STONE;
		this.toolType				= ToolType.PICKAXE;
		this.breakingSpeedModifier	= 1.5f;
		this.LoadPrefab();
    }
}
