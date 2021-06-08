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
	private RectTransform hidePanelTransform;

    // Start is called before the first frame update
    void Start()
    {
		this.hidePanelTransform = this.transform.GetChild(0).GetComponent<RectTransform>();

		this.fullSize = new Vector2(
			this.hidePanelTransform.rect.width, 
			this.hidePanelTransform.rect.height
		);

		switch(this.orientation)
		{
			case ProgressOrientation.TOP:
				this.hidePanelTransform.position += new Vector3(0, this.fullSize.y / 2, 0);
				this.hidePanelTransform.pivot = new Vector2(0.5f, 1.0f);
				break;
			case ProgressOrientation.BOTTOM:
				this.hidePanelTransform.position -= new Vector3(0, this.fullSize.y / 2, 0);
				this.hidePanelTransform.pivot = new Vector2(0.5f, 0.0f);
				break;
			case ProgressOrientation.LEFT:
				this.hidePanelTransform.position -= new Vector3(this.fullSize.x / 2, 0, 0);
				this.hidePanelTransform.pivot = new Vector2(0.0f, 0.5f);
				break;
			case ProgressOrientation.RIGHT:
				this.hidePanelTransform.position += new Vector3(this.fullSize.x / 2, 0, 0);
				this.hidePanelTransform.pivot = new Vector2(1.0f, 0.5f);
				break;
		}
    }

	/// <summary>
	/// Updates the current progress of the operation - updates the progress texture as well, in proportion to task completion.
	/// </summary>
    public void UpdateProgress(int progress)
	{
		this.progress = progress;
		this.progress /= 100.0f;

		if (this.orientation == ProgressOrientation.TOP)
			this.hidePanelTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.fullSize.y - this.fullSize.y * this.progress);

		if (this.orientation == ProgressOrientation.BOTTOM)
			this.hidePanelTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.fullSize.y - this.fullSize.y * this.progress);

		if (this.orientation == ProgressOrientation.LEFT)
			this.hidePanelTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.fullSize.x - this.fullSize.x * this.progress);

		if (this.orientation == ProgressOrientation.RIGHT)
			this.hidePanelTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.fullSize.x - this.fullSize.x * this.progress);
	}
}

/// <summary>
/// Defines the progress orientation.
/// </summary>
public enum ProgressOrientation
{
	TOP, BOTTOM, LEFT, RIGHT
}