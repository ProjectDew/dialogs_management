using System;
using System.Collections.Generic;
using UnityEngine;

public static class CSVReader
{
	public static List<List<string>> ReadAsList (TextAsset doc)
	{
		if (doc == null)
			throw new ArgumentNullException ("doc (TextAsset)", "The provided document is null.");

		string content = doc.text;

		if (content == null || content.Length <= 0)
			return default;

		List<string> dataSet = SplitText (content, out int totalRows, out int totalColumns);

		List<List<string>> contentAsTable = new ();

		for (int i = 0; i < totalRows; i++)
		{
			contentAsTable.Add (new List<string> ());

			for (int j = 0; j < totalColumns; j++)
			{
				int currentCell = i * totalColumns + j;
				contentAsTable[i].Add (dataSet[currentCell]);
			}
		}

		return contentAsTable;
	}

	public static string[][] ReadAsArray (TextAsset doc)
	{
		if (doc == null)
			throw new ArgumentNullException ("doc (TextAsset)", "The provided document is null.");

		string content = doc.text;

		if (content == null || content.Length <= 0)
			return default;

		string[] dataSet = SplitText (content, out int totalRows, out int totalColumns).ToArray ();

		string[][] contentAsArray = new string[totalRows][];

		for (int i = 0; i < totalRows; i++)
		{
			contentAsArray[i] = new string[totalColumns];

			for (int j = 0; j < totalColumns; j++)
			{
				int currentCell = i * totalColumns + j;
				contentAsArray[i][j] = dataSet[currentCell];
			}
		}

		return contentAsArray;
	}

	private static List<string> SplitText (string text, out int totalRows, out int totalColumns)
	{
		List<string> splittedColumns;
		
		if (text.Contains (','))
		{
			splittedColumns = SplitColumns (text);

			if (text.Contains ('\n'))
			{
				return SplitRows (splittedColumns, out totalRows, out totalColumns);
			}
			
			totalColumns = GetIndicesOfLastColumns (splittedColumns)[0] + 1;
			totalRows = 1;
		}
		else
		{
			splittedColumns = new ();
			splittedColumns.Add (text);

			if (text.Contains ('\n'))
			{
				return SplitRows (splittedColumns, out totalRows, out totalColumns);
			}

			totalColumns = 1;
			totalRows = 1;
		}

		return splittedColumns;
	}

	private static List<string> SplitColumns (string text)
	{
		List<string> splittedColumns = new ();

		int nextCell;
		int splitIndex = 0, nextIndex = 0;
		int start, length;

		while (splitIndex >= 0)
		{
			start = nextIndex;

			nextCell = text.IndexOf (',', nextIndex);
			
			splitIndex = nextCell;
			nextIndex = splitIndex + 1;

			if (splitIndex >= 0)
				length = splitIndex - start;
			else
				length = text.Length - start;

			if (nextIndex < text.Length)
			{
				if (text[nextIndex] != ' ')
					splittedColumns.Add (text.Substring (start, length));
			}
			else
			{
				if (text[splitIndex] == ',')
					splittedColumns.Add ("");
			}
		}

		return splittedColumns;
	}

	private static List<string> SplitRows (List<string> splittedColumns, out int totalRows, out int totalColumns)
	{
		int[] lastColumns = GetIndicesOfLastColumns (splittedColumns);

		string[] splittedCell = new string[2];

		int breakIndex, currentColumn;
		int secondToLast = lastColumns.Length - 2;

		for (int i = secondToLast; i >= 0; i--)
		{
			currentColumn = lastColumns[i];
			breakIndex = splittedColumns[currentColumn].LastIndexOf ('\n');

			splittedCell[0] = splittedColumns[currentColumn].Substring (0, breakIndex);
			splittedCell[1] = splittedColumns[currentColumn][(breakIndex + 1)..];

			splittedColumns.RemoveAt (currentColumn);

			for (int j = splittedCell.Length - 1; j >= 0; j--)
			{
				splittedColumns.Insert (currentColumn, splittedCell[j]);
			}
		}

		totalColumns = lastColumns[0] + 1;
		totalRows = splittedColumns.Count / totalColumns;

		return TrimText (splittedColumns);
	}

	private static int[] GetIndicesOfLastColumns (List<string> splittedColumns)
	{
		List<int> lastColumns = new ();
		int index;

		for (int i = 0; i < splittedColumns.Count; i++)
		{
			index = i;

			if (index == 0)
				continue;

			lastColumns.Clear ();

			while (index < splittedColumns.Count && splittedColumns[index].Contains ('\n'))
			{
				index += i;
				lastColumns.Add (index - i);
			}

			if (index + 1 == splittedColumns.Count)
			{
				lastColumns.Add (index);
				break;
			}
		}

		return lastColumns.ToArray ();
	}

	private static List<string> TrimText (List<string> splittedText)
	{
		char quot = '\"';

		for (int i = 0; i < splittedText.Count; i++)
		{
			splittedText[i] = splittedText[i].Trim ();

			if (splittedText[i].Contains (quot))
				splittedText[i] = splittedText[i].Trim (quot);
		}

		return splittedText;
	}
}
