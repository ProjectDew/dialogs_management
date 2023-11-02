using System;
using UnityEditor;

public abstract class SerializedArray<T>
{
	public SerializedArray (SerializedObject parentObject, string propertyPath)
	{
		if (parentObject == null)
			throw new ArgumentNullException ("parentObject (SerializedObject)", "The object provided in the constructor is null.");

		if (propertyPath == null || propertyPath == "")
			throw new ArgumentException ("The path provided in the constructor is not valid.", "propertyPath");

		baseProperty = parentObject.FindProperty (propertyPath);

		SetArrayProperties ();
	}

	public SerializedArray (SerializedProperty parentProperty, string propertyPath)
	{
		if (parentProperty == null)
			throw new ArgumentNullException ("The parent property provided in the constructor is null.");

		if (propertyPath == null || propertyPath == "")
			throw new ArgumentException ("The path provided in the constructor is not valid.", "propertyPath");

		baseProperty = parentProperty.FindPropertyRelative (propertyPath);

		SetArrayProperties ();
	}
	
	private readonly SerializedProperty baseProperty;

	private SerializedProperty arrayProperty;
	private SerializedProperty sizeProperty;

	protected int copiedIndex;

	public SerializedProperty BaseProperty => baseProperty.Copy ();

	protected SerializedProperty ArrayProperty => arrayProperty.Copy ();

	public abstract T this[int index] { get; set; }

	public int Length
	{
		get => sizeProperty.intValue;
		protected set => sizeProperty.intValue = value;
	}

	public abstract void Add (T element);

	public abstract void Insert (int index, T newElement);

	public virtual void Delete (int index)
	{
		arrayProperty.DeleteArrayElementAtIndex (index);
	}

	public virtual void Move (int indexFrom, int indexTo)
	{
		arrayProperty.MoveArrayElement (indexFrom, indexTo);
	}

	public virtual void Copy (int index)
	{
		copiedIndex = index;
	}

	public abstract void Paste (int index);

	private void SetArrayProperties ()
	{
		SerializedProperty copiedProperty = BaseProperty;
		
		copiedProperty.Next (true);
		arrayProperty = copiedProperty.Copy ();

		copiedProperty.Next (true);
		sizeProperty = copiedProperty.Copy ();
	}
}
