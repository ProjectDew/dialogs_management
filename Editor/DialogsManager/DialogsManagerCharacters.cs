using System;
using UnityEngine;
using UnityEditor;
using ExtensionMethods;

public sealed class DialogsManagerCharacters : InspectorSectionStructured, IInspectorSection
{
	public DialogsManagerCharacters (SerializedObject serializedObject, int totalColumnsInCharTable, IInspectorSection nextSection = null) : base (nextSection)
	{
		if (serializedObject == null)
			throw new ArgumentNullException ("serializedObject", "The object provided in the constructor is null.");
		
		delayMultipliers = new (serializedObject, "delayMultipliers");
		delayedCharacters = new (serializedObject, "delayedCharacters");
		delayTimes = new (serializedObject, "delayTimes");
	
		textSpeed = serializedObject.FindProperty ("textSpeed");
		minSpeed = serializedObject.FindProperty ("minSpeed");
		maxSpeed = serializedObject.FindProperty ("maxSpeed");

		minValue = minSpeed.floatValue;
		maxValue = maxSpeed.floatValue;

		totalColumns = totalColumnsInCharTable;
	}
	
	private readonly SerializedFloatArray delayMultipliers;
	private readonly SerializedStringArray delayedCharacters;
	private readonly SerializedFloatArray delayTimes;
	
	private readonly SerializedProperty textSpeed;
	private readonly SerializedProperty minSpeed;
	private readonly SerializedProperty maxSpeed;
	
	private float minValue, maxValue;

	private readonly int totalColumns;

	protected override void DrawHeader ()
	{
		GUILayout.BeginHorizontal ();

		if (GUILayout.Button ("Delay characters", Styles.SwitchHeaderStyle (delayedCharacters.BaseProperty.isExpanded)))
			delayedCharacters.BaseProperty.isExpanded = !delayedCharacters.BaseProperty.isExpanded;
			
		if (GUILayout.Button ("HIDE CHARACTER CONFIGURATION"))
			delayMultipliers.BaseProperty.isExpanded = !delayMultipliers.BaseProperty.isExpanded;

		GUILayout.EndHorizontal ();
	}

	protected override void DrawBody ()
	{
		if (!delayedCharacters.BaseProperty.isExpanded)
			return;

		CustomLayout.DrawSubsectionLine ();

		InsertDefaultDelayTime ();

		int offset = 1;

		for (int i = offset; i < delayTimes.Length; i++)
		{
			CustomLayout.BeginArrayTable (totalColumns, i - offset);
			
			InsertSpecificDelayTimes (i);

			CustomLayout.EndArrayTable (totalColumns, i - offset, delayTimes.Length - offset);
		}

		InsertRuntimeMultiplier ();
	}

	private void InsertDefaultDelayTime ()
	{
		if (delayedCharacters.Length < 1)
		{
			delayedCharacters.Add ("");
			delayTimes.Add (0.0f);
		}
		
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();

		GUILayout.Label ("Default delay");
		delayTimes[0] = EditorGUILayout.FloatField (delayTimes[0].Constrain (0.0f));
		
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();

		CustomLayout.DrawSubsectionLine (3);
		
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();

		if (GUILayout.Button ("Add exceptions"))
		{
			delayedCharacters.Add ("Ñ");
			delayTimes.Add (0.0f);
		}

		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();

		CustomLayout.DrawSubsectionLine (3);
	}

	private void InsertSpecificDelayTimes (int i)
	{
		string stringValue;

		if (GUILayout.Button ("X", GUILayout.Width (20)))
		{
			if (delayedCharacters.Length > 1)
			{
				delayedCharacters.Delete (i);
				delayTimes.Delete (i);
			}
		}

		if (i >= delayTimes.Length)
			return;
		
		stringValue = delayedCharacters[i];

		if (stringValue.Length == 0)
			stringValue = "Ñ";
		
		delayedCharacters[i] = EditorGUILayout.TextField (stringValue[0].ToString (), GUILayout.Width (20));

		if (stringValue[0] == '_')
			delayedCharacters[i] = "\n";

		delayTimes[i] = EditorGUILayout.FloatField (delayTimes[i].Constrain (0.0f));
	}

	private void InsertRuntimeMultiplier ()
	{
		GUILayout.Space (8);
		GUILayout.Label ("Runtime speed multiplier", Styles.Subheader);

		GUILayout.Space (5);
		GUILayout.BeginHorizontal ();

		minSpeed.floatValue = ConstrainValue (minSpeed.floatValue, minValue, true);
		
		GUILayout.Label ("Min");
		GUILayout.FlexibleSpace ();
		GUILayout.Label ("Max");

		maxSpeed.floatValue = ConstrainValue (maxSpeed.floatValue, maxValue, false);

		GUILayout.EndHorizontal ();
		GUILayout.Space (5);

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Current");

		if (Event.current.character != '\n')
			textSpeed.floatValue = EditorGUILayout.Slider (textSpeed.floatValue, minValue, maxValue);
		else
			textSpeed.floatValue = EditorGUILayout.Slider (textSpeed.floatValue, minSpeed.floatValue, maxSpeed.floatValue);

		GUILayout.EndHorizontal ();
	}

	private float ConstrainValue (float value, float comparedValue, bool constrainMinimum)
	{
		float valueWidth = 50;

		if (Event.current.character != '\n' || value == comparedValue)
			return EditorGUILayout.FloatField (value, GUILayout.Width (valueWidth));

		if (constrainMinimum)
		{
			value = EditorGUILayout.FloatField (value.Constrain (0.0f, maxValue), GUILayout.Width (valueWidth));
			minValue = value;
		}
		else
		{
			value = EditorGUILayout.FloatField (value.Constrain (minValue), GUILayout.Width (valueWidth));
			maxValue = value;
		}

		return value;
	}
}
