using UnityEngine;
using ENDB;
/// This is an example. You can delete this file if you are not using it.

namespace ENDB
{
    /// <summary>
    /// This is an example class. 
    /// </summary>
    public class GenericItemExample : DatabaseEntry
    {
        [TextArea] public string description;
        [DatabaseAssetPreview(32, 32)] public Sprite icon;

    }
}