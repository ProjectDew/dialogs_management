using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using ExtensionMethods;

public sealed class DialogsManagerTexts : InspectorSectionStructured, IInspectorSection
{
	public DialogsManagerTexts (SerializedObject serializedObject, IInspectorSection nextSection = null) : base (nextSection)
	{
		if (serializedObject == null)
			throw new ArgumentNullException ("serializedObject", "The object provided in the constructor is null.");
		
		document = serializedObject.FindProperty ("document");
		column = serializedObject.FindProperty ("column");
		
		texts = new (serializedObject, "texts");
		keywords = new (serializedObject, "keywords");
		delayMultipliers = new (serializedObject, "delayMultipliers");

		file = (TextAsset)document.objectReferenceValue;
	}

	private readonly SerializedProperty document;
	private readonly SerializedProperty column;

	private readonly SerializedObjectArray texts;
	private readonly SerializedStringArray keywords;
	private readonly SerializedFloatArray delayMultipliers;

	private readonly TextAsset file;

	protected override void DrawHeader ()
	{
		if (GUILayout.Button ("Display dialogs", Styles.SwitchHeaderStyle (texts.BaseProperty.isExpanded)))
			texts.BaseProperty.isExpanded = !texts.BaseProperty.isExpanded;
	}

	protected override void DrawBody ()
	{
		if (!texts.BaseProperty.isExpanded)
			return;

		CustomLayout.DrawSubsectionLine ();

		if (GUILayout.Button ("Add new keywords from document"))
		{
			AddKeywordsFromDocument ();
		}

		CustomLayout.DrawSubsectionLine (15, 5);

		GUILayout.BeginHorizontal ();
		
		GUILayout.Label ("", GUILayout.Width (20));
		GUILayout.Label ("\nTMP_Text");
		GUILayout.Label ("\nKeyword");

		InsertDelayLabel ();

		GUILayout.EndHorizontal ();
		GUILayout.Space (5);

		DrawTextAndKeywordConnections ();

		GUILayout.Space (5);

		if (GUILayout.Button ("Add new"))
		{
			texts.Add (null);
			keywords.Add ("");
			delayMultipliers.Add (1.0f);
		}
	}

	protected override void DrawFooter ()
	{
		CustomLayout.DrawSectionLine ();
		
		if (delayMultipliers.BaseProperty.isExpanded)
			return;

		if (GUILayout.Button ("SHOW CHARACTER CONFIGURATION"))
			delayMultipliers.BaseProperty.isExpanded = !delayMultipliers.BaseProperty.isExpanded;
		
		CustomLayout.DrawSectionLine ();
	}

	private void AddKeywordsFromDocument ()
	{
		List<List<string>> stringMatrix = CSVReader.ReadAsList (file);
		List<string> existingKeywords = stringMatrix.GetColumn (column.intValue - 1);

		int offset = 1;

		for (int i = offset; i < existingKeywords.Count; i++)
		{
			for (int j = 0; j < texts.Length; j++)
			{
				if (existingKeywords[i] == keywords[j])
					existingKeywords[i] = null;
			}

			if (existingKeywords[i] == null)
				continue;

			texts.Add (null);
			keywords.Add (existingKeywords[i]);
			delayMultipliers.Add (1.0f);
		}
	}

	private void DrawTextAndKeywordConnections ()
	{
		for (int i = 0; i < texts.Length; i++)
		{
			GUILayout.BeginHorizontal ();

			if (GUILayout.Button ("X", GUILayout.Width (20)))
			{
				texts.Delete (i);
				keywords.Delete (i);
				delayMultipliers.Delete (i);
			}

			if (i >= texts.Length)
				break;

			texts[i] = EditorGUILayout.ObjectField (texts[i], typeof (TMP_Text), true) as TMP_Text;
			keywords[i] = EditorGUILayout.TextField (keywords[i].NonNullable ());

			InsertDelayField (i);

			GUILayout.EndHorizontal ();
		}
	}

	private void InsertDelayLabel ()
	{
		if (!delayMultipliers.BaseProperty.isExpanded)
			return;
		
		GUILayout.Label ("Delay\nmultiplier", GUILayout.MaxWidth (65));
	}

	private void InsertDelayField (int i)
	{
		if (!delayMultipliers.BaseProperty.isExpanded)
			return;
		
		float floatValue = delayMultipliers[i].Constrain (0.0f);
		delayMultipliers[i] = EditorGUILayout.FloatField (floatValue, GUILayout.MaxWidth (65));
	}
}
