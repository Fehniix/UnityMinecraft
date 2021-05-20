using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		this.RegisterBlocks();

		Chunk test = new Chunk();

		for (int i = 0; i < 256; i++)
			test.blocks[Random.Range(0, 16), Random.Range(0, 32), Random.Range(0, 16)] = "stone";
		
		test.x = 5;
		test.z = 5;
		test.BuildMesh();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			Block test = Blocks.Spawn("cobblestone");
			Debug.Log(test);
		}
    }

	void RegisterBlocks()
	{
		Blocks.RegisterBlock<Cobblestone>("cobblestone");
	}
	
	void printArray<T>(T[] arr)
	{
		string output = "[ ";
		foreach(T el in arr)
		{
			output += el + " ";
		}
		output += "]";
		Debug.Log(output);
	}
}
