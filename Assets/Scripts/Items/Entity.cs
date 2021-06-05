using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
	/// <summary>
	/// The name of the entity.
	/// </summary>
	public string entityName;

    void OnTriggerEnter(Collider hitObject)
	{
		if (hitObject.GetComponent<Player>() == null)
			return;

		Entity entity = this.GetComponent<Entity>();
		
		if (PlayerInventoryManager.AddItem(this.entityName, 1))
			Destroy(this.transform.gameObject);
	}
}
