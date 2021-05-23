using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class PlayerController : MonoBehaviour
{
	private Camera _camera;

	/// <summary>
	/// Hard reference to the block that is currently being broken.
	/// </summary>
	private Block _breakingBlockReference;

	/// <summary>
	/// Indicates whether or not a block is being broken.
	/// </summary>
	private bool _breakingBlock = false;
	
    // Start is called before the first frame update
    void Start()
    {
        this._camera = this.GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
			this.HandleMouseLeftClickDown();

		if (Input.GetMouseButton(0))
			this.HandleMouseLeftClickHeld();

		if (Input.GetMouseButtonUp(0))
			this.HandleMouseLeftClickUp();
    }

	private void HandleMouseLeftClickDown()
	{
		// Player position test
		Debug.Log("Voxel Position: " + GameState.player.GetVoxelPosition());
		Debug.Log(TargetBlock.Get()?.blockName ?? "undefined");
	}

	private void HandleMouseLeftClickHeld()
	{
		this.KeepBreaking();
	}

	private void HandleMouseLeftClickUp()
	{
		this.EndBreak();
	}

	/// <summary>
	/// Manages the BeginBreak logic. See KeepBreaking().
	/// </summary>
	/// <param name="target">The `Face` GameObject that was hit by a Raycast.</param>
	private void BeginBreak(Block target)
	{
		if (target != null)
		{
			this._breakingBlock = true;
			this._breakingBlockReference = target;

			target.BeginBreak();
		}
	}

	private void PrintVector(Vector3 v)
	{
		Debug.Log("[" + v.x + ", " + v.y + ", " + v.z + "]");
	}

	/// <summary>
	/// Manages block breaking (start-progress-end). 
	/// If the player is currently breaking a block and the view moves off no longer targeting the block, breaking is reset.
	/// </summary>
	private void KeepBreaking()
	{
		Block block = this.GetTargetBlock();
		
		if (block == null) 
		{
			// The player is currently breaking a block and moved off the cursor to the sky.
			if (this._breakingBlock && this._breakingBlockReference != null)
				this.ResetBreaking();

			return;
		}

		if (block.broken)
			return;

		if (this._breakingBlockReference == null)
			this.BeginBreak(block);

		if ((this._breakingBlockReference.id != block.id) && this._breakingBlock)
			this.ResetBreaking();
	}

	/// <summary>
	/// If the player lifted the configured break button before the block was broken, end breaking.
	/// </summary>
	private void EndBreak()
	{
		if (!this._breakingBlock)
			return;

		Block block = this.GetTargetBlock();

		if (block.broken)
			return;

		this._breakingBlock = false;
		this._breakingBlockReference = null;

		if (block == null)
			return;

		block.EndBreak();
	}

	private void ResetBreaking()
	{
		if (this._breakingBlockReference == null || !this._breakingBlock)
			return;

		if (this._breakingBlockReference.broken)
		{
			// The block was successfully broken!
			// Block.EndBreak() was already called by the block itself. Reset only bool state and ref.
			this._breakingBlock = false;
			this._breakingBlockReference = null;

			return;
		}

		this._breakingBlockReference.EndBreak();

		this._breakingBlock = false;
		this._breakingBlockReference = null;
	}

	/// <summary>
	/// Shoots a Raycast from the center of the screen (relative to the camera)
	/// </summary>
	/// <param name="hit">Reference parameter to the GameObject that was hit.</param>
	/// <returns>`true` if the Raycast hit a GameObject, `false` otherwise.</returns>
	private bool CenterRaycast(out RaycastHit hit)
	{
		Vector3 cameraCenter = new Vector3(this._camera.pixelWidth / 2, this._camera.pixelHeight / 2, 0);

		Ray ray = this._camera.ScreenPointToRay(cameraCenter);
	
		if (Physics.Raycast(ray, out hit))
			return true;
		return false;
	}

	/// <summary>
	/// Returns the `Block` instance of the block that the player is looking at.
	/// If the targeted block is "air", returns null.
	/// </summary>
	private Block GetTargetBlock()
	{
		RaycastHit hit;
		if (!this.CenterRaycast(out hit))
			return null;

		Vector3 blockCenterPosition = hit.point - hit.normal * .5f;
			
		ChunkPosition chunkPosition = (
			Mathf.FloorToInt(blockCenterPosition.x / 16), 
			Mathf.FloorToInt(blockCenterPosition.z / 16)
		);

		Vector3 blockCoords = (
			Mathf.FloorToInt(blockCenterPosition.x), 
			Mathf.FloorToInt(blockCenterPosition.y), 
			Mathf.FloorToInt(blockCenterPosition.z)
		).ToVector3();
		
		string blockName = PCTerrain.GetInstance().chunks[chunkPosition].blocks[(int)blockCoords.x, (int)blockCoords.y, (int)blockCoords.z];

		return Blocks.Instantiate(blockName);
	}
}
