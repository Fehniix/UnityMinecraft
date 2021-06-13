using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CraftingRecipeRegistry
{
	/// <summary>
	/// Contains all the crafting recipes.
	/// </summary>
	public static Dictionary<string[,], CraftingResult> recipes = new Dictionary<string[,], CraftingResult>(new RequirementsEqualityComparer());

	/// <summary>
	/// Registers a crafting recipe to the registry.
	/// </summary>
	public static void RegisterRecipe(CraftingRecipe recipe)
	{
		recipes[recipe.requirements] = recipe.resultItem;
	}

	/// <summary>
	/// Determines whether the input requirements are part of a known recipe.
	/// </summary>
	public static CraftingResult? GetCraftingResult(string[,] requirements)
	{
		string[,] normalizedRequirements = NormalizeRequirements(requirements);

		if (!recipes.ContainsKey(normalizedRequirements))
			return null;

		return recipes[normalizedRequirements];
	}

	/// <summary>
	/// Given the requirements matrix, removes empty rows and columns from it.
	/// </summary>
	private static string[,] NormalizeRequirements(string[,] requirements)
	{
		string[,] normalized = new string[3,3];

		// Initialize normalized to requirements; effectively making a copy of it.
		for (int i = 0; i < 3; i++)
			for (int j = 0; j < 3; j++)
				normalized[j,i] = requirements[i,j];

		while(MatrixUtils.IsMatrixRowEmpty(normalized, 0) && !MatrixUtils.IsMatrixEmpty(normalized))
			MatrixUtils.UpShiftMatrix(normalized);

		while(MatrixUtils.IsMatrixColumnEmpty(normalized, 0) && !MatrixUtils.IsMatrixEmpty(normalized))
			MatrixUtils.LeftShiftMatrix(normalized);

		return normalized;
	}
}
