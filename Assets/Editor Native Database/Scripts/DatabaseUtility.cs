


using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ENDB
{


    /// <summary>
    /// Static class with helper methods
    /// </summary>
    public static class DatabaseUtility
    {
#if UNITY_EDITOR
        /// <summary>
        /// Reference to open database
        /// </summary>
        public static Database database;
        /// <summary>
        /// types that inherit from DatabaseEntry
        /// </summary>
        public static List<Type> entryTypes;
        /// <summary>
        /// Entry types selected container can hold
        /// </summary>
        public static List<Type> selectedContainerEntryTypes = new List<Type> { };
        /// <summary>
        /// Type names from selectedContainerEntryTypes
        /// </summary>
        public static string[] selectedContainerEntryTypeNames = {};
        /// <summary>
        /// Type names from entryTypes
        /// </summary>
        public static string[] entryTypeNames = {};
        /// <summary>
        /// Used to calculate width of dropdown menu
        /// </summary>
        public static string longestEntryTypeName = string.Empty;
        /// <summary>
        /// Used to calculate width of dropdown menu
        /// </summary>
        public static string selectedContainerLongestEntryTypeName = string.Empty;


        public static Database LoadDatabase()
        {
            // Debug.Log("DatabaseUtility  LoadDatabase");
            StoreEntryTypes();

            Database newDatabase;
            string databasePath = EditorPrefs.GetString("UNDB_databasePath");

            if (databasePath == string.Empty)
            {
                //Debug.Log("UNDB_databasePath  is  empty");
                return null;
            }
            else
            {
                newDatabase = (Database)AssetDatabase.LoadAssetAtPath(databasePath, typeof(Database));
            }
            database = newDatabase;

            return newDatabase;
        }   

        /// <summary>
        /// Returns entries from container and its children
        /// </summary>
        public static List<DatabaseEntry> GetEntries(DataBaseContainer sourceContainer, Type type = null)
        {
            List<DatabaseEntry> entries = new List<DatabaseEntry>();

            if (sourceContainer == null || database.showContainersAsColumn)
            { // get every entry in database 
                for (int containerIndex = 0; containerIndex < database.storage[0].containers.Count; containerIndex++)
                {
                    DataBaseContainer container =  database.storage[0].containers[containerIndex];
                    entries.AddRange(container.entries);

                    for (int i = 0; i < container.children.Count; i++)
                        entries.AddRange(GetEntries(container.children[i], type));
                }
            }
            else
            {
                entries.AddRange(sourceContainer.entries);

                for (int i = 0; i < sourceContainer.children.Count; i++)
                    entries.AddRange(GetEntries(sourceContainer.children[i], type));
            }

            for (int i = 0; i < entries.Count; i++)
            {
                DatabaseEntry entry = entries[i];
                if (entry.GetType() != type)
                    entries.RemoveAt(i);
            }

            return entries;
        }

        /// <summary>
        /// Move entries from one container to another 
        /// </summary>
        public static void MoveEntries(ref DataBaseContainer sourceContainer, ref DataBaseContainer destContainer)
        {
            string destPath = AssetDatabase.GetAssetPath(destContainer);
            destPath = destPath.Substring(0, destPath.Length - 6);
            string sourcePath = AssetDatabase.GetAssetPath(sourceContainer);
            sourcePath = sourcePath.Substring(0, sourcePath.Length - 6);
            
            // Debug.Log($"IsDirectoryEmpty before {DatabaseUtility.IsDirectoryEmpty(sourcePath)}");
            CreateDirectoryIfNotExists(destPath);

            for (int i = 0; i < sourceContainer.entries.Count; i++)
            {
                DatabaseEntry entry = sourceContainer.entries[i];
                entry.parent = destContainer;
                EditorUtility.SetDirty(entry);
                string assetPath = destPath + Path.DirectorySeparatorChar + entry.name + ".asset";
                AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(entry), assetPath);

            }
            // Debug.Log($"IsDirectoryEmpty after {DatabaseUtility.IsDirectoryEmpty(sourcePath)}");
            destContainer.entries.AddRange(sourceContainer.entries);
            sourceContainer.entries.Clear();
            EditorUtility.SetDirty(sourceContainer);
            EditorUtility.SetDirty(destContainer);
            sourceContainer = null; // this is why sourceContainer passed by reference

            if (IsDirectoryEmpty(sourcePath))
            {
                Directory.Delete(sourcePath);
                AssetDatabase.Refresh(); 
            }
        }

        /// <summary>
        /// Import an entry
        /// </summary>
        public static void ImportEntry( DatabaseEntry entryToImport, ref DataBaseContainer destContainer)
        {
            // Debug.Log("ImportEntry" );
            
            if (!IsNameOK(entryToImport.name))
            {
                entryToImport = null;
                return;     
            }
            string path = AssetDatabase.GetAssetPath(destContainer);
            path = path.Substring(0, path.Length - 6);
            CreateDirectoryIfNotExists(path);
            string assetPath = path + Path.DirectorySeparatorChar + entryToImport.name + ".asset";
            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(entryToImport), assetPath);
            List<DatabaseEntry> newCopies = GetNewEntriesFromContainer(destContainer);

            if ( newCopies.Count == 0)
                Debug.LogError("no new copy  " );
            else if ( newCopies.Count > 1)
                Debug.LogError("got more than 1 new copy  " );
            else
            {
                DatabaseEntry entry = newCopies[0];       
                destContainer.entries.Add(entry);
                entry.parent = destContainer;
                database.usedNames.Add(entry.name);
                EditorUtility.SetDirty(database);
                EditorUtility.SetDirty(entry);
                EditorUtility.SetDirty(destContainer);
                // Debug.Log("new entry  " + entry.name );
            }
            // entryToImport = null;
        }

        /// <summary>
        /// Checks if new name is valid. Not case sensitive. 
        /// </summary>
        public static bool IsNameOK(string newName)
        {
            newName = newName.Trim(); // remove leading and trailing spaces
            //newName = newName.ToLower(); // Make it lowercase

            if (newName == string.Empty )
            {
                EditorUtility.DisplayDialog("Invalid Name", "Please type in a name", "OK");
                return false;
            }
            
            for (int i = 0; i < database.usedNames.Count; i++)
            {
                if (database.usedNames[i].ToLower() == newName.ToLower())
                {
                    EditorUtility.DisplayDialog("Invalid Name", $"Name ''{newName}'' is already used in the database", "OK");
                    return false;
                }
            }

            return true;
        }

        public static void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path); // AssetDatabase.CreateFolder is weird
                AssetDatabase.Refresh();  // need this because created folder not using AssetDatabase.CreateFolder 
            }
        }

        /// <summary>
        /// Get entries not in current database
        /// </summary>
        public static List <DatabaseEntry> GetEntriesNotInDatabase(Type type)
        {
            // Debug.Log($"GetEntriesNotInDatabase");
            // string[] allGUIDs = AssetDatabase.FindAssets($" t:{type.ToString()}");
            string[] allGUIDs = AssetDatabase.FindAssets("t:" + type.ToString() );
            List <DatabaseEntry> allEntries = new List <DatabaseEntry>();

            for (int i = 0; i < allGUIDs.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(allGUIDs[i]);
                // Debug.Log($"asset path  {path}");
                allEntries.Add((DatabaseEntry)AssetDatabase.LoadAssetAtPath(path, typeof(DatabaseEntry)));
            }

            for (int i = allEntries.Count - 1; i >= 0; i--)
            {
                DatabaseEntry entry = allEntries[i];

                if (database.usedNames.Contains (entry.name))
                    allEntries.Remove(entry);
            }

            return allEntries;
        }

        /// <summary>
        /// Get new entries from container after importing
        /// </summary>
        public static List<DatabaseEntry> GetNewEntriesFromContainer (DataBaseContainer container)
        {
            string containerPath = AssetDatabase.GetAssetPath(container);
            containerPath = containerPath.Substring(0, containerPath.Length - 6) ;

            if (containerPath == string.Empty)
                return null;
            
            string[] guids = AssetDatabase.FindAssets(" t:DatabaseEntry", new[] { containerPath });
            List<DatabaseEntry> entries = new List<DatabaseEntry>();

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                // Debug.Log($"asset path  {path}");
                DatabaseEntry newEntry = (DatabaseEntry)AssetDatabase.LoadAssetAtPath(path, typeof(DatabaseEntry));

                if (!container.entries.Contains(newEntry)) // get only newly copied entries
                    entries.Add(newEntry);
            }
            
            return entries;
        }

        public static void ChangeDatabase(Database newDatabase)
        {
            //databasePath = AssetDatabase.GetAssetPath(newDatabase);
            EditorPrefs.SetString("UNDB_databasePath", AssetDatabase.GetAssetPath(newDatabase));
            StoreEntryTypes();
            database = newDatabase;
        }

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        /// <summary>
        /// Store types that inherit from DatabaseEntry in selectedContainerEntryTypes, and their names in selectedContainerEntryTypeNames
        /// </summary>
        public static void StoreEntryTypes(Type entryType = null)
        {
            // entryTypes = new List<Type> { typeof(DatabaseEntry) };
            selectedContainerEntryTypes = new List<Type> { };

            if (entryType == null)
                entryType = typeof(DatabaseEntry);
            else    
                selectedContainerEntryTypes.Add(entryType);

            foreach (Type type in System.Reflection.Assembly.GetAssembly(entryType).GetTypes().Where (x => x.IsSubclassOf(entryType)))
                selectedContainerEntryTypes.Add(type);

            selectedContainerEntryTypeNames = new string[selectedContainerEntryTypes.Count];

            for (int i = 0; i < selectedContainerEntryTypes.Count; i++)
            {
                string typeName = selectedContainerEntryTypes[i].ToString();
                selectedContainerEntryTypeNames[i] = typeName;

                if (typeName.Length > selectedContainerLongestEntryTypeName.Length)
                    selectedContainerLongestEntryTypeName = typeName;
            }
            if (entryType == typeof(DatabaseEntry))
            {
                entryTypes = new List<Type>(selectedContainerEntryTypes);
                // entryTypeNames = (string[])selectedContainerEntryTypeNames.Clone();
                entryTypeNames = new string[selectedContainerEntryTypeNames.Length + 1];
                entryTypeNames[0] = "None";
                Array.Copy(selectedContainerEntryTypeNames, 0, entryTypeNames, 1, selectedContainerEntryTypeNames.Length);
                longestEntryTypeName = selectedContainerLongestEntryTypeName;
            }
        }

        /// <summary>
        /// returns true if current database does not have any containers
        /// </summary>
        public static bool IsCurrentDatabaseEmpty()
        {
            if (database.storage == null || database.storage.Count == 0 || database.storage[0].containers == null || database.storage[0].containers.Count == 0)
                return true;
            else
                return false;
        }

#region Not used

        public static string GenerateRandomName (int charMin, int charMax)
        {
            int randomLenght = UnityEngine.Random.Range(charMin, charMax + 1);
            string name = string.Empty;

            for (int i = 0; i < randomLenght; i++)
                name = name + GetRandomLetter();
            
            return name;
        }
        
        public static char GetRandomLetter()
        { // returns a random lowercase letter.
            int randomNum = UnityEngine.Random.Range(0, 26); // Zero to 25
            char letter = (char)('a' + randomNum);
            return letter;
        }

#endregion
#endif

        /// <summary>
        /// returns true if database contains entry 
        /// </summary>
        public static bool IsEntryInDatabase(Database database, DatabaseEntry entry)
        {
            if (database == null || entry == null)
                return false;

            return database.usedNames.Contains(entry.name);
        }



    }

}
