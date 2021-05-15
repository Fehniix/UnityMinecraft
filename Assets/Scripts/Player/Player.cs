using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	// Reference to the player's CharacterController.
	private CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        this.characterController = GetComponent<CharacterController>();

		// Set the object reference in GameState
		GameState.player = this;
    }

	void Update()
	{
		
	}

	/// <summary>
	/// The player's position is described by an (x,y,z) vector referring to the VoxelWorld coordinates.
	/// (0,0,0) represents the origin, (1,0,0) represents the block just to the right.
	/// </summary>
	public Vector3 GetVoxelPosition()
	{
		float _x = Mathf.Abs(this.transform.position.x),
		_y = Mathf.Abs(this.transform.position.y),
		_z = Mathf.Abs(this.transform.position.z);

		// A block spawned at position (0,0,0) appears shifted by (.5f,0,.5f) due to its center point (also pivotal) being aligned to
		// Unity's global world space origin. Thus, to assign a block an integer (i,j,k) position, it's necessary to consider
		// the i,z .5f shift.

		Vector3 position = Vector3.zero;

		position.x = (_x % 1) > 0.49 ? Mathf.CeilToInt(_x) : Mathf.FloorToInt(_x);
		position.y = Mathf.FloorToInt(_y);
		position.z = (_z % 1) > 0.49 ? Mathf.CeilToInt(_z) : Mathf.FloorToInt(_z);

		return position;
	}
}
