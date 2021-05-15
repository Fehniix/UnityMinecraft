using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStone : MonoBehaviour
{
    void Start()
    {
		Block block 			= this.GetComponent<Block>();
		block.blockName 		= "stone";
		block.hasSidedTextures	= false;

		this.GetComponent<TextureLoader>().LoadTexture();
    }
}
