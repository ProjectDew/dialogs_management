using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ExtensionMethods;

public class DialogsManager : MonoBehaviour
{
	[SerializeField] private TextAsset document;
	[SerializeField] private int row;
	[SerializeField] private int column;

	[SerializeField] private TMP_Text[] texts;
	[SerializeField] private string[] keywords;
	[SerializeField] private float[] delayMultipliers;
	
	[SerializeField] private string[] delayedCharacters;
	[SerializeField] private float[] delayTimes;

	[SerializeField] private float textSpeed;
	[SerializeField] private float minSpeed;
	[SerializeField] private float maxSpeed;

	private List<List<string>> stringMatrix;

	public string Language { get; set; }

	public float TextSpeed
	{
		get => textSpeed;
		set => textSpeed = value.Constrain (minSpeed, maxSpeed);
	}

	private void Awake ()
	{
		Language = Application.systemLanguage.ToString ();
		stringMatrix = CSVReader.ReadAsList (document);
	}

	public string GetDialog (string keyword)
	{
		string dialog = "";

		int languageRow = row - 1;
		int columnIndex = column - 1;
		int rowIndex = stringMatrix.CoordinatesOf (keyword, 0, columnIndex).x;

		if (rowIndex < 0)
			return dialog;
		
		List<string> availableLanguages = stringMatrix[languageRow];
		List<string> dialogsData = stringMatrix[rowIndex];

		for (int i = 0; i < availableLanguages.Count; i++)
		{
			if (availableLanguages[i].Contains (Language[1..]))
			{
				columnIndex = i;
				break;
			}
		}

		if (dialogsData.Count <= columnIndex || dialogsData[columnIndex] == null)
			return dialog;

		return dialogsData[columnIndex];
	}

	public string[] GetAllDialogs ()
	{
		if (keywords == null || keywords.Length <= 0)
			throw new Exception ("There are no keywords.");

		string[] dialogs = new string[keywords.Length];

		for (int i = 0; i < keywords.Length; i++)
		{
			dialogs[i] = GetDialog (keywords[i]);
		}

		return dialogs;
	}

	public void DisplayDialog (string keyword, string dialog = null)
	{
		if (dialog == null)
			dialog = GetDialog (keyword);

		TMP_Text text = null;
		float delayMultiplier = 0;

		for (int i = 0; i < keywords.Length; i++)
		{
			if (keyword.Contains (keywords[i]))
			{
				text = texts[i];
				delayMultiplier = delayMultipliers[i];
			}
		}

		if (text == null)
			throw new Exception ("The text associated to the keyword is null.");

		if (dialog == "")
		{
			text.text = dialog;
			return;
		}

		StartCoroutine (DisplayDialogDelayed (text, dialog, delayMultiplier));
	}

	public void DisplayAllDialogs ()
	{
		if (keywords == null || keywords.Length <= 0)
			throw new Exception ("There are no keywords.");

		string[] dialogs = GetAllDialogs ();

		for (int i = 0; i < keywords.Length; i++)
		{
			DisplayDialog (keywords[i], dialogs[i]);
		}
	}

	private IEnumerator DisplayDialogDelayed (TMP_Text text, string dialog, float delayMultiplier)
	{
		if (TextSpeed <= 0 || delayMultiplier <= 0)
		{
			text.text = dialog;
			yield break;
		}

		float timer = 0;

		text.text = "";

		for (int i = 0; i < dialog.Length; i++)
		{
			while (timer > 0)
			{
				timer -= Time.deltaTime * Mathf.Pow (TextSpeed, 2);
				yield return null;
			}
			
			text.text += dialog[i];
			float characterDelay = GetCharacterDelay (dialog[i].ToString ());
			timer = characterDelay * delayMultiplier;
		}

		yield break;
	}

	private float GetCharacterDelay (string currentCharacter)
	{
		if (delayedCharacters == null || delayedCharacters.Length <= 0)
			return 0;

		for (int i = 0; i < delayedCharacters.Length; i++)
		{
			if (currentCharacter != delayedCharacters[i])
				continue;

			return delayTimes[i];
		}

		return delayTimes[0];
	}
}
