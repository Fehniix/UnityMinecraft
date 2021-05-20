using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Blocks
{
	/// <summary>
	/// For each dictionary entry, contains the name (ID) of the block & the type class reference to it.
	/// </summary>
	private static Dictionary<string, System.Type> registeredBlocks;

    public static void RegisterBlock<T>() where T: new()
	{
		
	}
}
