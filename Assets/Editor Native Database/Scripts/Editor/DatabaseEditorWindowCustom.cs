#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
// using System.Reflection;

namespace ENDB
{ 
    // Use this file to write any custom code. This file will not change in future updates.
	public partial class DatabaseEditorWindow : EditorWindow
	{
		/// <summary>
        /// Use this method to display anything before entry's content.
        /// </summary>
		partial void CustomStuffBeforeEntry()
		{
			DrawPreviewIcon();
            // skipDefaultInspector = true; // uncomment this line to not draw default inspector
		}

		/// <summary>
        /// Use this method to display anything after entry's content.
        /// </summary>
		partial void CustomStuffAfterEntry()
		{
			if (selectedEntry?.GetType() == typeof(GameEvent )) // delete this line if you wanna delete example folder
				EventCustomInspector(); // delete this line if you wanna delete example folder
		}



    }
}
#endif