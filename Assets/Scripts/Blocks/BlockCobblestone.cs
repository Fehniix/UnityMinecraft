using UnityEngine;

public class BlockCobblestone : MonoBehaviour
{
    void Start()
    {
		Block block 				= this.GetComponent<Block>();
		block.blockName 			= "cobblestone";
		block.hasSidedTextures		= false;
		block.hardness		 		= 2 * 20;
		block.breakDelegate 		= this.Break;
		block.beganBreakingDelegate = this.BeganBreaking;

		this.GetComponent<TextureLoader>().LoadTexture();
    }

	void BeganBreaking()
	{
		//TODO Access currently active item to modify the block's hardness.
		Debug.Log("Began breaking");
	}

	void Break()
	{
		Destroy(this.gameObject);
	}
}
