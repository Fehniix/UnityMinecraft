using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	public GameObject cube;
	public Texture2D testTexture;
    // Start is called before the first frame update
    void Start()
    {
		GameObject block = CachedResources.Load<GameObject>("Prefabs/Blocks/Block.Dirt");
		GameObject b2 = CachedResources.Load<GameObject>("Prefabs/Blocks/Block.Cobblestone");
		
		GameObject instantiatedBlock = Instantiate<GameObject>(block, new Vector3(0,0,0), Quaternion.identity);

		//this.CombineShenanigans();
		
		this.test();

		// for (int i = 0; i < 16; i++)
		// 	for (int k = 0; k < 16; k++)
		// 		Instantiate<GameObject>(b2, new Vector3(i,0,k), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
		
    }

	void test()
	{
		GameObject obj = new GameObject("triangle");
		Texture2D texture = TextureStitcher.instance.StitchedTexture;

		obj.AddComponent<MeshFilter>();
		obj.AddComponent<MeshRenderer>();

		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[] {
			new Vector3(0,0,0),
			new Vector3(1,0,1),
			new Vector3(1,0,0),
			new Vector3(0,0,1)
		};

		mesh.triangles = new int[] {
			3, 2, 0,
			3, 1, 2
		};

		mesh.uv = TextureStitcher.instance.TextureUVs["dirt"].ToArray();
		mesh.RecalculateNormals();

		printArray(mesh.uv);
		Debug.Log(texture);

		obj.GetComponent<MeshFilter>().mesh = mesh;
		obj.transform.position = new Vector3(3,0,3);

		obj.GetComponent<MeshRenderer>().material.mainTexture = texture;
	}

	void ExtremeCombineShenanigans()
	{
		// Stitch furnace texture.
		Texture2D front = CachedResources.Load<Texture2D>("Textures/furnace_front");
		Texture2D side = CachedResources.Load<Texture2D>("Textures/furnace_side");
		Texture2D top = CachedResources.Load<Texture2D>("Textures/furnace_top");

		Texture2D final = new Texture2D(front.width * 3, front.height * 4);

		
	}

	Texture2D StitchTexture(string name)
	{
		Texture2D front = CachedResources.Load<Texture2D>(System.String.Format("Textures/{0}_front", name));
		Texture2D side = CachedResources.Load<Texture2D>(System.String.Format("Textures/{0}_side", name));
		Texture2D top = CachedResources.Load<Texture2D>(System.String.Format("Textures/{0}_top", name));

		if (top == null)
			top = side;

		Texture2D final = new Texture2D(front.width * 4, front.height * 3, TextureFormat.RGBA32, false, false);
		final.filterMode = FilterMode.Point;

		// Set the final texture's default pixels to Color.clear
		Color[] finalPixels = final.GetPixels();
		for (int i = 0; i < finalPixels.Length; i++)
			finalPixels[i] = Color.clear;

		// Top face stitching
		for (int i = 0; i < top.width; i++)
			for (int j = 0; j < top.height; j++)
			{
				Color topPixel = top.GetPixel(i, j);
				Color sidePixel = side.GetPixel(i, j);
				Color frontPixel = front.GetPixel(i, j);

				int size = top.width;

				// Top face
				final.SetPixel(i + size, j + size * 2, topPixel);

				// Bottom face
				final.SetPixel(i + size, j, topPixel);

				// Back face
				final.SetPixel(i, j + size, sidePixel);

				// West face
				final.SetPixel(i + size, j + size, sidePixel);

				// Front face
				final.SetPixel(i + size * 2, j + size, frontPixel);

				// East face
				final.SetPixel(i + size * 3, j + size, sidePixel);
			}

		
		final.Apply(false, false);
		this.testTexture = final;

		return final;
	}

	void CombineShenanigans()
	{
		GameObject furnace = CachedResources.Load<GameObject>("Prefabs/Blocks/Block.Furnace");
		GameObject[] objsToCombine = new GameObject[3];
		objsToCombine[0] = Instantiate<GameObject>(furnace, new Vector3(0,0,3), Quaternion.identity);

		// Store the instantiated object's transform
		Vector3 oldPos = objsToCombine[0].transform.position;
		Quaternion oldRot = objsToCombine[0].transform.rotation;

		// Set the object's transform to identity to ease transformation to world coords
		objsToCombine[0].transform.position = Vector3.zero;
		objsToCombine[0].transform.rotation = Quaternion.identity;
		
		// Get all the quads' MeshFilters
		MeshFilter[] filters = objsToCombine[0].GetComponentsInChildren<MeshFilter>();

		// Instantiate the combiners
		CombineInstance[] combiners = new CombineInstance[filters.Length];

		Mesh finalMesh = new Mesh();

		for (int i = 0; i < filters.Length; i++)
		{
			combiners[i].mesh = filters[i].mesh;
			combiners[i].transform = filters[i].transform.localToWorldMatrix;
		}

		finalMesh.CombineMeshes(combiners);

		//printArray(finalMesh.vertices);

		objsToCombine[0].GetComponent<MeshFilter>().mesh = finalMesh;
		objsToCombine[0].GetComponentInChildren<BlockFaces>().gameObject.SetActive(false);

		objsToCombine[0].transform.position = oldPos;
		objsToCombine[0].transform.rotation = oldRot;

		Texture2D texture = this.StitchTexture("furnace");

		objsToCombine[0].GetComponent<MeshRenderer>().material.mainTexture = texture;
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
