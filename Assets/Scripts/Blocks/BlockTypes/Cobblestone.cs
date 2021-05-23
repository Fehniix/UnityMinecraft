using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cobblestone: Block
{
    public Cobblestone(): base()
	{
		this.blockName = "cobblestone";
		this.hardness = 3 * 20;
	}
}
