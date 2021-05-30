using UnityEngine;

public static class BlockItem
{
	/// <summary>
	/// Renders a block item to the screen.
	/// </summary>
	public static void Render(string blockName, Transform parent, Vector3 position)
	{
		GameObject blockItem 				= BlockBuilder.Build(blockName, true);
		blockItem.transform.parent 			= parent;
		blockItem.transform.localPosition 	= new Vector3(0.0f + position.x, 0.0f + position.y, -27.0f);
		blockItem.transform.localRotation 	= Quaternion.Euler(0.0f, -45.0f, 0.0f);
		blockItem.transform.localScale 		= new Vector3(17.0f, 17.0f, 17.0f);
		blockItem.layer 					= 8;
	}
}