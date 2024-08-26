#if UNITY_EDITOR

using UnityEngine;

namespace ENDB
{

    /// <summary>
    /// Detects when a new script is created. 
    /// </summary>
    public class NewScriptDetector :  UnityEditor.AssetPostprocessor
    {
        public static void OnGeneratedCSProjectFiles()
        { // doas not fire when you change inheritance
            DatabaseUtility.StoreEntryTypes();
            // Debug.Log("OnGeneratedCSProjectFiles");
        }
    }

}
#endif
