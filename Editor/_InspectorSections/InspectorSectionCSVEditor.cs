using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ExtensionMethods;

public sealed class InspectorSectionCSVEditor : InspectorSectionStructured, IInspectorSection
{
	public InspectorSectionCSVEditor (SerializedObject serializedObject, string folderPath) : base ()
	{
		if (serializedObject == null)
			throw new ArgumentNullException ("serializedObject", "The object provided in the constructor is null.");

		if (folderPath == null)
			throw new ArgumentNullException ("folderPath", "The path provided in the constructor is null.");

		document = serializedObject.FindProperty ("document");
		row = serializedObject.FindProperty ("row");
		column = serializedObject.FindProperty ("column");
		
		docWriter = new (folderPath);
	}

	private readonly SerializedProperty document;
	private readonly SerializedProperty row;
	private readonly SerializedProperty column;
	
	private readonly CSVWriter docWriter;

	private TextAsset file;

	private List<List<string>> stringMatrix;
	
	private int currentRow, currentColumnLeft, currentColumnRight;

	private readonly GUILayoutOption navigationButton = GUILayout.Width (20);
	private readonly GUILayoutOption areaHeigth = GUILayout.ExpandHeight (true);
	private GUILayoutOption areaWidth;

	public override void Insert ()
	{
		if (file != null)
		{
			DrawHeader ();
			DrawBody ();
			DrawFooter ();

			CustomLayout.DrawSubsectionLine ();

			if (GUILayout.Button ("Unload document"))
				file = null;
		}
		else
		{
			if (GUILayout.Button ("Load document"))
			{
				file = (TextAsset)document.objectReferenceValue;

				stringMatrix = CSVReader.ReadAsList (file);

				currentRow = row.intValue.Constrain (0, stringMatrix.Count - 1);
				currentColumnLeft = (column.intValue - 1).Constrain (0, stringMatrix.ColumnCount () - 1);
				currentColumnRight = column.intValue.Constrain (0, stringMatrix.ColumnCount () - 1);
			}
		}
	}

	protected override void DrawHeader ()
	{
		int intValue;

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Rows: ");

		if (GUILayout.Button ("<", navigationButton))
		{
			if (currentRow > 0)
				currentRow--;
		}

		intValue = (currentRow + 1).Constrain (1, stringMatrix.Count);
		intValue = EditorGUILayout.IntField (intValue, navigationButton);

		GUILayout.Label ("/" + stringMatrix.Count.ToString ());
			
		currentRow = intValue - 1;

		if (GUILayout.Button (">", navigationButton))
		{
			if (currentRow < stringMatrix.Count - 1)
				currentRow++;
		}

		GUILayout.FlexibleSpace ();

		if (GUILayout.Button ("Remove row"))
		{
			stringMatrix.RemoveAt (currentRow);
			docWriter.OverwriteFile (file.name, stringMatrix.ToArray ());

			currentRow = currentRow.Constrain (0, stringMatrix.Count - 1);
		}

		GUILayout.FlexibleSpace ();

		if (GUILayout.Button ("Add row"))
		{
			List<string> newRow = new ();

			for (int i = 0; i < stringMatrix.ColumnCount (); i++)
			{
				newRow.Add ("");
			}

			if (newRow.Count == 0)
				newRow.Add ("");

			docWriter.SaveData (file.name, newRow.ToArray ());
			stringMatrix.Add (newRow);

			if (stringMatrix.Count > 0)
				currentRow = stringMatrix.Count - 1;
		}

		GUILayout.EndHorizontal ();
	}

