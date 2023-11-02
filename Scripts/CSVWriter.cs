using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CSVWriter
{
	public CSVWriter () { }

	public CSVWriter (string folder)
	{
		currentFolder = folder;
	}

	private readonly string currentFolder = Application.persistentDataPath;

	private readonly Exception nullFileName = new ArgumentNullException ("fileName (string)", "The file name is null.");
	private readonly Exception nullOrEmptyKeyword = new ArgumentException ("The keyword is null or empty.", "keyword (string)");
	private readonly Exception noDataProvided = new ArgumentException ("No data provided.", "data (string array)");

	public void SaveData (string fileName, params IReadOnlyCollection<string>[] data)
	{
		if (fileName == null)
			throw nullFileName;

		if (data == null || data.Length <= 0)
			throw noDataProvided;

		StringBuilder sb = new (GetFullDocument (fileName));

		sb = WriteData (sb, data);
		SaveToFile (sb.ToString (), fileName);
	}

	public bool TrySaveData (string fileName, string keyword, params IReadOnlyCollection<string>[] data)
	{
		if (fileName == null)
			throw nullFileName;

		if (keyword == null || keyword == "")
			throw nullOrEmptyKeyword;

		if (data == null || data.Length <= 0)
			throw noDataProvided;
		
		string fullDocument = GetFullDocument (fileName);

		if (GetData (fullDocument, keyword) == "")
		{
			StringBuilder sb = new (fullDocument);

			sb = WriteData (sb, data);
			SaveToFile (sb.ToString (), fileName);

			return true;
		}

		return false;
	}

	public bool TryRemoveData (string fileName, string keyword)
	{
		if (fileName == null)
			throw nullFileName;

		if (keyword == null || keyword == "")
			throw nullOrEmptyKeyword;
		
		string fullDocument = GetFullDocument (fileName);
		string dataToReplace = GetData (fullDocument, keyword);

		if (dataToReplace != "")
		{
			StringBuilder sb = new (fullDocument);

			sb = sb.Replace (dataToReplace, "");
			SaveToFile (sb.ToString (), fileName);

			return true;
		}

		return false;
	}

	public bool TryReplaceData (string fileName, string keyword, params IReadOnlyCollection<string>[] data)
	{
		if (fileName == null)
			throw nullFileName;

		if (keyword == null || keyword == "")
			throw nullOrEmptyKeyword;

		if (data == null || data.Length <= 0)
			throw noDataProvided;

		string fullDocument = GetFullDocument (fileName);
		string dataToReplace = GetData (fullDocument, keyword);

		if (dataToReplace != "")
		{
			StringBuilder sb = new (fullDocument);
			StringBuilder sb_replace = new ();
				
			if (dataToReplace[0] == '\n')
			{
				sb_replace.Append ('\n');
			}

			sb_replace.Append (WriteData (new StringBuilder (), data));

			if (dataToReplace[^1] == '\n')
			{
				sb_replace.Append ('\n');
			}

			sb.Replace (dataToReplace, sb_replace.ToString ());
			SaveToFile (sb.ToString (), fileName);

			return true;
		}

		return false;
	}

	public string GetFullDocument (string fileName)
	{
		if (fileName == null)
			throw nullFileName;

		string filePath = GetFilePath (fileName);

		if (!File.Exists (filePath))
			throw new Exception ("The file doesn't exist.");
		
		using StreamReader sr = new (filePath);
		string doc = sr.ReadToEnd ();
		sr.Dispose ();

		if (doc != null)
			return doc;

		return "";
	}

	public bool FileExists (string fileName)
	{
		if (fileName == null)
			throw nullFileName;

		return File.Exists (GetFilePath (fileName));
	}

	public void OverwriteFile (string fileName, params IReadOnlyCollection<string>[] data)
	{
		if (fileName == null)
			throw nullFileName;

		if (data == null || data.Length <= 0)
			throw noDataProvided;

		StringBuilder sb = new ();

		sb = WriteData (sb, data);
		SaveToFile (sb.ToString (), fileName);
	}

	public void OverwriteFile (string fileName, string content)
	{
		if (fileName == null)
			throw nullFileName;

		if (content != null)
		{
			SaveToFile (content, fileName);
			return;
		}

		SaveToFile ("", fileName);
	}

	public void CreateFile (string fileName)
	{
		if (fileName == null)
			throw nullFileName;

		SaveToFile ("", fileName);
	}
	
	private string GetData (string doc, string keyword)
	{
		int firstIndex = doc.IndexOf (keyword);

		if (firstIndex < 0)
			return "";

		int lastIndex = doc.LastIndexOf (keyword);
		int endIndex = doc.LastIndexOf ("\n", lastIndex);
		int length;

		if (endIndex == -1)
		{
			if (firstIndex > 0)
				firstIndex--;

			length = doc.Length;
		}
		else
		{
			length = endIndex + 1;
		}
			
		return doc[firstIndex..length];
	}

	private StringBuilder WriteData (StringBuilder sb, params IReadOnlyCollection<string>[] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			if (data[i] == null || data[i].Count <= 0)
				continue;
			
			bool newLine = true;

			if (i == 0)
				newLine = sb.Length > 0;

			sb = WriteLine (sb, newLine, data[i]);
		}

		return sb;
	}

	private StringBuilder WriteLine (StringBuilder sb, bool newLine, IReadOnlyCollection<string> data) {
		if (newLine)
			sb.Append ('\n');

		for (int i = 0; i < data.Count; i++)
		{
			sb.Append (data.ElementAt (i));

			if (i >= data.Count - 1)
				continue;

			sb.Append (',');
		}

		return sb;
	}

	private string GetFilePath (string fileName)
	{
		string extension = ".csv";

		if (fileName.LastIndexOf (extension) < fileName.Length - extension.Length)
			fileName = string.Concat (fileName, extension);

		return Path.Combine (currentFolder, fileName);
	}

	private void SaveToFile (string content, string fileName)
	{
		if (!Directory.Exists (currentFolder))
			Directory.CreateDirectory (currentFolder);

		using StreamWriter writer = new (new FileStream (GetFilePath (fileName), FileMode.Create, FileAccess.Write));

		writer.Write (content);
		writer.Dispose ();
	}
}