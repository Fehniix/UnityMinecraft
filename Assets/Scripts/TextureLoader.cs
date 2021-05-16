using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void LoadTexture()
	{
		Block block 		= this.GetComponent<Block>();
		string textureName 	= block.textureName == "default" ? block.blockName : block.textureName;

		if (block.hasSidedTextures)
		{
			Texture2D side 		= CachedResources.Load<Texture2D>(string.Format("Textures/{0}_side", textureName));
			Texture2D front 	= CachedResources.Load<Texture2D>(string.Format("Textures/{0}_front", textureName));
			Texture2D top 		= CachedResources.Load<Texture2D>(string.Format("Textures/{0}_top", textureName));
			Texture2D bottom 	= CachedResources.Load<Texture2D>(string.Format("Textures/{0}_bottom", textureName));

			if (front == null)
				front = side;

			if (top == null)
				top = side;

			if (bottom == null)
				bottom = top;

			this.GetComponentInChildren<TopFace>().GetComponent<MeshRenderer>().material.mainTexture 	= top;
			this.GetComponentInChildren<BottomFace>().GetComponent<MeshRenderer>().material.mainTexture = bottom;
			this.GetComponentInChildren<SouthFace>().GetComponent<MeshRenderer>().material.mainTexture 	= front;
			this.GetComponentInChildren<NorthFace>().GetComponent<MeshRenderer>().material.mainTexture 	= side;
			this.GetComponentInChildren<EastFace>().GetComponent<MeshRenderer>().material.mainTexture	= side;
			this.GetComponentInChildren<WestFace>().GetComponent<MeshRenderer>().material.mainTexture 	= side;
		}
		else
		{
			Face[] faces = this.GetComponentsInChildren<Face>();

			foreach (Face face in faces)
			{
				// Load the texture from resources.
				Texture2D t = CachedResources.Load<Texture2D>(string.Format("Textures/{0}", textureName));

				// Set the texture to the target face.
				face.GetComponent<MeshRenderer>().material.mainTexture = t;
			}
		}
	}
}
