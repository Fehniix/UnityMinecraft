using System.Collections;
using System.Collections.Generic;

public static class Blocks
{
	/// <summary>
	/// For each dictionary entry, contains the name of the block & the type class associated with it.
	/// </summary>
	private static Dictionary<string, System.Type> registeredBlocks = new Dictionary<string, System.Type>();

	/// <summary>
	/// Registers a block to the local list of available blocks.
	/// Blocks can be then retrieved and instantiated.
	/// </summary>
    public static void RegisterBlock<T>(string name) where T: new()
	{
		Blocks.registeredBlocks[name] = typeof(T);
	}

	/// <summary>
	/// Given a block name, returns a Block instance.
	/// </summary>
	public static Block Spawn(string name)
	{
		if (!Blocks.registeredBlocks.ContainsKey(name))
			return null;

		return (Block)System.Activator.CreateInstance(Blocks.registeredBlocks[name]);
	}

	/// <summary>
	/// Creates a new Block instance from its registered type.
	/// </summary>
	public static T SpawnFromType<T>() where T: new()
	{
		return new T();
	}
}
