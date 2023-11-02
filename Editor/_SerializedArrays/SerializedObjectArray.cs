using UnityEngine;
using UnityEditor;

public class SerializedObjectArray : SerializedArray<Object>
{
	public SerializedObjectArray (SerializedObject parentObject, string propertyPath) : base (parentObject, propertyPath) { }

	public SerializedObjectArray (SerializedProperty parentProperty, string propertyPath) : base (parentProperty, propertyPath) { }

	public override Object this[int index]
	{
		get => ArrayProperty.GetArrayElementAtIndex (index).objectReferenceValue;
		set => ArrayProperty.GetArrayElementAtIndex (index).objectReferenceValue = value;
	}

	public override void Add (Object elementValue)
	{
		Insert (Length, elementValue);
	}

	public override void Insert (int index, Object newElement)
	{
		Object nextElement;

		Length++;

		for (int i = index; i < Length; i++)
		{
			nextElement = ArrayProperty.GetArrayElementAtIndex (i).objectReferenceValue;
			ArrayProperty.GetArrayElementAtIndex (i).objectReferenceValue = newElement;
			newElement = nextElement;
		}
	}

	public override void Paste (int index)
	{
		this[index] = ArrayProperty.GetArrayElementAtIndex (copiedIndex).objectReferenceValue;
	}
}
