using System;
using System.Collections.Generic;
using ExtensionMethods;

public class DialogVariablesManager
{
	public DialogVariablesManager (string openingCharacters, string closingCharacters)
	{
		if (openingCharacters.IsNullOrEmpty () || closingCharacters.IsNullOrEmpty ())
			throw new ArgumentException ("The opening or closing characters provided in the constructor are null or empty.");

		if (openingCharacters == closingCharacters)
			throw new ArgumentException ("The closing characters of the variable must be different from the opening ones.");

		storedVariables = new Dictionary<string, string> ();

		this.openingCharacters = openingCharacters;
		this.closingCharacters = closingCharacters;
	}

	private readonly Exception nullOrEmptyVariable = new ArgumentNullException ("variable (string)", "The provided variable is null or empty.");

	private readonly IDictionary<string, string> storedVariables;

	private readonly string openingCharacters;
	private readonly string closingCharacters;

	public string InsertVariablesInDialog (string dialog)
	{
		int currentIndex = 0;
		int start, end, length;

		string variable;

		while (currentIndex >= 0)
		{
			start = currentIndex;
			end = dialog.IndexOf (closingCharacters, start) + closingCharacters.Length;
			length = end - start;

			if (length > 0)
			{
				variable = dialog.Substring (start, length);

				if (storedVariables.ContainsKey (variable))
					dialog = dialog.Replace (variable, storedVariables[variable]);
			}
			
			start += openingCharacters.Length;

			currentIndex = dialog.IndexOf (openingCharacters, start);
		}

		return dialog;
	}

	public string GetVariableValue (string variable)
	{
		if (variable.IsNullOrEmpty ())
			throw nullOrEmptyVariable;

		string placeholder = string.Concat (openingCharacters, variable, closingCharacters);

		if (!storedVariables.TryGetValue (placeholder, out string value))
			throw new Exception ("The variable doesn't exist.");
		
		return value;
	}

	public void AddVariable (string variable, string value)
	{
		if (variable.IsNullOrEmpty ())
			throw nullOrEmptyVariable;

		string placeholder = string.Concat (openingCharacters, variable, closingCharacters);

		if (!storedVariables.ContainsKey (placeholder))
			storedVariables.Add (placeholder, value);
	}

	public void UpdateVariable (string variable, string value)
	{
		if (variable.IsNullOrEmpty ())
			throw nullOrEmptyVariable;

		string placeholder = string.Concat (openingCharacters, variable, closingCharacters);

		if (storedVariables.ContainsKey (placeholder))
			storedVariables.Remove (placeholder);
		
		storedVariables.Add (placeholder, value);
	}

	public void RemoveVariable (string variable)
	{
		if (variable.IsNullOrEmpty ())
			throw nullOrEmptyVariable;

		string placeholder = string.Concat (openingCharacters, variable, closingCharacters);

		if (storedVariables.ContainsKey (placeholder))
			storedVariables.Remove (placeholder);
	}
}
