using System.Collections.Generic;
using UnityEngine;
using Extensions;

/// <summary>
/// Represents a fundamental block.
/// </summary>
public abstract class Block: BaseBlock, IInteractable
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
	/// Determines whether the block is placeable in the world or not.
	/// </summary>
	public bool placeable = true;

	/// <summary>
	/// The level at which the IInteractable is breakable.
	/// </summary>
	private MiningLevel _miningLevel = MiningLevel.WOOD;
	/// <summary>
	/// The level at which the IInteractable is breakable.
	/// </summary>
	public MiningLevel miningLevel {
		get { return this._miningLevel; }
		set { this._miningLevel = value; }
	}

	/// <summary>
	/// Used to store accessors' value.
	/// </summary>
	private bool _interactable = false;

	/// <summary>
	/// Whether the block is interactable or not.
	/// </summary>
	public bool interactable { 
		get { return this._interactable; } 
		set{ this._interactable = value; }
	}

	/// <summary>
	/// Whether the item is smeltable or not.
	/// </summary>
	private bool _smeltable = false;

	/// <summary>
	/// Whether the item is smeltable or not.
	/// </summary>
	public bool smeltable {
		get { return this._smeltable; }
		set { this._smeltable = value; }
	}

	/// <summary>
	/// Whether the item can be burned to produce heat or not.
	/// </summary>
	private bool _burnable = false;

	/// <summary>
	/// Whether the item can be burned to produce heat or not.
	/// </summary>
	public bool burnable {
		get { return this._burnable; }
		set { this._burnable = value; }
	}

	/// <summary>
	/// The number of ticks the fuel item lasts for as a burnable item.
	/// </summary>
	public int burnTime {
		get { return 300; }
		set {}
	}

	/// <summary>
	/// Item result when smelted.
	/// </summary>
	private CraftingResult? _smeltedResult;

	/// <summary>
	/// Item result when smelted.
	/// </summary>
	public CraftingResult? smeltedResult {
		get { return this._smeltedResult; }
		set { this._smeltedResult = value; }
	}

	/// <summary>
	/// Whether the block was broken or not.
	/// </summary>
	public bool broken = false;

	/// <summary>
	/// Whether a specific tool type is required or not. 
	/// </summary>
	public bool anyToolRequired = false;

	/// <summary>
	/// The type of tool that might be required to break the block.
	/// </summary>
	public ToolType toolTypeRequired = ToolType.ANY;

	/// <summary>
	/// The sound the block produces when broken or placed.
	/// </summary>
	public BlockSoundType soundType = BlockSoundType.STONE;

	/// <summary>
	/// The list of items to drop.
	/// </summary>
	public List<Drop> drops;

	/// <summary>
	/// Whether the block drops itself when broken.
	/// </summary>
	public bool dropsItself = true;

	/// <summary>
	/// Maximum amount of items that can be aggregated in a single item slot.
	/// </summary>
	public int maxStack = 64;

	/// <summary>
	/// The temporary variable where to store the original hardness before the activeItem modifies it.
	/// </summary>
	private int originalHardness = -1;

	public Block()
	{
		this.id 	= System.Guid.NewGuid().ToString();
		this.drops 	= new List<Drop>();
	}

	/// <summary>
	/// Breaks the given block.
	/// </summary>
	public virtual void Break()
	{
		this.broken = true;

		if (this.dropsItself)
			Dropper.DropItem(this.blockName, this.coordinates);

		foreach(Drop drop in this.drops)
			if (Random.Range(0, 101) > (1.0f - drop.probability) * 100)
				Dropper.DropItem(drop.itemName, this.coordinates, drop.quantity);
	}

	/// <summary>
	/// Allows the player to interact with the block.
	/// </summary>
	public virtual void Interact() {}

	/// <summary>
	/// Allows to place the block where the player is currently looking at.
	/// </summary>
	public virtual Vector3? Place()
	{
		RaycastHit hit;
		bool didHit = Physics.Raycast(Camera.main.ScreenPointToRay((
			Camera.main.pixelWidth / 2,
			Camera.main.pixelHeight / 2,
			0
		).ToVector3()), out hit);

		if (!didHit)
			return null;
		
		Vector3Int placingBlockCoordinates = Utils.ToVectorInt(hit.point + hit.normal / 2.0f);
		Vector3Int playerPosition = Player.instance.GetVoxelPosition();

		if (
			placingBlockCoordinates == playerPosition || 
			placingBlockCoordinates == new Vector3Int(playerPosition.x, playerPosition.y + 1, playerPosition.z)
		)
			return null;

		string soundName = "Breaking/";

		if (this.soundType == BlockSoundType.STONE)
			soundName += "stone" + Random.Range(1, 5);

		if (this.soundType == BlockSoundType.WOOD)
			soundName += "wood" + Random.Range(1, 5);

		if (this.soundType == BlockSoundType.DIRT)
			soundName += "dirt" + Random.Range(1, 5);

		AudioSource source = AudioManager.Create3DSound(soundName);
		source.transform.parent = hit.transform;
		source.transform.localPosition = new Vector3(placingBlockCoordinates.x % 16, placingBlockCoordinates.y, placingBlockCoordinates.z % 16);
		AudioManager.Play3DSound(source);
		
		return PCTerrain.GetInstance().PlaceAt(this.blockName, placingBlockCoordinates);
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

		InventoryItem activeItem 	= InventoryContainers.hotbar.items[PlayerInventoryManager.activeItemIndex];
		Item itemInstance 			= activeItem?.itemInstance as Item;

		if (this.miningLevel > 0 || this.toolTypeRequired != ToolType.ANY)
		{
			if (itemInstance == null)
				return;
			
			if (this.miningLevel > itemInstance.miningLevel)
				return;

			if (itemInstance.toolType != this.toolTypeRequired)
				return;
		}
			
		if (itemInstance != null && this.toolTypeRequired == itemInstance.toolType)
		{
			this.originalHardness = this.hardness;
			this.hardness /= (int)itemInstance.breakingSpeedModifier;
		}

		//Debug.Log("[Block Breaking] Initiating block breaking... " + this.blockName);

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

		if (this.originalHardness != -1)
			this.hardness = this.originalHardness;

		//Debug.Log("[Block Breaking] User either lifted the break button, broke the block or moved outside block view: " + this.blockName);

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