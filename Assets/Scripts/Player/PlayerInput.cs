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
	public float gravitationalAcceleration = -9.8f;

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

		Vector3 movement = new Vector3(deltaX, this.gravitationalAcceleration, deltaY);

		// When moving diagonally, ||i+k|| > speed; "clamping" the movement vector's magnitude solves the issue.
		movement = Vector3.ClampMagnitude(movement, this.speed);
		
		// Now imagine the game running at 1200fps without the following line. Also, given the game is paused,
		// Time.deltaTime equals zero: movement is zeroed out too.
		movement *= Time.deltaTime;

		// Transform direction to global space coordinates.
		movement = this.transform.TransformDirection(movement);

        this.characterController.Move(movement);
    }
}
