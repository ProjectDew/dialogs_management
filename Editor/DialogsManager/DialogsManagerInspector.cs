using System.IO;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (DialogsManager))]
public class DialogsManagerInspector : Editor {

	private const int totalColumnsInCharTable = 3;

	private readonly string folderPath = Path.Combine (Application.dataPath, "Resources");
	
	private DialogsManager dialogsManager;

	private IInspectorSection baseInspector, fullInspector;
	
	private SerializedFloatArray delayMultipliers;

	public void OnEnable ()
	{
		dialogsManager = (DialogsManager)target;

		if (dialogsManager == null)
			return;
		
		delayMultipliers = new (serializedObject, "delayMultipliers");

		baseInspector =
			new DialogsManagerDocument (serializedObject, folderPath,
				new InspectorSectionCSVEditor (serializedObject, folderPath),
			new DialogsManagerTexts (serializedObject));

		fullInspector =
			new DialogsManagerDocument (serializedObject, folderPath,
				new InspectorSectionCSVEditor (serializedObject, folderPath),
			new DialogsManagerTexts (serializedObject,
			new DialogsManagerCharacters (serializedObject, totalColumnsInCharTable)));
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();

		Undo.RecordObject (dialogsManager, "Undo Dialogs manager");
		
		CustomLayout.DrawSectionLine ();

		if (delayMultipliers.BaseProperty.isExpanded)
			fullInspector.Insert ();
		else
			baseInspector.Insert ();

		serializedObject.ApplyModifiedProperties ();
	}
}
