using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	public enum RotationAxes
	{
		MouseXAndY = 0,
		MouseX = 1,
		MouseY = 2
	}

	public RotationAxes axes = RotationAxes.MouseX;

	public float sensitivityHor = 9.0f;
	public float sensitivityVert = 9.0f;

	public float minimumVert = -45.0f;
	public float maximumVert = 45.0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState 	= CursorLockMode.Locked;
		Cursor.visible 		= false;
    }

    // Update is called once per frame

	private float _rotationX = 0f;
    void Update()
    {
		if (GameState.isPaused || GameState.inventoryOpen)
			return;
			
        if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * this.sensitivityHor, 0);
		}
		else if (axes == RotationAxes.MouseY)
		{
			_rotationX -= Input.GetAxis("Mouse Y") * this.sensitivityVert;
			_rotationX = Mathf.Clamp(_rotationX, this.minimumVert, this.maximumVert);

			float rotationY = transform.localEulerAngles.y;

			transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
		}
		else
		{
			_rotationX -= Input.GetAxis("Mouse Y") * this.sensitivityVert; 
			_rotationX = Mathf.Clamp(_rotationX, this.minimumVert, this.maximumVert);

			float delta = Input.GetAxis("Mouse X") * this.sensitivityHor;
			float rotationY = transform.localEulerAngles.y + delta;

			transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
		}
    }
}
