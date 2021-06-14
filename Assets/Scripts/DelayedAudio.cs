using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAudio : MonoBehaviour
{
	/// <summary>
	/// Plays the given AudioSource after `byAmount` seconds.
	/// </summary>
	public void PlayDelayed(AudioSource source, float byAmount)
	{
		StartCoroutine(this.PlayDelayedCoroutine(byAmount, source));
	}

	/// <summary>
	/// Plays the given AudioSource after `byAmount` seconds.
	/// </summary>
	private IEnumerator PlayDelayedCoroutine(float byAmount, AudioSource source)
	{
		yield return new WaitForSeconds(byAmount);
		source.Play();
	}
}
