using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
	/// <summary>
	/// Determines where the hide panel is going to shrink from.
	/// </summary>
	public ProgressOrientation orientation;

	/// <summary>
	/// Determines the progress of the operation with a number from 0 to 100.
	/// </summary>
	private float progress = 0;

	/// <summary>
	/// Represents the original size of hide panel.
	/// </summary>
	private Vector2 fullSize;

	/// <summary>
	/// Transform reference of the hide panel.
	/// </summary>
	private RectTransform icon;

	/// <summary>
	/// Transform reference to this RectTransform object.
	/// </summary>
	private RectTransform mask;

    // Start is called before the first frame update
    void Start()
    {
		this.mask = this.GetComponent<RectTransform>();
		this.icon = this.transform.GetChild(0).GetComponent<RectTransform>();

		this.fullSize = new Vector2(
			this.mask.rect.width, 
			this.mask.rect.height
		);

		switch(this.orientation)
		{
			/**
			* Note to future self, for clarity.
			*
			* Moving the pivot point of a RectTransform also influences the point from which it gets DRAWN by Unity!
			* Originally, the pivot point is (0.5, 0.5). I'm leveraging it to make it easier to define transformations
			* along different axes.
			* Setting the pivot point to (0.5, 0.0f), or TOP, means the mask itself (parent object) needs to be translated
			* to the bottom by mask.height / 2 because the pivot point is moved downwards (moving the mask upwards) by half of the mask size.
			* Same thing applies for all the other axes and directions.
			*/

			case ProgressOrientation.TOP:
				this.mask.position -= new Vector3(0, this.fullSize.y / 2 + 0.1f, 0);
				this.mask.pivot = new Vector2(0.5f, 0.0f);
				this.icon.pivot = new Vector2(0.5f, 0.0f);
				break;
			case ProgressOrientation.BOTTOM:
				this.mask.position += new Vector3(0, this.fullSize.y / 2 + 0.1f, 0);
				this.mask.pivot = new Vector2(0.5f, 1.0f);
				this.icon.pivot = new Vector2(0.5f, 1.0f);
				break;
			case ProgressOrientation.LEFT:
				this.mask.position += new Vector3(this.fullSize.x / 2, 0.1f, 0);
				this.mask.pivot = new Vector2(1.0f, 0.5f);
				this.icon.pivot = new Vector2(1.0f, 0.5f);
				break;
			case ProgressOrientation.RIGHT:
				this.mask.position -= new Vector3(this.fullSize.x / 2, 0.1f, 0);
				this.mask.pivot = new Vector2(0.0f, 0.5f);
				this.icon.pivot = new Vector2(0.0f, 0.5f);
				break;
		}
    }

	/// <summary>
	/// Updates the current progress of the operation - updates the progress texture as well, in proportion to task completion.
	/// </summary>
    public void UpdateProgress(int progress)
	{
		this.progress = progress;
		this.progress = this.progress / 100.0f;

		/**
		* Note to future self who's going to forget all these empirical formulas.
		* "Be as big as progress", quoting. Lol.
		* The size of the mask directly reflects progress: 10% -> mask is 1/10 its original size.
		* The position of the icon itself (within the mask) changes by half the progress towards the opposite direction
		* to reflect the new mask size:
		* Progress is 25%. Mask size gets reduced to 1/4.
		* Before changing position, the icon is now positioned at the center of the smaller mask.
		* Moving the icon down by half the new size of the mask, the icon is placed correctly.
		*/

		if (this.orientation == ProgressOrientation.TOP)
		{
			this.mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.fullSize.y * this.progress);
			this.icon.Translate(new Vector3(0, -this.mask.rect.height / 2, 0));
		}
			
		if (this.orientation == ProgressOrientation.BOTTOM)
		{
			this.mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.fullSize.y * this.progress);
			this.icon.Translate(new Vector3(0, this.mask.rect.height / 2, 0));
		}

		if (this.orientation == ProgressOrientation.LEFT)
		{
			this.mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.fullSize.x * this.progress);
			this.icon.Translate(new Vector3(this.mask.rect.width / 2, 0, 0));
		}

		if (this.orientation == ProgressOrientation.RIGHT)
		{
			this.mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.fullSize.x * this.progress);
			this.icon.Translate(new Vector3(-this.mask.rect.width / 2, 0, 0));
		}
	}
}

/// <summary>
/// Defines the progress orientation.
/// </summary>
public enum ProgressOrientation
{
	TOP, BOTTOM, LEFT, RIGHT
}