using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ENDB;

namespace ENDB
{
    public class BookItem : DatabaseEntry
    {
        [TextArea] public string description;
        [DatabaseAssetPreview(32, 32)] public Sprite icon;

    }
}