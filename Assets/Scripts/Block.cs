using UnityEngine;

[RequireComponent(typeof(Clock))]
public class Block : MonoBehaviour
{
	/// <summary>
	/// ID describing the unique block.
	/// </summary>
	[HideInInspector]
	public string id;

	/// <summary>
	///Describes the name of the block. The block texture depends on it.
	/// </summary>
	[HideInInspector]
	public string blockName = "air";

	/// <summary>
	///Describes the name of the texture. If not set, defaults to the block name.
	/// </summary>
	[HideInInspector]
	public string textureName = "default";

	/// <summary>
	///If set to false, one texture is used for all the block's faces.
	/// </summary>
	[HideInInspector]
	public bool hasSidedTextures = false;

	/// <summary>
	/// The number of ticks it takes to break the block.
	/// </summary>
	public int hardness = -1;

	/// <summary>
	/// Whether the block is breakable or not.
	/// </summary>
	public bool breakable = true;

	[HideInInspector]
	/// <summary>
	/// Whether the block was broken or not.
	/// </summary>
	public bool broken = false;

	/// <summary>
	/// Called when the block is broken.
	/// </summary>
	[HideInInspector] 
	public delegate void BreakDelegate();

	/// <summary>
	/// Called when the block is broken.
	/// </summary>
	[HideInInspector] 
	public BreakDelegate breakDelegate;

	/// <summary>
	/// Called when the player began breaking the block.
	/// </summary>
	[HideInInspector]
	public delegate void BeganBreakingDelegate();

	/// <summary>
	/// Called when the player began breaking the block.
	/// </summary>
	[HideInInspector]
	public BeganBreakingDelegate beganBreakingDelegate;

	/// <summary>
	/// Breaks the given block.
	/// </summary>
	public void Break()
	{
		if (GameState.debug)
			foreach (MeshRenderer render in this.gameObject.GetComponentsInChildren<MeshRenderer>())
				render.material.color = Color.red;

		this.broken = true;

		this.breakDelegate();
	}

	// Block breaking logic.
	/// <summary>
	/// Indicates the current breaking progress. Upon a set threshold, the block is finally broken.
	/// </summary>
	private int _breakingProgress = 0;
	public void BeginBreak()
	{
		Debug.Log("[Block Breaking] Initiating block breaking... " + this.blockName);

		if (this.beganBreakingDelegate != null)
			this.beganBreakingDelegate();

		this._breakingProgress = 0;

		this.GetComponentInChildren<BreakingHypercube>(true).gameObject.SetActive(true);

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
			this.Break();
		}
	}

	/// <summary>
	/// If the user lifts the configured button to break the block before the aforementioned is broken, 
	/// stop breaking it and reset the breaking progress.
	/// </summary>
	public void EndBreak()
	{
		Debug.Log("[Block Breaking] User either lifted the break button, broke the block or moved outside block view: " + this.blockName);

		this._breakingProgress = 0;

		this.GetComponentInChildren<BreakingHypercube>(true).gameObject.SetActive(false);

		Clock.instance.RemoveTickDelegate(this.UpdateBreakingProgress);
	}

	/// <summary>
	/// Takes care of updating the current block's hypercube breaking texture.
	/// </summary>
	private void UpdateBreakingTexture()
	{
		HypercubeFace[] faces 		= this.GetComponentsInChildren<HypercubeFace>();
		int breakingStage 			= Mathf.FloorToInt((float)this._breakingProgress / (float)this.hardness * 10f);
		Texture2D breakingTexture 	= CachedResources.Load<Texture2D>(System.String.Format("Textures/Destroy/destroy_stage_{0}", breakingStage));
		
		foreach(HypercubeFace face in faces)
			face.GetComponent<MeshRenderer>().material.mainTexture = breakingTexture;
	}

	void Awake()
	{
		this.id = System.Guid.NewGuid().ToString();
	}
}
