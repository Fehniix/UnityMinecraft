using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements the "Tick loop". There are 20 ticks in a second.
/// </summary>
public class Clock : MonoBehaviour
{
	/// <summary>
	/// Hideous public static instance of the class.
	/// </summary>
	public static Clock instance;

	/// <summary>
	/// Called every tick.
	/// </summary>
	[HideInInspector]
	public delegate void TickDelegate();

	/// <summary>
	/// Contains all tick delegates to call each tick.
	/// </summary>
	private TickDelegate tickDelegates;

    void Awake()
    {
		Debug.Log("[Clock] Started.");
		
		Clock.instance = this;

		StartCoroutine(this.Tick());
    }

	private IEnumerator Tick()
	{
		while(true)
		{
			if (this.tickDelegates != null)
				this.tickDelegates();

			yield return new WaitForSeconds(1f / 20f);
		}
	}

	public void AddTickDelegate(TickDelegate tickDelegate)
	{
		this.tickDelegates += tickDelegate;
	}

	public void RemoveTickDelegate(TickDelegate tickDelegate)
	{
		this.tickDelegates -= tickDelegate;
	}
}
