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

		//this.test();

		Chunk test = new Chunk();
		
		for (int i = 0; i < 16; i++)
			for (int k = 0; k < 16; k++)
				test.blocks[i,1,k] = "dirt";
		test.blocks[1,1,1] = "cobblestone";

		test.BuildMesh();
    }

    // Update is called once per frame
    void Update()
    {
		
    }

	void test()
	{
		GameObject obj = new GameObject("SuperMesh");
		Texture2D texture = TextureStitcher.instance.StitchedTexture;

		obj.AddComponent<MeshFilter>();
		obj.AddComponent<MeshRenderer>();

		Mesh mesh = new Mesh();

		List<Vector3> vertices	= new List<Vector3>();
		List<Vector2> uvs		= new List<Vector2>();
		List<int> triangles 	= new List<int>();

		vertices.AddRange(CubeMeshFaces.front);
		vertices.AddRange(CubeMeshFaces.top);
		vertices.AddRange(CubeMeshFaces.west);
		vertices.AddRange(CubeMeshFaces.east);

		uvs.AddRange(TextureStitcher.instance.TextureUVs["dirt"].ToArray());
		uvs.AddRange(TextureStitcher.instance.TextureUVs["dirt"].ToArray());
		uvs.AddRange(TextureStitcher.instance.TextureUVs["dirt"].ToArray());
		uvs.AddRange(TextureStitcher.instance.TextureUVs["dirt"].ToArray());

		int[] identityQuad = new int[] {
			0, 1, 3,
			1, 2, 3
		};

		int[] identityQuadInverse = new int[] {
			3, 1, 0,
			3, 2, 1
		};

		triangles.AddRange(identityQuad);
		triangles.AddRange(identityQuad.Add(4));
		triangles.AddRange(identityQuad.Add(8));
		triangles.AddRange(identityQuad.Add(12));

		mesh.vertices 	= vertices.ToArray();
		mesh.uv 		= uvs.ToArray();
		mesh.triangles 	= triangles.ToArray();

		mesh.RecalculateNormals();

		obj.GetComponent<MeshFilter>().mesh = mesh;
		obj.transform.position = new Vector3(3,0,3);

		obj.GetComponent<MeshRenderer>().material.mainTexture = texture;
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
