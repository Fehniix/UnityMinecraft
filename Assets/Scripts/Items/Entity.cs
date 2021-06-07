using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
	/// <summary>
	/// The name of the entity.
	/// </summary>
	public string entityName;

	/// <summary>
	/// The number of items contained within the entity.
	/// </summary>
	public int quantity = 1;

	/// <summary>
	/// Determines whether the item can be currently picked up or not.
	/// Used to allow the player to drop a block/item and not pick it up immediately after.
	/// </summary>
	public bool pickupCooldownActive = false;

	/// <summary>
	/// Duration of the pickup cooldown expressed in ticks.
	/// </summary>
	public int pickupCooldownDuration = 20 * 2;

	/// <summary>
	/// Measure of elapsed time (ticks).
	/// </summary>
	private int elapsedTicks = 0;

    void OnTriggerStay(Collider hitObject)
	{
		if (hitObject.GetComponent<Player>() == null)
			return;

		if (this.pickupCooldownActive)
			return;

		int quantityPlaced;
		do {
			quantityPlaced = PlayerInventoryManager.AddItem(this.entityName, this.quantity);
			this.quantity -= quantityPlaced;

			if (quantityPlaced == 0)
				break;
		} while(this.quantity > 0);

		if (quantityPlaced != 0)
			Destroy(this.transform.gameObject);
	}

	/// <summary>
	/// Starts and enables the pickup cooldown. The player will not be able to pick up the entity before the cooldown duration expires.
	/// </summary>
	public void StartPickupCooldown()
	{
		Debug.Log("Pickup cooldown started. (" + this.entityName + ")");
		this.pickupCooldownActive = true;
		this.elapsedTicks = 0;

		Clock.instance.AddTickDelegate(this.CooldownTicked);
	}

	/// <summary>
	/// TickDelegate function. Keeping a hard reference inside the class to be able to remove the delegate from the Clock.
	/// </summary>
	private void CooldownTicked()
	{
		this.elapsedTicks++;

		if (this.elapsedTicks >= this.pickupCooldownDuration)
		{
			this.pickupCooldownActive = false;
			this.elapsedTicks = 0;
			Debug.Log("Pickup cooldown ended. (" + this.entityName + ")");

			Clock.instance.RemoveTickDelegate(this.CooldownTicked);
		}
	}
}
