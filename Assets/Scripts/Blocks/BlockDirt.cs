using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDirt : MonoBehaviour
{
    void Start()
    {
		Block block 			= this.GetComponent<Block>();
		block.blockName 		= "dirt";
		block.hasSidedTextures	= false;
		block.breakDelegate		= this.Break;
		
		this.GetComponent<TextureLoader>().LoadTexture();
    }

	void Break()
	{
		Debug.Log("Dirt block broken");
	}
}
