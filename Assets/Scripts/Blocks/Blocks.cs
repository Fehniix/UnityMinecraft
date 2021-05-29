using System.Collections.Generic;
using System;

public static class Blocks
{
	/// <summary>
	/// For each dictionary entry, contains the name of the block & the type class associated with it.
	/// </summary>
	private static Dictionary<string, Type> registeredBlocks = new Dictionary<string, Type>();

	/// <summary>
	/// Registers a block to the local list of available blocks.
	/// Blocks can be then retrieved and instantiated.
	/// </summary>
    public static void RegisterBlock<T>(string blockName) where T: new()
	{
		Blocks.registeredBlocks[blockName] = typeof(T);
	}

	/// <summary>
	/// Given a block name, returns a Block instance.
	/// </summary>
	public static Block Instantiate(string blockName)
	{
		if (!Blocks.registeredBlocks.ContainsKey(blockName))
			return null;

		return (Block)Activator.CreateInstance(Blocks.registeredBlocks[blockName]);
	}

	/// <summary>
	/// Given a block name, returns a BaseBlock instance.
	/// </summary>
	public static BaseBlock InstantiateBase(string name)
	{
		if (!Blocks.registeredBlocks.ContainsKey(name))
			return null;

		return (BaseBlock)Activator.CreateInstance(Blocks.registeredBlocks[name]);
	}

	/// <summary>
	/// Creates a new Block instance from its registered type.
	/// </summary>
	public static T SpawnFromType<T>() where T: new()
	{
		return new T();
	}
}
