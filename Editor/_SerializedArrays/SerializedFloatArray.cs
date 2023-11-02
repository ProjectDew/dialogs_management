using UnityEditor;

public class SerializedFloatArray : SerializedArray<float>
{
	public SerializedFloatArray (SerializedObject parentObject, string propertyPath) : base (parentObject, propertyPath) { }

	public SerializedFloatArray (SerializedProperty parentProperty, string propertyPath) : base (parentProperty, propertyPath) { }

	public override float this[int index]
	{
		get => ArrayProperty.GetArrayElementAtIndex (index).floatValue;
		set => ArrayProperty.GetArrayElementAtIndex (index).floatValue = value;
	}

	public override void Add (float elementValue)
	{
		Insert (Length, elementValue);
	}

	public override void Insert (int index, float newElement)
	{
		float nextElement;

		Length++;

		for (int i = index; i < Length; i++)
		{
			nextElement = ArrayProperty.GetArrayElementAtIndex (i).floatValue;
			ArrayProperty.GetArrayElementAtIndex (i).floatValue = newElement;
			newElement = nextElement;
		}
	}

	public override void Paste (int index)
	{
		this[index] = ArrayProperty.GetArrayElementAtIndex (copiedIndex).floatValue;
	}
}
