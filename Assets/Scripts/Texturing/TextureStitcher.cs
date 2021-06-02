using System.Collections.Generic;
using UnityEngine;

public class TextureStitcher : MonoBehaviour
{
	/// <summary>
	/// For a given texture name, associates the UV mapping on the stitched texture atlas for an ordered set of clockwise vertices.
	/// </summary>
	private Dictionary<string, List<Vector2>> textureUVs;

	/// <summary>
	/// For a given texture name, associates the UV mapping on the stitched texture atlas for an ordered set of clockwise vertices.
	/// </summary>
	public Dictionary<string, List<Vector2>> TextureUVs {
		get { return this.textureUVs; }
		private set {}
	}

	/// <summary>
	/// Private instance of the stitched texture.
	/// </summary>
	private Texture2D stitchedTexture;

	public Texture2D StitchedTexture {
		get { return this.stitchedTexture; }
		private set {}
	}

	/// <summary>
	/// Static instance of the class to be accessed globally.
	/// </summary>
	public static TextureStitcher instance;

    // Start is called before the first frame update
    void Start()
    {
		// Instantiate the dictionary.
		this.textureUVs = new Dictionary<string, List<Vector2>>();

		// Load recursively all textures.
        Texture2D[] textures = Resources.LoadAll<Texture2D>("Textures/Stitch");

		// Initialize the dictionary.
		foreach (Texture2D texture in textures)
			this.textureUVs[texture.name] = new List<Vector2>();

		this.StitchTextures(textures);

		TextureStitcher.instance = this;
    }

	/// <summary>
	/// Given all textures in Resources/Textures, stitch them all together in a single texture atlas and calculate UVs.
	/// </summary>
	private void StitchTextures(Texture2D[] textures)
	{
		int maxWidth = 0, maxHeight = 0;
		foreach (Texture2D texture in textures)
		{
			if (texture.width > maxWidth)
				maxWidth = texture.width;
			if (texture.height > maxHeight)
				maxHeight = texture.height;
		}

		// Get the bigger of the two dimensions to give enough space to the biggest texture.
		// The square root of this "square side" represents the number of rows and columns of textures
		// in the final stitched texture.
		// Ceiling this value ensures every possible pixel is given to the biggest texture.
		int squareSize 	= Mathf.Max(maxWidth, maxHeight);
		int sideCount 	= Mathf.CeilToInt(Mathf.Sqrt(textures.Length));
		int rowSize 	= sideCount * squareSize;

		Texture2D finalTexture = new Texture2D(squareSize * sideCount, squareSize * sideCount, TextureFormat.RGBA32, false, false);
		finalTexture.filterMode = FilterMode.Point;

		for (int z = 0; z < textures.Length; z++)
		{
			int xCoord = z % sideCount;
			int yCoord = z / sideCount;

			// Loop through each individual pixel of the texture and bake it onto the final texture
			for (int i = 0; i < textures[z].width; i++)
				for (int j = 0; j < textures[z].height; j++)
				{
					Color pixel = textures[z].GetPixel(i,j);
					finalTexture.SetPixel(i + squareSize * xCoord, j + squareSize * yCoord, pixel);
				}

			// Allows to slightly adjust the UV coordinates to not include the empty part of the stitched texture.
			float whiteBordersAdjustment = 0.00039f;

			// Adding squareSize moves the UV coordinate to the right
			// Calculate the 0,0 UV.
			this.textureUVs[textures[z].name].Add(new Vector2(
				xCoord * squareSize / (float)rowSize + whiteBordersAdjustment, 
				yCoord * squareSize / (float)rowSize + whiteBordersAdjustment
			));

			// Calculate the 0,1 UV.
			this.textureUVs[textures[z].name].Add(new Vector2(
				xCoord * squareSize / (float)rowSize + whiteBordersAdjustment,
				(yCoord * squareSize + squareSize) / (float)rowSize - whiteBordersAdjustment
			));

			// Calculate the 1,1 UV.
			this.textureUVs[textures[z].name].Add(new Vector2(
				(xCoord * squareSize + squareSize) / (float)rowSize - whiteBordersAdjustment,
				(yCoord * squareSize + squareSize) / (float)rowSize - whiteBordersAdjustment
			));

			// Calculate the 1,0 UV.
			this.textureUVs[textures[z].name].Add(new Vector2(
				(xCoord * squareSize + squareSize) / (float)rowSize - whiteBordersAdjustment,
				yCoord * squareSize / (float)rowSize + whiteBordersAdjustment
			));
		}

		finalTexture.Apply();

		this.stitchedTexture = finalTexture;
	}

	/// <summary>
	/// Given a blockName, returns a Sprite representing the block item.
	/// </summary>
	public Sprite GetBlockItemSprite(string blockName)
	{
		object instantiatedObject 	= Registry.Instantiate(blockName);
		Block block 				= instantiatedObject as Block;
		Item item 					= instantiatedObject as Item;
		
		string textureName	= block?.blockName;
		string texturePath	= item != null ? "Items" : "Stitch";

		if (item != null)
			textureName = item.itemTextureName;

		if (block != null && block.textureName != "default")
			textureName = block.textureName;

		if (block != null && block.hasSidedTextures == true)
			if (TextureUVs.ContainsKey(System.String.Format("{0}_{1}", textureName, "front")))
				textureName = System.String.Format("{0}_{1}", textureName, "front");
			else
				textureName = System.String.Format("{0}_{1}", textureName, "side");

		Texture2D tex = CachedResources.Load<Texture2D>(System.String.Format("Textures/{0}/{1}", texturePath, textureName));

		return Sprite.Create(
			tex, 
			new Rect(0.0f, 0.0f, tex.width, tex.height), 
			new Vector2(0.5f, 0.5f)
		);
	}
}
