public static class MatrixUtils
{
	/// <summary>
	/// Determines whether the matrix is empty or not.
	/// </summary>
	public static bool IsMatrixEmpty(object[,] matrix)
	{
		for (int i = 0; i < matrix.GetLength(0); i++)
			for (int j = 0; j < matrix.GetLength(1); j++)
				if (matrix[i,j] != null)
					return false;
		return true;
	}

	/// <summary>
	/// Returns `true` if the given row of the given matrix is null.
	/// </summary>
	public static bool IsMatrixRowEmpty(object[,] matrix, int row)
	{
		for (int j = 0; j < matrix.GetLength(1); j++)
			if (matrix[row, j] != null)
				return false;
		return true;
	}

	/// <summary>
	/// Returns `true` if the given column of the given matrix is null.
	/// </summary>
	public static bool IsMatrixColumnEmpty(object[,] matrix, int column)
	{
		for (int i = 0; i < matrix.GetLength(0); i++)
			if (matrix[i, column] != null)
				return false;
		return true;
	}

	/// <summary>
	/// Shifts the given matrix to the top by one.
	/// </summary>
	public static void UpShiftMatrix(object[,] matrix)
	{
		for (int column = 0; column < matrix.GetLength(1); column++)
		{
			object first = matrix[0,column];
			for (int row = 0; row < matrix.GetLength(0) - 1; row++)
				matrix[row,column] = matrix[row+1,column];
			matrix[matrix.GetLength(1) - 1, column] = first;
		}
	}

	/// <summary>
	/// Shifts the given matrix to the left by one.
	/// </summary>
	public static void LeftShiftMatrix(object[,] matrix)
	{
		for (int row = 0; row < matrix.GetLength(0); row++)
		{
			object first = matrix[row,0];
			for (int column = 0; column < matrix.GetLength(1) - 1; column++)
				matrix[row,column] = matrix[row,column+1];
			matrix[row, matrix.GetLength(1) - 1] = first;
		}
	}

	/// <summary>
	/// Given an input matrix, it prints it to UnityEngine Debug console.
	/// </summary>
	public static void PrintMatrix(object[,] matrix)
	{
		string output = "";
		for (int i = 0; i < matrix.GetLength(0); i++)
		{
			output += i == 0 ? "[ " : "[";
			for (int j = 0; j < matrix.GetLength(1); j++)
				output += (matrix[i,j] == null ? "-" : matrix[i,j]) + " ";
			output += "]";
		}
		UnityEngine.Debug.Log(output);
	}
}