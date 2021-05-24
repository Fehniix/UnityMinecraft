using UnityEngine;

public class BlockRegistrar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Blocks.RegisterBlock<Cobblestone>("cobblestone");
		Blocks.RegisterBlock<Stone>("stone");
		Blocks.RegisterBlock<Dirt>("dirt");
		Blocks.RegisterBlock<Grass>("grass");
		Blocks.RegisterBlock<Bedrock>("bedrock");
    }
}
