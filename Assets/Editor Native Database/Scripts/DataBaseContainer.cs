using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ENDB
{
    public class DataBaseContainer :  ScriptableObject
    {
		/// <summary>
        /// row number in database tree.
        /// </summary>
        public int row = 0;
        /// <summary>
        /// position  number in row.
        /// </summary>
        public int posInRow = 0;
        /// <summary>
        /// container button size. Min is 1. +1 for every child after 1st
        /// </summary>
        public int buttonSize = 1;
        
        public List<DataBaseContainer> children = new List<DataBaseContainer>();
        public List<DataBaseContainer> parents = new List<DataBaseContainer>();
        public DataBaseContainer parent;
        public List<DatabaseEntry> entries = new List<DatabaseEntry>();

    }

}
