using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCobblestone : MonoBehaviour
{
    void Start()
    {
		Block block 			= this.GetComponent<Block>();
		block.blockName 		= "cobblestone";
		block.hasSidedTextures	= false;
		block.hardness		 	= 2 * 20;
		block.breakDelegate 	= this.Break;

		this.GetComponent<TextureLoader>().LoadTexture();
    }

	void Break()
	{
		Debug.Log("BLOCK BROKEN!");
	}
}
