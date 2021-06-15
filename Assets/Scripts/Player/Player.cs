using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	/// <summary>
	/// Obnoxious and unsafe instance of Player.
	/// </summary>
	public static Player instance;

	/// <summary>
	/// Stepping clips related t dirt-type blocks ready to be played.
	/// </summary>
	private AudioClip[] dirtSteppingClips;

	/// <summary>
	/// Stepping clips related t dirt-type blocks ready to be played.
	/// </summary>
	private AudioClip[] stoneSteppingClips;

	/// <summary>
	/// Stepping clips related t dirt-type blocks ready to be played.
	/// </summary>
	private AudioClip[] woodSteppingClips;

	/// <summary>
	/// Reference to the player's AudioSource.
	/// </summary>
	private AudioSource audioSource;

    void Awake()
    {
		Player.instance = this;

		this.dirtSteppingClips 	= new AudioClip[6];
		this.stoneSteppingClips = new AudioClip[6];
		this.woodSteppingClips 	= new AudioClip[6];

		for (int i = 0; i < 6; i++)
		{
			this.dirtSteppingClips[i] = Resources.Load<AudioClip>("Sounds/Stepping/grass" + (i + 1));
			this.stoneSteppingClips[i] = Resources.Load<AudioClip>("Sounds/Stepping/stone" + (i + 1));
			this.woodSteppingClips[i] = Resources.Load<AudioClip>("Sounds/Stepping/wood" + (i + 1));
		}

		this.audioSource = this.GetComponent<AudioSource>();
    }

	/// <summary>
	/// Plays a stepping sound depending on the block that the player stepped on.
	/// </summary>
	public void PlaySteppingSound(BlockSoundType soundType)
	{
		int soundToPlay = Random.Range(0, 6);
		AudioClip selectedClip = null;

		if (soundType == BlockSoundType.STONE)
			selectedClip = this.stoneSteppingClips[soundToPlay];

		if (soundType == BlockSoundType.WOOD)
			selectedClip = this.woodSteppingClips[soundToPlay];

		if (soundType == BlockSoundType.DIRT)
			selectedClip = this.dirtSteppingClips[soundToPlay];

		Debug.Log("Playing a sound! " + selectedClip);

		this.audioSource.clip = selectedClip;
		this.audioSource.Play();
	}

	/// <summary>
	/// Stops all sounds currently playing that are associated with the Player.
	/// </summary>
	public void StopAllSounds()
	{
		this.audioSource.Stop();
	}

	/// <summary>
	/// The player's position is described by an (x,y,z) vector referring to the VoxelWorld coordinates.
	/// (0,0,0) represents the origin, (1,0,0) represents the block just to the right.
	/// </summary>
	public Vector3Int GetVoxelPosition()
	{
		return new Vector3Int(
			Mathf.FloorToInt(this.transform.position.x),
			Mathf.FloorToInt(this.transform.position.y),
			Mathf.FloorToInt(this.transform.position.z)
		);
	}

	/// <summary>
	/// Returns the ChunkPosition where the player is currently standing.
	/// </summary>
	public ChunkPosition GetVoxelChunk()
	{
		return new ChunkPosition(
			Mathf.FloorToInt(this.transform.position.x / 16.0f),
			Mathf.FloorToInt(this.transform.position.z / 16.0f)	
		);
	}
}
