using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	/// <summary>
	/// The rate of change of the player's position over time.
	/// </summary>
	public float speed = 6.0f;

	/// <summary>
	/// The downwards acceleration due to gravity.
	/// </summary>
	private float gravitationalAcceleration = -9.81f;

	/// <summary>
	/// Current magnitude of the downwards velocity vector.
	/// </summary>
	private float downwardsSpeed = -9.81f;

	/// <summary>
	/// Height of the jump that can be performed by the player.
	/// </summary>
	private float jumpHeight = 1f;

	/// <summary>
	/// Reference to the player's CharacterController. Used to change the player's position.
	/// </summary>
	private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
		this.characterController = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
		float deltaX = Input.GetAxis("Horizontal") * speed;
		float deltaY = Input.GetAxis("Vertical") * speed;

		// Movement along the x-z axis
		Vector3 movement_xz = new Vector3(deltaX, 0, deltaY);
		// Movement along the y axis
		Vector3 movement_y	= new Vector3(0, this.downwardsSpeed, 0);

		// When moving diagonally, ||i+k|| > speed; "clamping" the movement vector's magnitude solves the issue.
		movement_xz = Vector3.ClampMagnitude(movement_xz, this.speed);

		if (this.characterController.isGrounded)
		{
			movement_y = Vector3.zero;
			this.downwardsSpeed = this.gravitationalAcceleration;
			
			if (Input.GetKeyDown(KeyCode.Space))
			{
				movement_y += new Vector3(0, Mathf.Sqrt(this.jumpHeight * -3.0f * this.gravitationalAcceleration), 0);
			}
		}
		else
			// Emulate gravitational acceleration.
			this.downwardsSpeed += this.gravitationalAcceleration * Time.deltaTime;

		// Now imagine the game running at 1200fps without the following line. Also, given the game is paused,
		// Time.deltaTime equals zero: movement is zeroed out too.
		movement_xz *= Time.deltaTime;
		movement_y 	*= Time.deltaTime;

		// Transform direction to global space coordinates.
		Vector3 movement = this.transform.TransformDirection(movement_xz + movement_y);

        this.characterController.Move(movement);
    }
}
