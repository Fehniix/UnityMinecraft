using UnityEngine;

public class BlockRegistrar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Blocks.RegisterBlock<Cobblestone>("cobblestone");
		Blocks.RegisterBlock<Bedrock>("bedrock");
    }
}
