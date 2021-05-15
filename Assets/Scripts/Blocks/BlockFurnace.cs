using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFurnace : MonoBehaviour
{
    void Start()
    {
		Block block 			= this.GetComponent<Block>();
		block.blockName 		= "furnace";
		block.hasSidedTextures	= true;

		this.GetComponent<TextureLoader>().LoadTexture();
    }
}
