using UnityEngine;

namespace Extensions 
{
	public static class TupleExtensions
	{
		/// <summary>
		/// Converts the tuple to a Vector3.
		/// </summary>
		public static Vector3 ToVector3(this (float,float,float) t)
		{
			return new Vector3(t.Item1, t.Item2, t.Item3);
		}

		/// <summary>
		/// Converts the tuple to a Vector3.
		/// </summary>
		public static Vector3 ToVector3(this (double,double,double) t)
		{
			return new Vector3((float)t.Item1, (float)t.Item2, (float)t.Item3);
		}

		/// <summary>
		/// Converts the tuple to a Vector3.
		/// </summary>
		public static Vector3 ToVector3(this (int,int,int) t)
		{
			return new Vector3((float)t.Item1, (float)t.Item2, (float)t.Item3);
		}

		/// <summary>
		/// Converts the tuple to a Vector3Int.
		/// </summary>
		public static Vector3Int ToVector3Int(this (int,int,int) t)
		{
			return new Vector3Int(t.Item1, t.Item2, t.Item3);
		}

		/// <summary>
		/// Converts the tuple to a Vector2.
		/// </summary>
		public static Vector2 ToVector2(this (float,float) t)
		{
			return new Vector2(t.Item1, t.Item2);
		}

		/// <summary>
		/// Converts the tuple to a Vector2.
		/// </summary>
		public static Vector2 ToVector2(this (double,double) t)
		{
			return new Vector2((float)t.Item1, (float)t.Item2);
		}

		/// <summary>
		/// Converts the tuple to a Vector2.
		/// </summary>
		public static Vector2 ToVector2(this (int,int) t)
		{
			return new Vector2((float)t.Item1, (float)t.Item2);
		}
	}

	public static class ArrayExtension
	{
		/// <summary>
		/// Multiplies each integer element of the array by the provided integer. Returns a copy of the original array.
		/// </summary>
		public static int[] MultiplyBy(this int[] t, int n)
		{
			int[] arr = new int[t.Length];
			for (int i = 0; i < t.Length; i++)
				arr[i] = t[i] * n;
			return arr;
		}

		/// <summary>
		/// Adds the given integer to each element of the integer array. Returns a copy of the original array.
		/// </summary>
		public static int[] Add(this int[] t, int n)
		{
			int[] arr = new int[t.Length];
			for (int i = 0; i < t.Length; i++)
				arr[i] = t[i] + n;
			return arr;
		}
	}

	public static class VectorExtension
	{
		/// <summary>
		/// Adds the (x,y,z) float tuple to all components of these vectors. Returns a copy of the original vectors.
		/// </summary>
		public static Vector3[] Add(this Vector3[] vectors, (float,float,float) n)
		{
			Vector3[] v = new Vector3[vectors.Length];
			for (int i = 0; i < vectors.Length; i++)
				v[i] = vectors[i] + n.ToVector3();
			return v;
		}

		/// <summary>
		/// Adds `n` to all components of the Vector3.
		/// </summary>
		public static Vector3 FloorAdd(this Vector3 v, float n)
		{
			return new Vector3(
				Mathf.FloorToInt(v.x) + n, 
				Mathf.FloorToInt(v.y) + n,
				Mathf.FloorToInt(v.z) + n
			);
		}
	}

	public static class MaterialExtensions
	{
		public static void ToOpaqueMode(this Material material)
		{
			material.SetOverrideTag("RenderType", "");
			material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
			material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
			material.SetInt("_ZWrite", 1);
			material.DisableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = -1;
		}
	
		public static void ToFadeMode(this Material material)
		{
			material.SetOverrideTag("RenderType", "Cutout");
			material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
			material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			material.SetInt("_ZWrite", 0);
			material.DisableKeyword("_ALPHATEST_ON");
			material.EnableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
		}
	}
}