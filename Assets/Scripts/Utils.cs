namespace Utils 
{
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
	}
}