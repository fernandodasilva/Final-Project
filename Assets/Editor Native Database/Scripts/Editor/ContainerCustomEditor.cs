#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;


namespace ENDB
{
	/// <summary>
    ///  Custom editor for container file to prevent user from editing it.
    /// </summary>
	[CustomEditor(typeof(DataBaseContainer))]
	public class ContainerCustomEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			EditorGUILayout.LabelField("This is UNDB container file.");
			EditorGUILayout.LabelField("It should not be edited in inspector window.");
			// DrawDefaultInspector();
		}

	}
}
#endif