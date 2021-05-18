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
}