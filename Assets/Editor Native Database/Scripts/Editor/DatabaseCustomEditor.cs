#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;


namespace ENDB
{
	/// <summary>
    ///  Custom editor for database file to prevent user from editing it.
    /// </summary>
	[CustomEditor(typeof(Database))]
	public class DatabaseCustomEditor : Editor
	{
		Database database;

		void OnEnable()
		{
			database = (Database)target;
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.LabelField("This is UNDB database file.");
			EditorGUILayout.LabelField("It should not edited in inspector window.");
			// DrawDefaultInspector();
			// GUI.color = Color.white;
			if (GUILayout.Button("Open this database"))
				OpenDatabase();

			// GUI.color = Color.white;
		}

		void OpenDatabase()
		{
			DatabaseEditorWindow.databaseWindow  = DatabaseEditorWindow.OpenDatabaseWindow();

			if (DatabaseEditorWindow.databaseWindow )
				DatabaseEditorWindow.databaseWindow.ChangeDatabase(database);
		}
	}
}
#endif