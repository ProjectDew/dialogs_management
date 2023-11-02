using UnityEditor;

public class SerializedStringArray : SerializedArray<string>
{
	public SerializedStringArray (SerializedObject parentObject, string propertyPath) : base (parentObject, propertyPath) { }

	public SerializedStringArray (SerializedProperty parentProperty, string propertyPath) : base (parentProperty, propertyPath) { }

	public override string this[int index]
	{
		get => ArrayProperty.GetArrayElementAtIndex (index).stringValue;
		set => ArrayProperty.GetArrayElementAtIndex (index).stringValue = value;
	}

	public override void Add (string elementValue)
	{
		Insert (Length, elementValue);
	}

	public override void Insert (int index, string newElement)
	{
		string nextElement;

		Length++;

		for (int i = index; i < Length; i++)
		{
			nextElement = ArrayProperty.GetArrayElementAtIndex (i).stringValue;
			ArrayProperty.GetArrayElementAtIndex (i).stringValue = newElement;
			newElement = nextElement;
		}
	}

	public override void Paste (int index)
	{
		this[index] = ArrayProperty.GetArrayElementAtIndex (copiedIndex).stringValue;
	}
}
