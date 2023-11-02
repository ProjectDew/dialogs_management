namespace ExtensionMethods
{
	using System.Collections.Generic;

	public static class Extensions
	{

		public static double Constrain (this double value, double minValue, double maxValue = double.MaxValue)
		{
			if (value < minValue)
				value = minValue;

			if (value > maxValue)
				value = maxValue;

			return value;
		}

		public static float Constrain (this float value, float minValue, float maxValue = float.MaxValue)
		{
			if (value < minValue)
				value = minValue;

			if (value > maxValue)
				value = maxValue;

			return value;
		}

		public static int Constrain (this int value, int minValue, int maxValue = int.MaxValue)
		{
			if (value < minValue)
				value = minValue;

			if (value > maxValue)
				value = maxValue;

			return value;
		}

		public static long Constrain (this long value, long minValue, long maxValue = long.MaxValue)
		{
			if (value < minValue)
				value = minValue;

			if (value > maxValue)
				value = maxValue;

			return value;
		}

		public static string NonNullable (this string value)
		{
			if (value == null)
			{
				return string.Empty;
			}

			return value;
		}

		public static bool IsNullOrEmpty (this string value)
		{
			if (value != null)
				return value.Length == 0;

			return true;
		}

		public static bool IsNullOrEmpty<T> (this IList<T> value)
		{
			if (value != null)
				return value.Count == 0;

			return true;
		}

		public static void Move<T> (this IList<T> list, int indexFrom, int indexTo)
		{
			if (indexFrom == indexTo)
				return;

			T objectToMove = list[indexFrom];

			if (indexFrom < indexTo)
			{
				for (int i = indexFrom; i < indexTo; i++)
				{
					int next = i + 1;
					list[i] = list[next];
				}
			}
			else
			{
				for (int i = indexFrom; i > indexTo; i--)
				{
					int next = i - 1;
					list[i] = list[next];
				}
			}

			list[indexTo] = objectToMove;
		}
	}
}
