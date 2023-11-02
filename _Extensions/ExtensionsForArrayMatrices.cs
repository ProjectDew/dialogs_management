namespace ExtensionMethods
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	public static class ExtensionsForArrayMatrices
	{
		private static readonly IndexOutOfRangeException indexOutOfRange = new ("Index is out of range.");

		public static int ColumnLength<T> (this T[][] matrix)
		{
			if (matrix.IsNullOrEmpty () || matrix[0] == null)
				return 0;

			return matrix[0].Length;
		}

		public static IList<T> GetColumn<T> (this T[][] matrix, int index)
		{
			if (index < 0 || index >= matrix.ColumnLength ())
				throw indexOutOfRange;

			IList<T> columns = new T[matrix.Length];

			for (int i = 0; i < matrix.Length; i++)
			{
				columns[i] = matrix[i][index];
			}

			return columns;
		}

		public static void ReplaceColumn<T> (this T[][] matrix, int index, IList<T> item)
		{
			if (index < 0 || index >= matrix.ColumnLength ())
				throw indexOutOfRange;

			for (int i = 0; i < matrix.Length; i++)
			{
				if (i < item.Count)
					matrix[i][index] = item[i];
				else
					matrix[i][index] = default;
			}
		}

		public static T GetEntry<T> (this T[][] matrix, int row, int column)
		{
			if (row < 0 || row >= matrix.Length)
				throw indexOutOfRange;

			if (column < 0 || column >= matrix.ColumnLength ())
				throw indexOutOfRange;

			return matrix[row][column];
		}

		public static void SetEntry<T> (this T[][] matrix, int row, int column, T value)
		{
			if (row < 0 || row >= matrix.Length)
				throw indexOutOfRange;

			if (column < 0 || column >= matrix.ColumnLength ())
				throw indexOutOfRange;

			matrix[row][column] = value;
		}

		public static Vector2Int CoordinatesOf<T> (this T[][] matrix, T cellContent, int startingRow, int startingColumn)
		{
			Vector2Int defaultCoordinates = new (-1, -1);

			if (startingRow < 0 || startingRow >= matrix.Length)
				return defaultCoordinates;
		
			if (startingColumn < 0 || startingColumn >= matrix.ColumnLength ())
				return defaultCoordinates;

			for (int i = startingRow; i < matrix.Length; i++)
			{
				for (int j = startingColumn; j < matrix.ColumnLength (); j++)
				{
					if (!CompareGenerics (matrix[i][j], cellContent))
						continue;

					return new Vector2Int (i, j);
				}
			}

			return defaultCoordinates;
		}

		private static bool CompareGenerics<T> (T itemA, T itemB)
		{
			if (itemA == null)
			{
				if (itemB == null)
					return true;
			
				return false;
			}
			else
			{
				EqualityComparer<T> c = EqualityComparer<T>.Default;

				if (c.Equals (itemA, itemB))
					return true;

				return false;
			}
		}
	}
}
