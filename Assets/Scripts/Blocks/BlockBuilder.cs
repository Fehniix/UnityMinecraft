using System;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public static class BlockBuilder
{
	/// <summary>
	/// Used to build a quad's triangles.
	/// </summary>
	private static int[] identityQuad = new int[] {
		0, 1, 3,
		1, 2, 3
	};

	/// <summary>
	/// Given a block name, creates a new block entity.
	/// </summary>
    public static GameObject Build(string blockName, bool meshOnly = false)
	{
		Block block 		= Registry.Instantiate(blockName) as Block;
		GameObject entity 	= new GameObject(System.String.Format("entity_{0}", blockName));
		Texture2D texture 	= TextureStitcher.instance.StitchedTexture;

		List<Vector3> vertices 	= new List<Vector3>();
		List<Vector2> uvs		= new List<Vector2>();
		List<int> triangles		= new List<int>();

		entity.AddComponent<MeshFilter>();
		entity.AddComponent<MeshRenderer>();
		entity.GetComponent<MeshRenderer>().material.mainTexture = texture;

		Mesh mesh = new Mesh();

		// Add top face.
		vertices.AddRange(CubeMeshFaces.top);
		uvs.AddRange(BlockBuilder.TextureUVSelector(block, "top"));

		// Add bottom face.
		vertices.AddRange(CubeMeshFaces.bottom);
		uvs.AddRange(BlockBuilder.TextureUVSelector(block, "bottom"));

		// Add front face.
		vertices.AddRange(CubeMeshFaces.front);
		uvs.AddRange(BlockBuilder.TextureUVSelector(block, "front"));

		// Add back face.
		vertices.AddRange(CubeMeshFaces.back);
		uvs.AddRange(BlockBuilder.TextureUVSelector(block, "back"));

		// Add east face.
		vertices.AddRange(CubeMeshFaces.east);
		uvs.AddRange(BlockBuilder.TextureUVSelector(block, "east"));

		// Add west face.
		vertices.AddRange(CubeMeshFaces.west);
		uvs.AddRange(BlockBuilder.TextureUVSelector(block, "west"));

		// Add all triangles.
		for (int i = 0; i < 6; i++)
			triangles.AddRange(BlockBuilder.identityQuad.Add(i * 4));

		mesh.vertices 	= vertices.ToArray();
		mesh.uv 		= uvs.ToArray();
		mesh.triangles 	= triangles.ToArray();

		mesh.RecalculateNormals();

		entity.GetComponent<MeshFilter>().mesh = mesh;

		if (!meshOnly)
		{
			entity.AddComponent<MeshCollider>();
			entity.AddComponent<SphereCollider>();
			entity.AddComponent<Rigidbody>();
			entity.AddComponent<Entity>();

			entity.GetComponent<MeshCollider>().sharedMesh 	= mesh;
			entity.GetComponent<MeshCollider>().convex 		= true;
			entity.GetComponent<MeshCollider>().material	= CachedResources.Load<PhysicMaterial>("SuperFriction");

			entity.GetComponent<SphereCollider>().center	= new Vector3(0.5f, 0.5f, 0.5f);
			entity.GetComponent<SphereCollider>().radius	= 4.0f;
			entity.GetComponent<SphereCollider>().isTrigger	= true;

			entity.GetComponent<Entity>().entityName 		= blockName;

			entity.GetComponent<Rigidbody>().constraints 	= RigidbodyConstraints.FreezeRotation;
		}

		entity.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

		return entity;
	}

	/// <summary>
	/// Given the block reference and a face name, returns the UVs of the provided face.
	/// </summary>
	private static Vector2[] TextureUVSelector(Block block, string faceName)
	{
		string textureName = block.blockName;

		if (block.textureName != "default")
			textureName = block.textureName;

		if (block.hasSidedTextures)
		{
			if (TextureStitcher.instance.TextureUVs.ContainsKey(System.String.Format("{0}_{1}", textureName, faceName)))
				textureName = System.String.Format("{0}_{1}", textureName, faceName);
			else
				textureName = System.String.Format("{0}_{1}", textureName, "side");	
		}

		return TextureStitcher.instance.TextureUVs[textureName].ToArray();
	}
}
