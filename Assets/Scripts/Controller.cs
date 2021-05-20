using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class Controller : MonoBehaviour
{
	public GameObject cube;
	public Texture2D testTexture;
    // Start is called before the first frame update
    void Start()
    {
		GameObject block = CachedResources.Load<GameObject>("Prefabs/Blocks/Block.Dirt");
		GameObject instantiatedBlock = Instantiate<GameObject>(block, new Vector3(0,0,0), Quaternion.identity);

		Chunk test = new Chunk();

		for (int i = 0; i < 256; i++)
			test.blocks[Random.Range(0, 16), Random.Range(0, 32), Random.Range(0, 16)] = "furnace";
		
		test.x = 5;
		test.z = 5;
		test.BuildMesh();
    }

    // Update is called once per frame
    void Update()
    {
		
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
