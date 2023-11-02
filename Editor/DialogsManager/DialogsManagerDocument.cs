using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using ExtensionMethods;

public sealed class DialogsManagerDocument : InspectorSectionStructured, IInspectorSection
{
	public DialogsManagerDocument (SerializedObject serializedObject, string folderPath, IInspectorSection fileEditor, IInspectorSection nextSection = null) : base (nextSection)
	{
		if (serializedObject == null)
			throw new ArgumentNullException ("serializedObject", "The object provided in the constructor is null.");

		if (folderPath == null)
			throw new ArgumentNullException ("folderPath", "The path provided in the constructor is null.");

		this.fileEditor = fileEditor ?? throw new ArgumentNullException ("fileEditor", "The interface provided in the constructor is null.");
		
		document = serializedObject.FindProperty ("document");
		row = serializedObject.FindProperty ("row");
		column = serializedObject.FindProperty ("column");

		relativePath = Path.GetRelativePath (Directory.GetCurrentDirectory (), Path.Combine (folderPath, defaultFileName));
		fileWriter = new (folderPath);
	}

	private readonly string defaultFileName = "Data_dialogs.csv";
	
	private readonly SerializedProperty document;
	private readonly SerializedProperty row;
	private readonly SerializedProperty column;
	
	private readonly IInspectorSection fileEditor;
	
	private readonly CSVWriter fileWriter;
	private readonly string relativePath;

	protected override void DrawHeader ()
	{
		if (GUILayout.Button ("Read dialogs", Styles.SwitchHeaderStyle (document.isExpanded)))
			document.isExpanded = !document.isExpanded;
	}

	protected override void DrawBody ()
	{
		if (!document.isExpanded)
			return;

		CustomLayout.DrawSubsectionLine (15, 10);

		GUILayout.BeginHorizontal ();

		GUILayout.Label ("CSV document");
		document.objectReferenceValue = EditorGUILayout.ObjectField (document.objectReferenceValue, typeof (TextAsset), true, GUILayout.Width (Screen.width * 0.5f)) as TextAsset;
			
		if (document.objectReferenceValue == null)
		{
			if (GUILayout.Button ("Create"))
				CreateFile ();
		}

		GUILayout.EndHorizontal ();

		row.intValue = DrawDocumentData (row.intValue, "Row for language names");
		column.intValue = DrawDocumentData (column.intValue, "Column for keywords");

		CustomLayout.DrawSubsectionLine (10);

		GUILayout.Label ("Edit document", Styles.Subheader);

		GUILayout.Space (15);

		fileEditor.Insert ();
	}

	private void CreateFile ()
	{
		TextAsset textAsset = AssetDatabase.LoadAssetAtPath (relativePath, typeof(TextAsset)) as TextAsset;

		if (textAsset == null)
		{
			fileWriter.CreateFile (defaultFileName);
			AssetDatabase.Refresh ();

			document.objectReferenceValue = AssetDatabase.LoadAssetAtPath (relativePath, typeof(TextAsset)) as TextAsset;
		}
		else
		{
			document.objectReferenceValue = textAsset;
		}
	}

	private int DrawDocumentData (int value, string labelText)
	{
		GUILayout.Space (5);
		GUILayout.BeginHorizontal ();

		GUILayout.Label (labelText);

		EditorGUILayout.IntField (value.Constrain (1), GUILayout.Width (Screen.width * 0.5f));

		GUILayout.EndHorizontal ();

		return value;
	}
}
