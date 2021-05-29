using UnityEngine;

public class Registrar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		this.RegisterBlocks();
		this.RegisterItems();
    }

	void RegisterBlocks()
	{
		Blocks.RegisterBlock<Air>("air");
        Blocks.RegisterBlock<Cobblestone>("cobblestone");
		Blocks.RegisterBlock<Stone>("stone");
		Blocks.RegisterBlock<Dirt>("dirt");
		Blocks.RegisterBlock<Grass>("grass");
		Blocks.RegisterBlock<Bedrock>("bedrock");
	}

	void RegisterItems()
	{
		
	}
}
