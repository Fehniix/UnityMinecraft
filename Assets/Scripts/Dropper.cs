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
	public static void DropItem(string itemName, Vector3 coordinates, int quantity = 1, Vector3? forceVector = null, bool pickupCooldownActive = false)
	{
		if (Registry.IsBlock(itemName))
		{
			// Other than building the block mesh, this also adds in the Entity component + SphereCollider trigger. 
			GameObject entity = BlockBuilder.Build(itemName, quantity);

			// Adding 0.5f after flooring components results in the block's world center coordinates.
			entity.transform.position = Utils.ToVectorInt(coordinates.FloorAdd(0.5f));

			Vector3 force = new Vector3(
				Random.Range(0, 30) * 0.1f,
				4.0f,
				Random.Range(0, 30) * 0.1f
			);

			entity.GetComponent<Rigidbody>().AddForce(forceVector != null ? forceVector.Value : force, ForceMode.Impulse);

			if (pickupCooldownActive)
				entity.GetComponent<Entity>().StartPickupCooldown();
		}
		else
		{
			Item item = Registry.Instantiate(itemName) as Item;
			
			// Create an entity to drop.
			GameObject entity = GameObject.Instantiate(item.prefab);

			entity.AddComponent<Rigidbody>();
			entity.AddComponent<SphereCollider>();
			entity.AddComponent<Entity>();

			entity.GetComponent<SphereCollider>().center	= new Vector3(0.5f, 0.5f, 0.5f);
			entity.GetComponent<SphereCollider>().radius	= 4.0f;
			entity.GetComponent<SphereCollider>().isTrigger	= true;

			entity.GetComponent<Entity>().entityName 		= itemName;
			entity.GetComponent<Entity>().quantity			= quantity;

			entity.GetComponent<Rigidbody>().constraints 	= RigidbodyConstraints.FreezeRotation;

			entity.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
			entity.layer				= 2; // Ignore Raycast.
			
			// Adding 0.5f after flooring components results in the block's world center coordinates.
			entity.transform.position = Utils.ToVectorInt(coordinates.FloorAdd(0.5f));
			entity.transform.position += new Vector3(0, 1, 0);

			entity.GetComponent<Rigidbody>().AddForce(new Vector3(
				Random.Range(0, 30) * 0.1f,
				4.0f,
				Random.Range(0, 30) * 0.1f
			), ForceMode.Impulse);

			if (pickupCooldownActive)
				entity.GetComponent<Entity>().StartPickupCooldown();
		}
	}
}
