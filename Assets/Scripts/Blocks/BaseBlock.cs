public class BaseBlock
{
    /// <summary>
	///Describes the name of the block. The block texture depends on it.
	/// </summary>
	public string blockName;

	/// <summary>
	///Describes the name of the texture. If not set, defaults to the block name.
	/// </summary>
	public string textureName = "default";

	/// <summary>
	///If set to false, one texture is used for all the block's faces.
	/// </summary>
	public bool hasSidedTextures = false;
}
