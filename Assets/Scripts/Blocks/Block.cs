using UnityEngine;
using Extensions;

/// <summary>
/// Represents a fundamental block.
/// </summary>
public abstract class Block: BaseBlock
{
	/// <summary>
	/// ID describing the unique block.
	/// </summary>
	public string id;

	/// <summary>
	/// (x,y,z) Voxel world coordinates of the current block. 
	/// </summary>
	public Vector3Int coordinates;

	/// <summary>
	/// The number of ticks it takes to break the block.
	/// </summary>
	public int hardness = -1;

	/// <summary>
	/// Whether the block is breakable or not.
	/// </summary>
	public bool breakable = true;

	/// <summary>
	/// Whether the block was broken or not.
	/// </summary>
	public bool broken = false;

	/// <summary>
	/// Determines whether the block needs a strong and unique reference to it in memory.
	/// Disposed only if broken.
	/// </summary>
	public bool stateful = false;

	public Block()
	{
		this.id = System.Guid.NewGuid().ToString();
	}

	/// <summary>
	/// Breaks the given block.
	/// </summary>
	public void Break()
	{
		Debug.Log("Block broken.");
		this.broken = true;
	}

	// Block breaking logic.
	/// <summary>
	/// Indicates the current breaking progress. Upon a set threshold, the block is finally broken.
	/// </summary>
	private int _breakingProgress = 0;
	public void BeginBreak()
	{
		if (!this.breakable)
			return;
			
		Debug.Log("[Block Breaking] Initiating block breaking... " + this.blockName);

		this._breakingProgress = 0;

		this.SpawnBreakHypercube();

		Clock.instance.AddTickDelegate(this.UpdateBreakingProgress);
	}

	/// <summary>
	/// Updates the breaking progress of the current block and eventually breaks it.
	/// Called exclusively by the Clock's ticks.
	/// </summary>
	private void UpdateBreakingProgress()
	{
		this._breakingProgress++;

		this.UpdateBreakingTexture();

		if (this._breakingProgress >= this.hardness)
		{
			this.EndBreak();
			PCTerrain.GetInstance().BreakAt(this.coordinates.x, this.coordinates.y, this.coordinates.z);
		}
	}

	/// <summary>
	/// If the user lifts the configured button to break the block before the aforementioned is broken, 
	/// stop breaking it and reset the breaking progress.
	/// </summary>
	public void EndBreak()
	{
		if (!this.breakable)
			return;

		Debug.Log("[Block Breaking] User either lifted the break button, broke the block or moved outside block view: " + this.blockName);

		this._breakingProgress = 0;

		GameObject.Destroy(this.hypercubeRef);

		Clock.instance.RemoveTickDelegate(this.UpdateBreakingProgress);
	}

	// Reference the hypercube prefab. Used to destroy the prefab.
	private GameObject hypercubeRef;

	/// <summary>
	/// Takes care of updating the current block's hypercube breaking texture.
	/// </summary>
	private void UpdateBreakingTexture()
	{
		int breakingStage 			= Mathf.FloorToInt((float)this._breakingProgress / (float)this.hardness * 10f);
		Texture2D breakingTexture 	= CachedResources.Load<Texture2D>(System.String.Format("Textures/Destroy/destroy_stage_{0}", breakingStage));
		
		this.hypercubeRef.GetComponentInChildren<MeshRenderer>().sharedMaterial.mainTexture = breakingTexture;
	}

	/// <summary>
	/// Spawns the "break hypercube" at the block's position.
	/// </summary>
	private void SpawnBreakHypercube()
	{
		GameObject breakHypercube = Resources.Load<GameObject>("Prefabs/BreakHypercube");
		breakHypercube.GetComponentInChildren<MeshRenderer>().sharedMaterial.mainTexture = CachedResources.Load<Texture2D>("Textures/Destroy/destroy_stage_0");
		
		Vector3 floatCoords = this.coordinates;
		this.hypercubeRef = GameObject.Instantiate(
			breakHypercube, 
			floatCoords + (0.5, 0.5, 0.5).ToVector3(), 
			Quaternion.identity
		);
	}
}
