using UnityEngine;

public class PresetStyles
{
	private GUIStyle headerColapsed, headerExpanded, subheader, info;

	public GUIStyle HeaderColapsed
	{
		get
		{
			if (headerColapsed == null)
				SetHeaderColapsedStyle ();

			return headerColapsed;
		}
	}

	public GUIStyle HeaderExpanded
	{
		get
		{
			if (headerExpanded == null)
				SetHeaderExpandedStyle ();

			return headerExpanded;
		}
	}

	public GUIStyle Subheader
	{
		get
		{
			if (subheader == null)
				SetSubheaderStyle ();

			return subheader;
		}
	}

	public GUIStyle Info
	{
		get
		{
			if (info == null)
				SetInfoStyle ();

			return info;
		}
	}

	public GUIStyle SwitchHeaderStyle (bool expanded)
	{
		if (expanded)
			return HeaderExpanded;

		return HeaderColapsed;
	}

	private void SetSubheaderStyle ()
	{
		GUIStyle guiStyle = new ();

		guiStyle.normal.textColor = new Color (0.8f, 0.8f, 0.8f);
		guiStyle.fontStyle = FontStyle.Bold;
		guiStyle.alignment = TextAnchor.MiddleCenter;

		subheader = guiStyle;
	}

	private void SetInfoStyle ()
	{
		GUIStyle guiStyle = new ();
			
		guiStyle.normal.textColor = new Color (0.6f, 0.6f, 0.6f);
		guiStyle.alignment = TextAnchor.MiddleCenter;

		info = guiStyle;
	}

	private void SetHeaderExpandedStyle ()
	{
		GUIStyle guiStyle = new ();

		guiStyle.normal.textColor = new Color (0.6f, 0.6f, 0.6f);
		SetHeaderStyle (guiStyle);

		headerExpanded = guiStyle;
	}

	private void SetHeaderColapsedStyle ()
	{
		GUIStyle guiStyle = new ();

		guiStyle.normal.textColor = new Color (1, 1, 1);
		SetHeaderStyle (guiStyle);

		headerColapsed = guiStyle;
	}

	private void SetHeaderStyle (GUIStyle guiStyle)
	{
		guiStyle.hover.textColor = guiStyle.normal.textColor;
		guiStyle.alignment = TextAnchor.LowerLeft;
		guiStyle.fontStyle = FontStyle.Bold;
		guiStyle.fontSize = 13;
	}
}