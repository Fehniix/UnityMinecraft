using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public static class Dropper
{
	/// <summary>
	/// Given the registry item name to drop and broken block coordinates, instantiates a small cube and makes it shoot out 
	/// in a random direction from the block center.
	/// </summary>
	public static void DropItem(string itemName, Vector3 coordinates, int quantity = 1)
	{
		for (int i = 0; i < quantity; i++)
		{
			GameObject entity = BlockBuilder.Build(itemName);

			// Adding 0.5f after flooring components results in the block's world center coordinates.
			entity.transform.position = Utils.ToVectorInt(coordinates.FloorAdd(0.5f));

			entity.GetComponent<Rigidbody>().AddForce(new Vector3(
				Random.Range(0, 30) * 0.1f,
				4.0f,
				Random.Range(0, 30) * 0.1f
			), ForceMode.Impulse);
		}
	}
}