	protected override void DrawBody ()
	{
		CustomLayout.DrawSubsectionLine (15, 10);

		if (stringMatrix.Count == 0 || stringMatrix.ColumnCount () == 0)
		{
			GUILayout.Label ("This document has no rows nor columns", Styles.Info);
			return;
		}

		areaWidth = GUILayout.Width (Screen.width * 0.48f - 65);

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("", GUILayout.Width (19));

		currentColumnLeft = DrawColumnHeader (currentColumnLeft);

		GUILayout.FlexibleSpace ();

		currentColumnRight = DrawColumnHeader (currentColumnRight);

		GUILayout.EndHorizontal ();
		GUILayout.Space (5);
		GUILayout.BeginHorizontal ();

		currentColumnLeft = DrawColumnBody (currentColumnLeft);

		GUILayout.FlexibleSpace ();

		currentColumnRight = DrawColumnBody (currentColumnRight);

		GUILayout.EndHorizontal ();
		GUILayout.Space (5);
		GUILayout.BeginHorizontal ();

		GUILayout.Label ("", GUILayout.Width (19));

		currentColumnLeft = DrawColumnFooter (currentColumnLeft);

		GUILayout.FlexibleSpace ();

		currentColumnRight = DrawColumnFooter (currentColumnRight);
		
		GUILayout.Label ("", GUILayout.Width (19));
		GUILayout.EndHorizontal ();
	}

	protected override void DrawFooter ()
	{
		CustomLayout.DrawSubsectionLine (10, 15);

		GUILayout.BeginHorizontal ();

		GUILayout.Label ("Columns: " + stringMatrix.ColumnCount ().ToString ());
		GUILayout.FlexibleSpace ();

		if (GUILayout.Button ("Add column"))
		{
			List<string> newColumn = new ();

			for (int i = 0; i < stringMatrix.Count; i++)
			{
				newColumn.Add ("");
			}

			if (newColumn.Count == 0)
				newColumn.Add ("");

			stringMatrix.AddColumn (newColumn);
			docWriter.OverwriteFile (file.name, stringMatrix.ToArray ());

			if (stringMatrix.ColumnCount () > 0)
				currentColumnRight = stringMatrix.ColumnCount () - 1;
		}

		GUILayout.EndHorizontal ();

		CustomLayout.DrawSubsectionLine ();

		if (GUILayout.Button ("SAVE CHANGES"))
		{
			docWriter.OverwriteFile (file.name, stringMatrix.ToArray ());
		}
	}

	private int DrawColumnHeader (int column)
	{
		const int headerRow = 0;

		int intValue = (column + 1).Constrain (1, stringMatrix.ColumnCount ());
		string stringValue = stringMatrix.GetEntry (headerRow, column);

		if (stringValue.IsNullOrEmpty ())
			stringValue = "Column " + intValue.ToString ();

		GUILayout.Label (stringValue, Styles.Subheader, areaWidth);
		intValue = EditorGUILayout.IntField (intValue, navigationButton);

		return intValue - 1;
	}

	private int DrawColumnBody (int column)
	{
		string stringValue;

		char currentChar = Event.current.character;

		if (GUILayout.Button ("<", navigationButton))
		{
			if (column > 0)
				column--;
		}
		
		stringValue = stringMatrix.GetEntry (currentRow, column);
		stringValue = EditorGUILayout.TextArea (stringValue.NonNullable (), areaWidth, areaHeigth);

		stringMatrix.SetEntry (currentRow, column, stringValue);

		if (currentChar == '\t')
		{
			EditorGUI.FocusTextInControl (null);
		}

		if (GUILayout.Button (">", navigationButton))
		{
			if (column < stringMatrix.ColumnCount () - 1)
				column++;
		}

		return column;
	}

	private int DrawColumnFooter (int column)
	{
		if (GUILayout.Button ("Remove column", areaWidth))
		{
			stringMatrix.RemoveColumn (column);
			docWriter.OverwriteFile (file.name, stringMatrix.ToArray ());

			currentColumnLeft = currentColumnLeft.Constrain (0, stringMatrix.ColumnCount () - 1);
			currentColumnRight = currentColumnRight.Constrain (0, stringMatrix.ColumnCount () - 1);
		}

		return column.Constrain (0, stringMatrix.ColumnCount () - 1);
	}
}