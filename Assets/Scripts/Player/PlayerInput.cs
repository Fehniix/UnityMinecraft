using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class PlayerInput : MonoBehaviour
{
	/// <summary>
	/// The rate of change of the player's position over time.
	/// </summary>
	private float speed;

	/// <summary>
	/// The speed at which the player walks.
	/// </summary>
	private const float walkingSpeed = 4.317f;

	/// <summary>
	/// The speed at which the player runs.
	/// </summary>
	private const float runningSpeed = 5.612f;

	/// <summary>
	/// Height of the jump that can be performed by the player.
	/// </summary>
	private float jumpHeight = 1.125f;

	/// <summary>
	/// Determines whether the player is currently jumping or not.
	/// </summary>
	private bool jumping = false;

	/// <summary>
	/// Reference to the player's CharacterController. Used to change the player's position.
	/// </summary>
	private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
		this._rigidbody = this.GetComponent<Rigidbody>();
		this.speed = walkingSpeed;
    }

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			this.jumping = true;
			Debug.Log("Space pressed");
		}
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		float deltaX = Input.GetAxis("Horizontal");
		float deltaY = Input.GetAxis("Vertical");

		if (deltaX == 0 && deltaY == 0 && !this.jumping && false)
			this._rigidbody.velocity = Vector3.zero;
		
		// Grounding logic.
		float raycastDistance = this.GetComponent<BoxCollider>().bounds.extents.y + 0.01f;
		
		RaycastHit verticalHit;
		bool grounded = Physics.Raycast(this.transform.position, -this.transform.up, out verticalHit, raycastDistance);

		// Movement along the x-z axis
		Vector3 movement_xz = new Vector3(deltaX, 0.1f, deltaY);

		// When moving diagonally, ||i+k|| > speed; "clamping" the movement vector's magnitude solves the issue.
		movement_xz = Vector3.ClampMagnitude(movement_xz, this.speed);

		if (grounded && this.jumping)
		{
			this.jumping = false;

			// The force is applied istantaneously to the player at its center of mass.
			// Given the istantaneous nature of the force, we're thinking about an impulse.
			this._rigidbody.AddForce((
				0f, 
				Mathf.Sqrt(-2.0f * Physics.gravity.y * this.jumpHeight), 
				0f
			).ToVector3(), ForceMode.Impulse);
		}

		// Transform direction to global space coordinates.
		Vector3 movement = this.transform.TransformDirection(movement_xz);

		this._rigidbody.velocity = new Vector3(movement.x * this.speed, this._rigidbody.velocity.y, movement.z * this.speed);
    }
}
