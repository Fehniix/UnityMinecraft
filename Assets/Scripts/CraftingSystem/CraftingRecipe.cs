using System.Collections;
using System.Collections.Generic;

public struct CraftingRecipe
{
	/// <summary>
	/// A recipe is made of items, in this case just the item names is sufficient.
	/// Each item name specifies where exactly the item needs to go to get a specific crafting result.
	/// </summary>
	public string[,] requirements {
		get; set;
	}

	/// <summary>
	/// The name of the resulting item.
	/// </summary>
	public CraftingResult resultItem {
		get; set;
	}

	public CraftingRecipe(string[,] requirements, CraftingResult result)
	{
		this.requirements = requirements;
		this.resultItem = result;
	}
}
