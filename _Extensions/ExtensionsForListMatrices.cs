namespace ExtensionMethods
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	public static class ExtensionsForListMatrices
	{
		private static readonly ArgumentException nullOrEmptyList = new ("The provided list is null or empty.", "item (IList)");
		private static readonly IndexOutOfRangeException indexOutOfRange = new ("Index is out of range.");

		public static int ColumnCount<T> (this List<List<T>> matrix)
		{
			if (matrix.IsNullOrEmpty () || matrix[0] == null)
				return 0;

			return matrix[0].Count;
		}

		public static List<T> GetColumn<T> (this List<List<T>> matrix, int index)
		{
			if (index < 0 || index >= matrix.ColumnCount ())
				throw indexOutOfRange;

			List<T> columns = new ();

			for (int i = 0; i < matrix.Count; i++)
			{
				columns.Add (matrix[i][index]);
			}

			return columns;
		}

		public static void AddColumn<T> (this List<List<T>> matrix, List<T> item)
		{
			if (item.IsNullOrEmpty ())
				throw nullOrEmptyList;

			for (int i = 0; i < matrix.Count; i++)
			{
				if (i < item.Count)
					matrix[i].Add (item[i]);
				else
					matrix[i].Add (default);
			}
		}

		public static void InsertColumn<T> (this List<List<T>> matrix, int index, List<T> item)
		{
			if (item.IsNullOrEmpty ())
				throw nullOrEmptyList;
		
			if (index < 0 || index >= matrix.ColumnCount ())
				throw indexOutOfRange;

			for (int i = 0; i < matrix.Count; i++)
			{
				if (i < item.Count)
					matrix[i].Insert (index, item[i]);
				else
					matrix[i].Insert (index, default);
			}
		}

		public static void ReplaceColumn<T> (this List<List<T>> matrix, int index, List<T> item)
		{
			if (index < 0 || index >= matrix.ColumnCount ())
				throw indexOutOfRange;

			for (int i = 0; i < matrix.Count; i++)
			{
				if (i < item.Count)
					matrix[i][index] = item[i];
				else
					matrix[i][index] = default;
			}
		}

		public static void RemoveColumn<T> (this List<List<T>> matrix, int index)
		{
			if (index < 0 || index >= matrix.ColumnCount ())
				throw indexOutOfRange;
		
			for (int i = 0; i < matrix.Count; i++)
			{
				matrix[i].RemoveAt (index);
			}
		}

		public static T GetEntry<T> (this List<List<T>> matrix, int row, int column)
		{
			if (row < 0 || row >= matrix.Count)
				throw indexOutOfRange;

			if (column < 0 || column >= matrix.ColumnCount ())
				throw indexOutOfRange;

			return matrix[row][column];
		}

		public static void SetEntry<T> (this List<List<T>> matrix, int row, int column, T value)
		{
			if (row < 0 || row >= matrix.Count)
				throw indexOutOfRange;

			if (column < 0 || column >= matrix.ColumnCount ())
				throw indexOutOfRange;

			matrix[row][column] = value;
		}

		public static Vector2Int CoordinatesOf<T> (this List<List<T>> matrix, T cellContent, int startingRow, int startingColumn)
		{
			Vector2Int defaultCoordinates = new (-1, -1);

			if (startingRow < 0 || startingRow >= matrix.Count)
				return defaultCoordinates;
		
			if (startingColumn < 0 || startingColumn >= matrix.ColumnCount ())
				return defaultCoordinates;

			for (int i = startingRow; i < matrix.Count; i++)
			{
				for (int j = startingColumn; j < matrix.ColumnCount (); j++)
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
