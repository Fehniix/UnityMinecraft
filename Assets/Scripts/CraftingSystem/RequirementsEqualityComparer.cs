using System.Collections.Generic;

/// <summary>
/// Used to force the recipe dictionary to compare the string matrix's elements rather than hashcodes.
/// </summary>
public class RequirementsEqualityComparer : IEqualityComparer<string[,]>
{
	public bool Equals(string[,] x, string[,] y)
	{
		for (int i = 0; i < x.GetLength(0); i++)
			for (int j = 0; j < y.GetLength(1); j++)
				if (x[i,j] != y[i,j])
					return false;
		return true;
	}

	public int GetHashCode(string[,] obj)
	{
		int result = 17;
		for (int i = 0; i < obj.GetLength(0); i++)
			for (int j = 0; j < obj.GetLength(1); j++)
				unchecked
				{
					result = result * 23 + obj[i,j]?.GetHashCode() ?? 0;
				}
		return result;
	}
}