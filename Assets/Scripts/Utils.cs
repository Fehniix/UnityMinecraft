public static class Utils
{
	/// <summary>
	/// Creates a new instance of the supplied type.
	/// </summary>
	/// Footnote: `where T: new()` is a type conditional clause. 
	/// It forces (by runtime checking) T to necessarily include a constructor.
	public static T CreateInstance<T>() where T: new()
	{
		return new T();
	}

	/// <summary>
	/// Given the input `Vector3`, floors each component to the nearest int and returns a `Vector3Int`.
	/// </summary>
	public static UnityEngine.Vector3Int ToVectorInt(UnityEngine.Vector3 v)
	{
		return new UnityEngine.Vector3Int(
			UnityEngine.Mathf.FloorToInt(v.x),
			UnityEngine.Mathf.FloorToInt(v.y),
			UnityEngine.Mathf.FloorToInt(v.z)
		);
	}

	/// <summary>
	/// Given the input `Vector3`, floors each component and returns a new `Vector3`.
	/// </summary>
	public static UnityEngine.Vector3 FloorVector3(UnityEngine.Vector3 v)
	{
		return new UnityEngine.Vector3(
			UnityEngine.Mathf.Floor(v.x),
			UnityEngine.Mathf.Floor(v.y),
			UnityEngine.Mathf.Floor(v.z)
		);
	}
}