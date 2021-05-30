using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    void OnTriggerEnter(Collider hitObject)
	{
		if (hitObject.GetComponent<Player>() == null)
			return;

		Destroy(this.transform.gameObject);
	}
}
