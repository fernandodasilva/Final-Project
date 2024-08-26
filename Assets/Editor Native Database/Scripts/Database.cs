using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ENDB
{

    [CreateAssetMenu(menuName = "Scriptable Object/Database/New Database")]
    public class Database : ScriptableObject
    {   
        /// <summary>
        ///  Stores container and entry names Every name is unique.
        /// </summary>
        public List<string> usedNames = new List<string>();
        public Color NormalContainerColor = Color.white;
        public Color SelectedContainerColor = Color.green;
        public Color NormalContainerFontColor = Color.black;
        public Color SelectedContainerFontColor = Color.black;
        public Color NormalEntryColor = Color.white;
        public Color SelectedEntryColor = Color.green;
        public Color NormalEntryFontColor = Color.black;
        public Color SelectedEntryFontColor = Color.black;
        public int containerFontSize = 14;
        public int entryFontSize = 14;
        public int leftWindowWidth = 555;
        public int rightWindowWidth = 333;
        public int containerButtonWidth = 111;
        /// <summary>
        ///  Space between container buttons.
        /// </summary>
        public int containerSpacing  = 8; 
        public FontStyle containerNormalFontStyle = FontStyle.Normal;
        public FontStyle containerSelectedFontStyle = FontStyle.Normal;
        public FontStyle entryNormalFontStyle = FontStyle.Normal;
        public FontStyle entrySelectedFontStyle = FontStyle.Normal;
        
        /// <summary>
        ///  When true container's parents are selected with the container. It's only a visual aid and does not affect how database works.
        /// </summary>
        public bool selectContainerWithParents = false;
        public bool hideSearchField;

        /// <summary>
        ///  When true containers are displayed not as tree but as single column. Containers not in database root will be hidden.
        /// </summary>
        public bool showContainersAsColumn;

        /// <summary>
        ///  When true container name always fits container button.
        /// </summary>
        public bool containerNameAlwaysFitsButton;

        /// <summary>
        ///  Stores containers.
        /// </summary>
        public List<RowHolder> storage = new List<RowHolder>();

    }

    /// <summary>
    ///  Stores row of containers.
    /// </summary>
    [System.Serializable]
    public class RowHolder
    {
        public List<DataBaseContainer> containers = new List<DataBaseContainer>();

        public RowHolder(List<DataBaseContainer> containers)
        {
            this.containers = containers;
        }
    }

}