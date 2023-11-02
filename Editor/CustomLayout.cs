using UnityEngine;
using UnityEditor;

public static class CustomLayout
{
	private const int minColumns = 1;

	private static readonly Color sectionColor = Color.black;
	private static readonly Color subsectionColor = new (0.4f, 0.4f, 0.4f, 1);

	public static void DrawLine (float height, float paddingTop, float paddingBottom, Color color)
	{
		GUILayout.Space (paddingTop);
		EditorGUI.DrawRect (EditorGUILayout.GetControlRect (GUILayout.Height (height)), color);
		GUILayout.Space (paddingBottom);
	}

	public static void DrawLine (float height, float padding, Color color)
	{
		DrawLine (height, padding, padding, color);
	}

	public static void DrawSectionLine (float paddingTop, float paddingBottom)
	{
		DrawLine (1, paddingTop, paddingBottom, sectionColor);
	}

	public static void DrawSectionLine (float padding = 15)
	{
		DrawLine (1, padding, sectionColor);
	}

	public static void DrawSubsectionLine (float paddingTop, float paddingBottom)
	{
		DrawLine (1, paddingTop, paddingBottom, subsectionColor);
	}

	public static void DrawSubsectionLine (float padding = 15)
	{
		DrawLine (1, padding, subsectionColor);
	}

	public static void BeginArrayTable (int totalColumns, int arrayIndex)
	{
		if (totalColumns < minColumns)
			return;

		if (arrayIndex % totalColumns == 0)
		{
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
		}
	}

	public static void EndArrayTable (int totalColumns, int arrayIndex, int arrayLength)
	{
		if (totalColumns < minColumns)
			return;

		if (arrayIndex % totalColumns == totalColumns - 1 || arrayIndex == arrayLength - 1)
		{
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();

			DrawSubsectionLine (3);
		}
	}
}
