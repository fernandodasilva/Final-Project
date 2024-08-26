#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
// using static ENDB.DatabaseUtility;

namespace ENDB
{
    /// <summary>
    /// This class handles custom editor window
    /// </summary>
    public partial class DatabaseEditorWindow : EditorWindow
    {
    #region Fields

        bool showEventListenersPrefabs = false;
        bool showEventRaisersPrefabs = false;
        bool showEventListenersScene = false;
        bool showEventRaisersScene = false;
        static Dictionary <GameEvent, List<string>>  EventListenersPrefabs = new  Dictionary <GameEvent, List<string>> ();
        static Dictionary <GameEvent, List<string>> EventRaisersPrefabs = new  Dictionary <GameEvent, List<string>> ();
        static Dictionary <GameEvent, List<string>> EventListenersScene = new  Dictionary <GameEvent, List<string>> ();
        static Dictionary <GameEvent, List<string>> EventRaisersScene = new  Dictionary <GameEvent, List<string>> ();
        /// <summary>
        /// Reference to open database
        /// </summary>
        static Database database = null;
        Vector2 LeftWindowScrollPos = Vector2.zero;
        Vector2 rightWindowScrollPos = Vector2.zero;
        private Vector2 mainWindowScrollPos = Vector2.zero;
        /// <summary>
        /// Entry name to find
        /// </summary>
        string entryNameToFind = string.Empty;
        /// <summary>
        /// String in search text field
        /// </summary>
        string enteredEntryNameToFind = string.Empty;
        /// <summary>
        /// Reference to database editor window
        /// </summary>
        public static DatabaseEditorWindow databaseWindow = null;
        /// <summary>
        /// Path to open database
        /// </summary>
        string databasePath = string.Empty;
        /// <summary>
        /// String in 'new name' text field
        /// </summary>
        string newName = string.Empty;
        /// <summary>
        /// Currently selected container
        /// </summary>
        DataBaseContainer selectedContainer;
        /// <summary>
        /// Currently selected entry
        /// </summary>
        DatabaseEntry selectedEntry;
        /// <summary>
        /// Container to move entries from
        /// </summary>
        DataBaseContainer sourceContainer;
        Color defaultGUIcolor;
        /// <summary>
        /// Currently selected container and its parents
        /// </summary>
        List<DataBaseContainer> selectedContainers = new List<DataBaseContainer>();
        /// <summary>
        /// List of entries to search
        /// </summary>
        List<DatabaseEntry> entriesToSearch = new List<DatabaseEntry>();
        /// <summary>
        /// Entries that match search string
        /// </summary>
        List<DatabaseEntry> foundEntries = new List<DatabaseEntry>();
        /// <summary>
        /// database options are shown when true 
        /// </summary>
        bool showDatabaseOptions = false;
        /// <summary>
        /// container options are shown when true 
        /// </summary>
        bool showContainerOptions = false;
        /// <summary>
        /// entry options are shown when true 
        /// </summary>
        bool showEntryOptions = false;
        /// <summary>
        /// true when searching for entry
        /// </summary>
        bool searching = false;
        private float leftWindowHeight;
        int selectedEntryType;
        private Vector2 middleWindowScrollPos;
        /// <summary>
        /// GUI style for right window title 
        /// </summary>
        GUIStyle titleStyle;
        /// <summary>
        /// Container button GUI style
        /// </summary>
        GUIStyle containerStyle; 
        /// <summary>
        /// Entry button GUI style
        /// </summary>
        GUIStyle entryStyle;
        /// <summary>
        /// database root button GUI style
        /// </summary>
        GUIStyle dbRootStyle ;
        Rect leftWindowRect;
        /// <summary>
        /// True when renaming entry
        /// </summary>
        private bool renamingEntry;
        private Color dividerColor = new Color(0.7f, 0.7f, 0.7f);
        private Color dividerColorProSkin = new Color(0.3f, 0.3f, 0.3f);
        /// <summary>
        /// List of entries that can be imported into open database 
        /// </summary>
        private List <DatabaseEntry> entriesToImport;
        private DatabaseEntry entryToImport;
        /// <summary>
        /// True when importing entry
        /// </summary>
        private bool importingEntry = false;
        private int selectedEntryToImport;
        /// <summary>
        /// Names of entries from entriesToImport list
        /// </summary>
        private string[] entriesToImportNames;
        /// <summary>
        /// True when importing entries by type
        /// </summary>
        private bool importingType;
        int entryTypeIndexToImport = 0;
        /// <summary>
        /// Names of types that can be imported
        /// </summary>
        string[] entryTypesForImport = new string[0];
        Editor entryEditor = null;
        /// <summary>
        /// set this to true in DatabaseEditorWindowCustomStuff to skip default inspector
        /// </summary>
        private bool skipDefaultInspector = false;
        private int searchFilterEntryType;
        private int searchFilterEntryType_;
        private bool containerNameAlwaysFitsButton;

        // Vector2 popupStyleSize = Vector2.zero;
        #endregion

        /// <summary>
        /// Opens Database Editor Window
        /// </summary>
        [MenuItem("Tools/Database/Open database window %#d", priority = 0)]  // Control + Shift + d as hotkey
        public static DatabaseEditorWindow OpenDatabaseWindow()
        {
            if (databaseWindow != null)
            {  // close database editor window if it's open
                GetWindow<DatabaseEditorWindow>().Close();
                return null;
            }

            // create new database editor window 
            databaseWindow = GetWindow<DatabaseEditorWindow>(false, "Database", true);
            // databaseWindow.titleContent.text = database.name;

            databaseWindow.minSize = new Vector2(333, 333);
            databaseWindow.Show();

            return databaseWindow;
        }

        void OnEnable()
        {
            // Debug.Log("Database  OnEnable  ");

            if (!database)
            {
                // Debug.Log("MyDatabaseEditorWindow  OnEnable NO Database");
                database = DatabaseUtility.LoadDatabase();

                if (database)
                    ChangeDatabase(database);
            }

            if (database)
            {
                databasePath =  AssetDatabase.GetAssetPath(database);
                // SetupStyles() ;
                SelectRoot();
            }
        }

        void OnGUI()
        {
            if (!database)
            {
                SelectDatabase();
                return;
            }
            SetupStyles();
            defaultGUIcolor = GUI.color;    

            mainWindowScrollPos = EditorGUILayout.BeginScrollView(mainWindowScrollPos, false, false);

            string databaseOptions = showDatabaseOptions == false ? "Show Database Options" : "Hide Database Options";
            Vector2 foldout1size = EditorStyles.foldout.CalcSize(new GUIContent(databaseOptions));
            showDatabaseOptions = EditorGUI.Foldout(new Rect(5, 5, foldout1size.x, foldout1size.y), showDatabaseOptions, databaseOptions, true);

            if (showDatabaseOptions)
            { // When showing this options foldout hide other 2
                showContainerOptions = false;
                showEntryOptions = false;
            }

            string containerOptions = showContainerOptions == false ? "Show Container Button Options" : "Hide Container Button Options";
            Vector2 foldout2size = EditorStyles.foldout.CalcSize(new GUIContent(containerOptions));
            showContainerOptions = EditorGUI.Foldout(new Rect(228, 5, foldout2size.x, foldout2size.y), showContainerOptions, containerOptions, true);

            if (showContainerOptions)
            { // When showing this options foldout hide other 2
                showDatabaseOptions = false;
                showEntryOptions = false;
            }

            string entryOptions = showEntryOptions == false ? "Show Entry Button Options" : "Hide Entry Button Options";
            Vector2 foldout3size = EditorStyles.foldout.CalcSize(new GUIContent(entryOptions));
            showEntryOptions = EditorGUI.Foldout(new Rect(500, 5, foldout3size.x, foldout3size.y), showEntryOptions, entryOptions, true);

            if (showEntryOptions)
            { // When showing this options foldout hide other 2
                showDatabaseOptions = false;
                showContainerOptions = false;
            }

            // EditorGUILayout.Space();
            GUILayout.Space(20);

            if (showDatabaseOptions)
                DisplayDatabaseOptions();
            else if (showContainerOptions)
                DisplayContainerOptions();
            else if (showEntryOptions)
                DisplayEntryOptions();

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true)); // Horizontal layout for 3 windows

            DrawLeftWindow();
            DrawDivider();

            if (searching || selectedContainer?.entries.Count > 0)
            {
                DrawMiddleWindow();
                DrawDivider();
            }

            DrawRightWindow();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            GUI.color = defaultGUIcolor;
        }

        /// <summary>
        /// Prompt user to select a database to open
        /// </summary>
        void SelectDatabase()
        {
            Database newDatabase;
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Select Database :", GUILayout.Width(122));
            newDatabase = (Database)EditorGUILayout.ObjectField(database, typeof(Database), false, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();

            if (newDatabase)
            {
                ChangeDatabase(newDatabase);
            }
        }

        public void ChangeDatabase(Database newDatabase)
        {
            //Debug.Log($"ChangeDatabase    ");
            if (newDatabase)
            {
                sourceContainer = null;
                SelectRoot();
                DatabaseUtility.ChangeDatabase(newDatabase);
                database = newDatabase;
                databasePath = AssetDatabase.GetAssetPath(newDatabase);
            }
        }

        void DisplayDatabaseOptions()
        {
            bool showContainersAsColumn = database.showContainersAsColumn;
            Database newDatabase;
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            newDatabase = (Database)EditorGUILayout.ObjectField("Current Database", database, typeof(Database), false, GUILayout.Width(366));

            if (database != newDatabase)
                ChangeDatabase(newDatabase);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Show Containers As Single Column", GUILayout.Width(211));
            database.showContainersAsColumn = EditorGUILayout.Toggle(database.showContainersAsColumn);
            EditorGUILayout.EndHorizontal();

            if (!database.showContainersAsColumn)
                database.leftWindowWidth = EditorGUILayout.IntSlider("Left Window Width", database.leftWindowWidth, (int)(position.width * .2f), (int)(position.width * .8f), GUILayout.Width(333));

            database.rightWindowWidth = EditorGUILayout.IntSlider("Right Window Width", database.rightWindowWidth, (int)(position.width * .2f), (int)(position.width * .8f), GUILayout.Width(333));

            if (!database.showContainersAsColumn)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Select Container with Parents", GUILayout.Width(177));
                database.selectContainerWithParents = EditorGUILayout.Toggle(  database.selectContainerWithParents);
                EditorGUILayout.EndHorizontal();
            }
            database.hideSearchField = EditorGUILayout.Toggle("Hide Search Text Field", database.hideSearchField);

            EditorGUILayout.EndVertical();

            if (GUI.changed)
            {
                if (showContainersAsColumn != database.showContainersAsColumn)
                    SelectRoot();  // prevent wierd stuff when user switches how containers shown

                EditorUtility.SetDirty(database); // need to mark asset as dirty to save it to disk.
            }
        }

        void DisplayContainerOptions()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Container name always fits button", GUILayout.Width(201));
            database.containerNameAlwaysFitsButton = EditorGUILayout.Toggle(database.containerNameAlwaysFitsButton);
            EditorGUILayout.EndHorizontal();

            if (!database.containerNameAlwaysFitsButton)
                database.containerButtonWidth = EditorGUILayout.IntSlider("Container Button Width", database.containerButtonWidth, 33, (int)(position.width * .3f), GUILayout.Width(333));

            database.containerSpacing  = EditorGUILayout.IntSlider("Conrainer Button Spacing",  database.containerSpacing , 0, 55, GUILayout.Width(333));

            database.containerFontSize = EditorGUILayout.IntSlider("Font Size", database.containerFontSize, 10, 55, GUILayout.Width(333));

            database.containerNormalFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Normal Font Style", database.containerNormalFontStyle, GUILayout.Width(333));

            database.containerSelectedFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Selected Font Style", database.containerSelectedFontStyle, GUILayout.Width(333));

            database.NormalContainerColor = EditorGUILayout.ColorField("Normal Button Color", database.NormalContainerColor, GUILayout.Width(333));

            database.SelectedContainerColor = EditorGUILayout.ColorField("Selected Button Color", database.SelectedContainerColor, GUILayout.Width(333));

            database.NormalContainerFontColor = EditorGUILayout.ColorField("Normal Font Color", database.NormalContainerFontColor, GUILayout.Width(333));

            database.SelectedContainerFontColor = EditorGUILayout.ColorField("Selected Font Color", database.SelectedContainerFontColor, GUILayout.Width(333));

            EditorGUILayout.EndVertical();

            if (GUI.changed)
            {
                if (database.containerNameAlwaysFitsButton != containerNameAlwaysFitsButton)
                {
                    SetContainerButtonWidthToLongestName();
                    containerNameAlwaysFitsButton = database.containerNameAlwaysFitsButton;
                }
                EditorUtility.SetDirty(database);
            }
        }

        void DisplayEntryOptions()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            database.entryFontSize = EditorGUILayout.IntSlider("Font Size", database.entryFontSize, 10, 55, GUILayout.Width(333));

            database.entryNormalFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Normal Font Style", database.entryNormalFontStyle, GUILayout.Width(333));

            database.entrySelectedFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Selected Font Style", database.entrySelectedFontStyle, GUILayout.Width(333));

            database.NormalEntryColor = EditorGUILayout.ColorField("Normal Button Color", database.NormalEntryColor, GUILayout.Width(333));

            database.SelectedEntryColor = EditorGUILayout.ColorField("Selected Button Color", database.SelectedEntryColor, GUILayout.Width(333));

            database.NormalEntryFontColor = EditorGUILayout.ColorField("Normal Font Color", database.NormalEntryFontColor, GUILayout.Width(333));

            database.SelectedEntryFontColor = EditorGUILayout.ColorField("Selected Font Color", database.SelectedEntryFontColor, GUILayout.Width(333));

            EditorGUILayout.EndVertical();

            if (GUI.changed)
                EditorUtility.SetDirty(database);
        }

        void DrawLeftWindow()
        {
            int leftWindowWidth = database.leftWindowWidth;

            if (database.showContainersAsColumn)
                // leftWindowWidth = database.containerButtonWidth + containerStyle.margin.left  + containerStyle.margin.right ;
                leftWindowWidth = (int)GetLeftWindowWidth() ;

            leftWindowRect = (Rect)EditorGUILayout.BeginVertical(GUILayout.Width(leftWindowWidth));
            // leftWindowRect = (Rect)EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(false));

            if (leftWindowRect.height > 0)
                leftWindowHeight = leftWindowRect.height;
            //Make left window scrollable
            LeftWindowScrollPos = EditorGUILayout.BeginScrollView(LeftWindowScrollPos, false, false, GUILayout.ExpandWidth(false));

            if (database.showContainersAsColumn)
                DisplayContainersAsColumn();
            else
                DisplayContainersAsTree();

            EditorGUILayout.Space();
            EditorGUILayout.EndScrollView();    

            if (!database.hideSearchField)
            {
                if (!database.showContainersAsColumn)
                    EditorGUILayout.BeginHorizontal();

                GUILayout.Label($"Find entry", GUILayout.Width(60));

                Vector2 size = EditorStyles.label.CalcSize(new GUIContent(enteredEntryNameToFind + "  "));
                // GUI.SetNextControlName("SearchTextField");
                enteredEntryNameToFind = EditorGUILayout.TextField( enteredEntryNameToFind, GUILayout.Width(Mathf.Max(70, size.x )));

                if (!database.showContainersAsColumn)
                    GUILayout.Space(33);
                    
                GUILayout.Label($"Filter by type", GUILayout.Width(80));
                Vector2 popupStyleSize = EditorStyles.popup.CalcSize(new GUIContent(DatabaseUtility.longestEntryTypeName + " "));      
                searchFilterEntryType = EditorGUILayout.Popup(searchFilterEntryType, DatabaseUtility.entryTypeNames, GUILayout.Width(popupStyleSize.x));
                   
                if (!database.showContainersAsColumn)
                    EditorGUILayout.EndHorizontal();

                if ( enteredEntryNameToFind != entryNameToFind || searchFilterEntryType_ != searchFilterEntryType) 
                {
                    entryNameToFind = enteredEntryNameToFind;
                    searchFilterEntryType_ = searchFilterEntryType;
                    selectedEntry = null;
                    DoSearch();
                }
            }
            
            EditorGUILayout.BeginHorizontal();

            // if (GUILayout.Button("Test"))
            //     Test();

            // if (GUILayout.Button("RESET DATABASE"))
            //     ResetDatabase();
 
            EditorGUILayout.EndHorizontal(); 
           
            EditorGUILayout.EndVertical();
        }

        void SetupStyles() 
        {
            titleStyle = new GUIStyle(EditorStyles.toolbarButton);
            titleStyle.fontStyle = FontStyle.Bold;
            containerStyle = new GUIStyle(EditorStyles.helpBox);
            entryStyle = new GUIStyle(EditorStyles.helpBox);
            dbRootStyle = new GUIStyle(EditorStyles.helpBox);
            containerStyle.wordWrap = false;
            dbRootStyle.alignment = TextAnchor.MiddleCenter;
            dbRootStyle.fontSize = database.containerFontSize;
            containerStyle.margin = new RectOffset( database.containerSpacing ,  database.containerSpacing ,  database.containerSpacing ,  database.containerSpacing );
            containerStyle.fontSize = database.containerFontSize;
            entryStyle.wordWrap = false;
            entryStyle.fontSize = database.entryFontSize;
        }

        private void DisplayContainersAsColumn()
        {
            if (DatabaseUtility.IsCurrentDatabaseEmpty())
                return;

            List<DataBaseContainer> column = database.storage[0].containers;

            for (int containerIndex = 0; containerIndex < column.Count; containerIndex++)
            {
                DataBaseContainer container = column[containerIndex];
                int buttonWidth = database.containerButtonWidth ;

                if (container.buttonSize > 1)
                    buttonWidth = buttonWidth * container.buttonSize + containerStyle.margin.left * container.buttonSize - containerStyle.margin.left;

                GUIContent containerButtonContent = new GUIContent(container.name);
                Vector2 size = containerStyle.CalcSize(containerButtonContent);

                if (size.x > buttonWidth)
                    containerStyle.alignment = TextAnchor.MiddleLeft;
                else
                    containerStyle.alignment = TextAnchor.MiddleCenter;

                if (selectedContainers.Contains(container))
                {
                    GUI.color = database.SelectedContainerColor;
                    containerStyle.normal.textColor = database.SelectedContainerFontColor; 
                    containerStyle.fontStyle = database.containerSelectedFontStyle;
                }
                else
                {
                    GUI.color = database.NormalContainerColor;
                    containerStyle.normal.textColor = database.NormalContainerFontColor;
                    containerStyle.fontStyle = database.containerNormalFontStyle;
                }

                if (GUILayout.Button(containerButtonContent, containerStyle, GUILayout.Width(buttonWidth), GUILayout.Height(size.y)))
                {
                    // if (container == selectedContainer)
                    //     SelectRoot();
                    // else
                        SelectContainer(container);
                }
            }
            GUI.color = defaultGUIcolor;
        }

        void DisplayContainersAsTree()
        {
            //Debug.Log($" containerStyle.margin  { containerStyle.margin}");

            if (!selectedContainer)
            {
                GUI.color = database.SelectedContainerColor;
                dbRootStyle.normal.textColor = database.SelectedContainerFontColor;
                dbRootStyle.fontStyle = database.containerSelectedFontStyle;
            }
            else
            {
                GUI.color = database.NormalContainerColor;
                dbRootStyle.normal.textColor = database.NormalContainerFontColor;
                dbRootStyle.fontStyle = database.containerNormalFontStyle; 
            }

            if (GUILayout.Button($"DATABASE ROOT", dbRootStyle))
                SelectRoot();

            for (int rowIndex = 0; rowIndex < database.storage.Count; rowIndex++)
            {
                List<DataBaseContainer> row = database.storage[rowIndex].containers;
                GUILayout.BeginHorizontal();

                for (int containerIndex = 0; containerIndex < row.Count; containerIndex++)
                {
                    DataBaseContainer container = row[containerIndex];
                    int buttonWidth = database.containerButtonWidth ;

                    if (container.buttonSize > 1)
                        buttonWidth = buttonWidth * container.buttonSize + containerStyle.margin.left * container.buttonSize - containerStyle.margin.left;

                    int dummiesPlaced = 0;
                    int dummiesToPlace = 0;
                    
                    if (container.parent?.children[0] == container)
                        dummiesToPlace = GetNumDummiesToPlace(container);

                    for (int i = 0; i < dummiesToPlace; i++)
                    { // add empty space so that children are always directly below their parents
                        GUILayout.Space( database.containerButtonWidth );

                        if (dummiesPlaced == 0 && containerIndex == 0)
                            GUILayout.Space( containerStyle.margin.left * 2);
                        else
                            GUILayout.Space(containerStyle.margin.left);

                        // EditorGUILayout.LabelField($"DUMMY {currentPos}", GUILayout.Width(database.containerButtonWidth));
                        dummiesPlaced++;
                    }
                    // GUIContent containerButtonContent = new GUIContent(container.name + " " + container.posInRow + " " + dummiesToPlace);
                    GUIContent containerButtonContent = new GUIContent(container.name );
                    Vector2 size = containerStyle.CalcSize(containerButtonContent);

                    if (size.x > buttonWidth)
                        containerStyle.alignment = TextAnchor.MiddleLeft;
                    else
                        containerStyle.alignment = TextAnchor.MiddleCenter;

                    if (selectedContainers.Contains(container))
                    {
                        GUI.color = database.SelectedContainerColor;
                        containerStyle.normal.textColor = database.SelectedContainerFontColor; 
                        containerStyle.fontStyle = database.containerSelectedFontStyle;
                    }
                    else
                    {
                        GUI.color = database.NormalContainerColor;
                        containerStyle.normal.textColor = database.NormalContainerFontColor;
                        containerStyle.fontStyle = database.containerNormalFontStyle;
                    }

                    if (GUILayout.Button(containerButtonContent, containerStyle, GUILayout.Width(buttonWidth), GUILayout.Height(size.y)))
                    {
                        // if (container == selectedContainer)
                        //     SelectRoot(); // if clicked selected container, unselect it
                        // else
                        {    
                            if (database.selectContainerWithParents)
                                SelectContainer(container, true);
                            else
                                SelectContainer(container);
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUI.color = defaultGUIcolor;
        }

        /// <summary>
        /// Returns number of empty spaces we need to place before container
        /// </summary>
        int GetNumDummiesToPlace( DataBaseContainer container)
        {
            if (container.row == 0) // database root
               return 0;
            else if (container.parent.children[0] != container) // only oldest child may need it
               return 0;

            int num = 0;
            List<DataBaseContainer> row = database.storage[container.row].containers;

            if (row[0] == container)
               return container.posInRow;

            int cIndex = row.IndexOf(container);

            if (cIndex > 0)
            {
                DataBaseContainer previousC = row[cIndex - 1];

                if (previousC.buttonSize == 1)
                    num = container.posInRow - previousC.posInRow - 1; 
                else
                    num = container.posInRow - previousC.posInRow - previousC.buttonSize; 
            }
            return num;
        }

        /// <summary>
        /// Draw the divider that separates windows
        /// </summary>
        void DrawDivider()
        {
            if (EditorGUIUtility.isProSkin)
                GUI.color = dividerColorProSkin;
            else
                GUI.color = dividerColor;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true));
            EditorGUILayout.EndVertical();
            GUI.color = defaultGUIcolor;
        }

        void DrawMiddleWindow()
        {
            middleWindowScrollPos = EditorGUILayout.BeginScrollView(middleWindowScrollPos, false, false,  GUILayout.ExpandWidth(false), GUILayout.Width(GetMiddleWindowWidth() ));

            if (searching )
                DisplaySearchResults();
            else if ( selectedContainer?.entries.Count > 0)
                DisplayEntryNames();
            // Debug.Log($"MiddleWindowRect.height {MiddleWindowRect.height}");
            EditorGUILayout.EndScrollView();
        }

        void DisplayEntryNames()
        {
            for (int entryIndex = 0; entryIndex < selectedContainer.entries.Count; entryIndex++)
            {
                DatabaseEntry entry = selectedContainer.entries[entryIndex];

                if (selectedEntry == entry)
                {
                    GUI.color = database.SelectedEntryColor;
                    entryStyle.normal.textColor = database.SelectedEntryFontColor;  
                    entryStyle.fontStyle = database.entrySelectedFontStyle;
                }
                else
                {
                    GUI.color = database.NormalEntryColor;
                    entryStyle.normal.textColor = database.NormalEntryFontColor;
                    entryStyle.fontStyle = database.entryNormalFontStyle;
                }

                Vector2 size = entryStyle.CalcSize(new GUIContent(entry.name));

                if (GUILayout.Button(entry.name, entryStyle, GUILayout.Width(size.x), GUILayout.Height(size.y)))
                {
                    SelectEntry(entry);
                }
            }

            GUI.color = defaultGUIcolor;
        }

        void DisplaySearchResults()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Search results : ", GUILayout.Width(88));
            
            if (entryNameToFind.Trim().Length > 0)
            {
                for (int entryIndex = 0; entryIndex < foundEntries.Count; entryIndex++)
                {
                    DatabaseEntry entry = foundEntries[entryIndex];

                    if (selectedEntry == entry)
                    {
                        GUI.color = database.SelectedEntryColor;
                        entryStyle.normal.textColor = database.SelectedEntryFontColor;
                        entryStyle.fontStyle = database.entrySelectedFontStyle;
                    }
                    else
                    {
                        GUI.color = database.NormalEntryColor;
                        entryStyle.normal.textColor = database.NormalEntryFontColor;
                        entryStyle.fontStyle = database.entryNormalFontStyle;
                    }
                    
                    Vector2 size = entryStyle.CalcSize(new GUIContent(entry.name));
                    
                    if (GUILayout.Button($"{entry.name}", entryStyle, GUILayout.Width(size.x), GUILayout.Height(size.y)))
                    { 
                        SelectEntry(entry);
                    }
                }
            }

            GUI.color = defaultGUIcolor;
        }

        void DrawRightWindow()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(database.rightWindowWidth));
            rightWindowScrollPos = EditorGUILayout.BeginScrollView(rightWindowScrollPos, false, false);
            
            EditorGUILayout.Space();

            if (selectedEntry)
            { 
                DisplaySelectedEntry();
            }
            else if (searching)
            { 
                if (selectedContainer == null || database.showContainersAsColumn)
                    GUILayout.Label($"Searching database for ''{entryNameToFind}''", GUILayout.ExpandWidth(true));
                else
                    GUILayout.Label($"Searching ''{selectedContainer.name}'' and its children for ''{entryNameToFind}''", GUILayout.ExpandWidth(true));
            }
            else if (selectedContainer)
            {
                DisplaySelectedContainerUI();
            }
            else 
            {
                DisplayDatabaseRootWindow();
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void DisplayDatabaseRootWindow()
        {
            GUILayout.Label("DATABASE ROOT", titleStyle, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("New Container Name", GUILayout.Width(120));
            Vector2 size = EditorStyles.label.CalcSize(new GUIContent(newName + "  "));
            newName = EditorGUILayout.TextField(newName, GUILayout.Width(Mathf.Max(70, size.x )));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (GUILayout.Button("Create new container", GUILayout.Width(210)))
                CreateNewContainerInTree(newName);

            // EditorGUILayout.Space();
        }

        /// <summary>
        /// Displays content of selected entry and a couple of buttons
        /// </summary>
        void DisplaySelectedEntry()
        {
            GUILayout.Label(selectedEntry.name, titleStyle, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            
            if (searching)
                EditorGUILayout.LabelField($"Comtainer Name :  {selectedEntry.parent.name}");

            EditorGUILayout.Space();
            CustomStuffBeforeEntry();
            
            // entryEditor = Editor.CreateEditor(selectedEntry);
            if (skipDefaultInspector == false)
                entryEditor.DrawDefaultInspector(); 

            CustomStuffAfterEntry();
            EditorGUILayout.Space();
            GUILayout.FlexibleSpace(); 

            EditorGUILayout.BeginHorizontal(); 

            if (!renamingEntry)
            {
                if (GUILayout.Button("Rename This Entry"))
                {
                    newName = string.Empty;
                    renamingEntry = true;
                }

                if (GUILayout.Button("Delete This Entry"))
                    DeleteEntry(selectedEntry);
            }
            else if (renamingEntry)
            {
                if (newName == string.Empty)
                    newName = selectedEntry.name;
        
                Vector2 size1 = EditorStyles.label.CalcSize(new GUIContent(newName + "  "));
                // GUI.SetNextControlName("NewEntryNameTextField");
                newName = EditorGUILayout.TextField(newName, GUILayout.Width(Mathf.Max(50, size1.x ) ));

                if (GUILayout.Button("Enter new name and click me", GUILayout.ExpandWidth(false)))
                // if (GUILayout.Button("Enter new name and click me"), GUILayout.Width(155))
                {
                    RenameEntry(selectedEntry, newName);
                    
                }
                // Debug.Log($"GetControlID()  {GUIUtility.GetControlID(FocusType.Keyboard)}");
                // Debug.Log($"GetNameOfFocusedControl()  {GUI.GetNameOfFocusedControl()}");
                // if (focusedNewEntryName == false)
                // { // buggy if entry has text field
                //     focusedNewEntryName = true;
                //     EditorGUI.FocusTextInControl("NewEntryNameTextField"); // text field in intry gets focus
                // }
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Displays UI for selected container
        /// </summary>
        void DisplaySelectedContainerUI()
        {
            GUILayout.Label(selectedContainer.name, titleStyle, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();

            if (selectedContainer.entries.Count > 0)
            {
                EditorGUILayout.LabelField("Number of entries: " + selectedContainer.entries.Count, EditorStyles.label);
                EditorGUILayout.Space();
                // EditorGUILayout.LabelField("Entry Type: " + selectedContainer.entries[0].GetType(), EditorStyles.label);
                // EditorGUILayout.Space();    
            }

            Vector2 size = EditorStyles.label.CalcSize(new GUIContent(newName + "  "));
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("New Name:", GUILayout.Width(70));
            newName = EditorGUILayout.TextField(newName, GUILayout.Width(Mathf.Max(70, size.x )));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (database.showContainersAsColumn)
            {
                if (GUILayout.Button($"Create new container", GUILayout.Width(180)))
                    CreateNewContainer(newName);

                EditorGUILayout.Space();
            }
            else if (!database.showContainersAsColumn && selectedContainer.entries.Count == 0)
            {
                if (GUILayout.Button($"Create new container", GUILayout.Width(180)))
                    CreateNewContainerInTree(newName);

                EditorGUILayout.Space();
            }

            if (selectedContainer.children.Count == 0)
            {
                Vector2 popupStyleSize = EditorStyles.popup.CalcSize(new GUIContent(DatabaseUtility.selectedContainerLongestEntryTypeName + " "));
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("New entry type :", GUILayout.Width(100));

                selectedEntryType = EditorGUILayout.Popup(selectedEntryType, DatabaseUtility.selectedContainerEntryTypeNames, GUILayout.Width(popupStyleSize.x));
                // selectedEntryType = EditorGUILayout.Popup(selectedEntryType, DatabaseUtility.entryTypeNames, GUILayout.ExpandWidth(false));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                if (GUILayout.Button("Add new entry", GUILayout.Width(180)))
                {
                    // if (selectedContainer.entries.Count == 0)
                        AddEntry(DatabaseUtility.selectedContainerEntryTypes[selectedEntryType], newName);
                    // else if (selectedContainer.entries.Count > 0)
                    //     AddEntry(selectedContainer.entries[0].GetType(), newName);
                }
                EditorGUILayout.Space();
            
                if (importingType == false)
                {
                    if (GUILayout.Button("Import every entry of type", GUILayout.Width(180)))
                    {
                        importingType = true;
                        entryTypesForImport = new string[DatabaseUtility.selectedContainerEntryTypeNames.Length + 1];
                        entryTypesForImport[0] = "None";
                        Array.Copy(DatabaseUtility.selectedContainerEntryTypeNames, 0, entryTypesForImport, 1, DatabaseUtility.selectedContainerEntryTypeNames.Length);
                    }
                }
                else if (importingType == true)
                {
                    entryTypeIndexToImport = EditorGUILayout.Popup("Select type to Import",entryTypeIndexToImport, entryTypesForImport, GUILayout.ExpandWidth(false));

                    if (entryTypeIndexToImport > 0)
                    {
                        List <DatabaseEntry> entriesToImport = DatabaseUtility.GetEntriesNotInDatabase(DatabaseUtility.selectedContainerEntryTypes[entryTypeIndexToImport - 1]);
                    
                        if (entriesToImport.Count == 0)
                            EditorUtility.DisplayDialog("", "No entries to import.", "OK");

                        foreach (var entry in entriesToImport)
                            DatabaseUtility.ImportEntry( entry, ref selectedContainer);

                        importingType = false;
                        entryTypeIndexToImport = 0;
                    }
                }
                EditorGUILayout.Space();
                
                if (importingEntry == false)
                {
                    if (GUILayout.Button("Import single entry", GUILayout.Width(180)))
                    {
                        importingEntry = true;
                        Type entryType = typeof(DatabaseEntry);

                        if (selectedContainer.entries.Count > 0) 
                            entryType = selectedContainer.entries[0].GetType();

                        entriesToImport = new List<DatabaseEntry>(DatabaseUtility.GetEntriesNotInDatabase(entryType).ToList());
                        entriesToImportNames = new string[entriesToImport.Count + 1];
                        entriesToImportNames[0] = "None ";    

                        for (int i = 0; i < entriesToImport.Count; i++)
                        {
                            string name = entriesToImport[i].name;
                            entriesToImportNames[i+1] = name;
                            // Debug.Log($"entriesToImport {name}");
                        }
                    }
                }
                else if (importingEntry == true)
                {
                    selectedEntryToImport = EditorGUILayout.Popup("Select entry to Import",selectedEntryToImport, entriesToImportNames, GUILayout.ExpandWidth(false));

                    if (selectedEntryToImport > 0)
                    {
                        entryToImport = entriesToImport[selectedEntryToImport - 1];
                        selectedEntryToImport = 0;
                    }

                    if (entryToImport)
                    {
                        importingEntry = false;
                        // GUIUtility.keyboardControl = 0;
                        DatabaseUtility.ImportEntry( entryToImport, ref selectedContainer);
                    }
                }
                EditorGUILayout.Space();
            } 

            if (selectedContainer.entries.Count > 0)
            {   

                if (GUILayout.Button("Move entries from this container", GUILayout.ExpandWidth(false)))
                {
                    sourceContainer = selectedContainer; 
                }
                EditorGUILayout.Space();
            }

            if (CanMoveEntriesToSelectedContainer())
            {   
                if (GUILayout.Button($"Move entries from ''{sourceContainer.name}'' into this container", GUILayout.ExpandWidth(false)))
                {
                    DatabaseUtility.MoveEntries(ref sourceContainer, ref selectedContainer);
                }
            }

            GUILayout.FlexibleSpace(); 

            if (GUILayout.Button($"Delete this container", GUILayout.Width(150)))
            {
                bool deletionConfirmed = true;

                if (selectedContainer.children.Count > 0 || selectedContainer.entries.Count > 0)
                {
                    deletionConfirmed = EditorUtility.DisplayDialog("", $"Container ''{selectedContainer.name}'' is not empty. Are you sure you want to delete it?", "Yes", "No");
                }

                if (deletionConfirmed)
                    DeleteContainer(selectedContainer, true, true);
            }
        }

        bool CanMoveEntriesToSelectedContainer()
        {
            if (sourceContainer && selectedContainer.children.Count == 0 && sourceContainer != selectedContainer)
            {   
                if (selectedContainer.entries.Count == 0 || (selectedContainer.entries.Count > 0 && selectedContainer.entries[0].GetType() == sourceContainer.entries[0].GetType()))
                    return true;
            }
            return false;       
        }

        private void DoSearch()
        {
            if (!searching)
            {
                // Debug.Log("START searching ");
                searching = true;
                entriesToSearch = DatabaseUtility.GetEntries(selectedContainer);
                foundEntries = new List<DatabaseEntry>(entriesToSearch);
                // searchTextFieldControlID = GUIUtility.GetControlID(FocusType.Keyboard);
                //  Debug.Log($"searchTextFieldControlID  {searchTextFieldControlID}");
            }
            // if (searching && entryNameToFind.Trim().Length > 0)
            if (searching)
            {
                // if (entryNameToFindLength > entryNameToFind.Length)
                // { // user deleted a character from search string. Restart search. Does not fire when string is 1 char and user selects and replaces it

                    foundEntries = new List<DatabaseEntry>(entriesToSearch);
                // }

                for (int i = foundEntries.Count - 1; i >= 0; i--)
                {
                    DatabaseEntry entry = foundEntries[i];

                    if (searchFilterEntryType > 0 && entry.GetType() != DatabaseUtility.entryTypes[searchFilterEntryType - 1])
                    {
                        // Debug.Log($" search filter {DatabaseUtility.entryTypes[searchFilterEntryType - 1]}");
                        foundEntries.Remove(entry);
                    }
                    else if (!entry.name.ToLower().Contains(entryNameToFind.ToLower() ))
                        foundEntries.Remove(entry);
                }
            }
        }

        float GetMiddleWindowWidth ()
        {
            float largestWidth = 0f;
            List<DatabaseEntry> entriesList = new List<DatabaseEntry>(); 
            Vector2 size  = Vector2.zero;

            if (searching)
            {
                size = EditorStyles.label.CalcSize(new GUIContent("Search results:"));
                largestWidth = size.x + entryStyle.margin.right + entryStyle.margin.left ;

                if ( entryNameToFind.Trim().Length > 0)
                    entriesList = foundEntries;
            }
            else if (selectedContainer?.entries.Count > 0)
                entriesList = selectedContainer.entries;

            for (int i = 0; i < entriesList.Count; i++)
            { 
                DatabaseEntry entry = entriesList[i];

                if (entry == selectedEntry)
                    entryStyle.fontStyle = database.entrySelectedFontStyle;
                else
                    entryStyle.fontStyle = database.entryNormalFontStyle;

                size = entryStyle.CalcSize(new GUIContent( entry.name ));  
                float width = size.x + entryStyle.margin.right + entryStyle.margin.left ;

                if ( width > largestWidth)
                    largestWidth = width;
            }

            if (GetMiddleWindowContentHeight() > leftWindowHeight)
            { // add width of vertical scrollbar when its visible
                largestWidth = largestWidth + 18f;
                // Debug.Log($"increase  Middle Window Width   {GetMiddleWindowContentHeight()}  {leftWindowHeight}");
            }
                
            return largestWidth + 1f;  // horizontal scroll bar is still visible sometimes so add 1
        }

        float GetLeftWindowWidth ()
        {
            if (DatabaseUtility.IsCurrentDatabaseEmpty())
                return 0f;

            float width = database.containerButtonWidth + containerStyle.margin.left  + containerStyle.margin.right ;
            //  Debug.Log($"  left Window Width   {width} ");

            // if (!database.hideSearchField && width < 158f )
            //     width = 158f; // no need for this cause search text field is outside of scroll window 

            if (GetLeftWindowContentHeight() > leftWindowHeight)
            { // add width of vertical scrollbar when its visible
                width = width + 18f;
                // Debug.Log($"increase  left Window Width   {GetLeftWindowContentHeight()}  {leftWindowHeight}");
            }

            return width ;
        }

        float GetMiddleWindowContentHeight()
        {
            float contentHeight = 0f;
        
            if (searching && foundEntries.Count > 0)
            {
                Vector2 size = entryStyle.CalcSize(new GUIContent(foundEntries[0].name )); // capital letters dont matter
                float entryHieght = size.y + entryStyle.margin.top + entryStyle.margin.bottom;
                contentHeight = entryHieght * foundEntries.Count ;
            }
            else if (selectedContainer?.entries.Count > 0)
            {
                Vector2 size = entryStyle.CalcSize(new GUIContent( selectedContainer.entries[0].name )); // capital letters dont matter
                float entryHieght = size.y + entryStyle.margin.top ;
                // Debug.Log($"entries height  {entryHieght * selectedContainer.entries.Count}");
                contentHeight = entryHieght * selectedContainer.entries.Count + entryStyle.margin.bottom;
            }    

            if (contentHeight > 0f && !database.hideSearchField)
            {
                // if (database.showContainersAsColumn) 
                contentHeight += 11f;
            }

            return contentHeight;
        }

        float GetLeftWindowContentHeight()
        {
            float contentHeight = 0f;
            Vector2 size = containerStyle.CalcSize(new GUIContent(database.storage[0].containers[0].name )); // capital letters dont matter
            // float containerHieght = size.y + containerStyle.margin.top + containerStyle.margin.bottom;
            float containerHieght = size.y + database.containerSpacing;
            contentHeight = containerHieght * database.storage[0].containers.Count + database.containerSpacing * 2;

            if (!database.hideSearchField) 
                // contentHeight -= containerHieght * 2f;
                contentHeight += 76f;

            return contentHeight;
        }

        void RenameEntry(DatabaseEntry entry, string newName)
        {
            if (!DatabaseUtility.IsNameOK(newName))
                return;

            renamingEntry = false;
            GUIUtility.keyboardControl = 0;
            database.usedNames.Remove(entry.name);
            database.usedNames.Add(newName);
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(entry), newName);
            //AssetDatabase.SaveAssets();
            newName = string.Empty;
            EditorUtility.SetDirty(database); // looks like this is not needed after  AssetDatabase.RenameAsset
        }

        void DeleteContainer(DataBaseContainer container, bool updateParents, bool updateFollowingContainers = false)
        {
            // Debug.Log($"delete  {container.name}  updateParents  {updateParents}  updateFollowingContainers  {updateFollowingContainers} ");

            if (container.parent)
            {
                if (updateParents)
                {
                    if (container.parent.children.Count > 1)
                        UpdateParentsButtonSize(container, -container.buttonSize);
                    else if (container.buttonSize > 1)
                        UpdateParentsButtonSize(container, -container.buttonSize + 1);
                }
                container.parent.children.Remove(container);
                EditorUtility.SetDirty(container.parent); 
            }

            if (updateFollowingContainers)
            {
                if (!container.parent || container.parent.children.Count > 0)
                    UpdatePosInRowFollowingContainers(container, -container.buttonSize, true);
                else if (container.parent && container.buttonSize > 1)
                    UpdatePosInRowFollowingContainers(container, -container.buttonSize + 1, true);
            }

            database.usedNames.Remove(container.name);

            for (int i = container.entries.Count - 1; i >= 0; i--)
                DeleteEntry(container.entries[i]);

            for (int i = container.children.Count - 1; i >= 0; i--)
                DeleteContainer(container.children[i], false); // dont update parent 

            database.storage[container.row].containers.Remove(container);

            if (database.containerNameAlwaysFitsButton)
                SetContainerButtonWidthToLongestName();

            EditorUtility.SetDirty(database); 
            string parentPath = string.Empty;

            if (container.parent)
            {
                parentPath = AssetDatabase.GetAssetPath(container.parent);
                parentPath = parentPath.Substring(0, parentPath.Length - 6);
            }

            SelectRoot();
            AssetDatabase.MoveAssetToTrash(AssetDatabase.GetAssetPath(container));

            if (parentPath != string.Empty && DatabaseUtility.IsDirectoryEmpty(parentPath))
            {
                Directory.Delete(parentPath);
                AssetDatabase.Refresh(); 
            }
        }

        void DeleteEntry(DatabaseEntry entry)
        {
            selectedEntry = null;
            database.usedNames.Remove(entry.name);
            entry.parent.entries.Remove(entry);

            if (entry.parent.entries.Count == 0)
                SelectContainer(entry.parent); // update new entry type list

            string parentPath = AssetDatabase.GetAssetPath(entry.parent);
            parentPath = parentPath.Substring(0, parentPath.Length - 6) ;
            AssetDatabase.MoveAssetToTrash(AssetDatabase.GetAssetPath(entry));
            EditorUtility.SetDirty(entry.parent); 
            EditorUtility.SetDirty(database); 

            if (DatabaseUtility.IsDirectoryEmpty(parentPath))
            {
                Directory.Delete(parentPath);
                AssetDatabase.Refresh(); 
            }
        }

        void CreateNewContainer(string newContainerName)
        {
            newContainerName = newContainerName.Trim(); 

            if (!DatabaseUtility.IsNameOK(newContainerName))
                return;

            string path = databasePath.Substring(0, databasePath.Length - 6);
            DataBaseContainer newContainer = null;

            DatabaseUtility.CreateDirectoryIfNotExists(path);
            newContainer = ScriptableObject.CreateInstance<DataBaseContainer>();
            AssetDatabase.CreateAsset(newContainer, path + Path.DirectorySeparatorChar + newContainerName + ".asset");

            if (database.storage.Count == 0) // no containers in database
                database.storage.Add(new RowHolder(new List<DataBaseContainer> { newContainer }));
            else
                database.storage[0].containers.Add(newContainer);

            if (database.containerNameAlwaysFitsButton)
            {
                Vector2 size = containerStyle.CalcSize(new GUIContent(newContainerName));

                if (size.x > database.containerButtonWidth)
                    database.containerButtonWidth = (int)size.x;
            }

            database.usedNames.Add(newContainerName);
            newName = string.Empty;
            EditorUtility.SetDirty(newContainer);
            EditorUtility.SetDirty(database);
            SelectContainer(newContainer);
            GUIUtility.keyboardControl = 0; // avoid search field getting focus bug 
        }

        void CreateNewContainerInTree(string newContainerName)
        {
            newContainerName = newContainerName.Trim(); 

            if (!DatabaseUtility.IsNameOK(newContainerName))
                return;

            string path = databasePath.Substring(0, databasePath.Length - 6);
            DataBaseContainer newContainer = ScriptableObject.CreateInstance<DataBaseContainer>();
            // DataBaseContainer parent = selectedContainer;

            if (selectedContainer == null) //  database root
            {
                DatabaseUtility.CreateDirectoryIfNotExists(path);
                AssetDatabase.CreateAsset(newContainer, path + Path.DirectorySeparatorChar + newContainerName + ".asset");

                if (database.storage.Count == 0 )  // no containers in database
                    database.storage.Add(new RowHolder(new List<DataBaseContainer> { newContainer }));
                else if (database.storage[0].containers.Count == 0)  // no containers in database
                    database.storage[0].containers.Add(newContainer);
                else
                {
                    List<DataBaseContainer> rootRow = database.storage[0].containers;
                    DataBaseContainer lastC = rootRow[rootRow.Count - 1];
                    rootRow.Add(newContainer);
                    newContainer.posInRow = lastC.posInRow + lastC.buttonSize;
                }
            }
            else if (selectedContainer)
            {
                // Debug.Log("selectedContainer.row" + selectedContainer.row);
                path = AssetDatabase.GetAssetPath(selectedContainer);
                path = path.Substring(0, path.Length - 6);
                DatabaseUtility.CreateDirectoryIfNotExists(path);
                AssetDatabase.CreateAsset(newContainer, path + Path.DirectorySeparatorChar + newContainerName + ".asset");
                newContainer.parents = new List<DataBaseContainer>(selectedContainer.parents);
                newContainer.parents.Add(selectedContainer);
                newContainer.parent = selectedContainer;
                newContainer.row = selectedContainer.row + 1;
                newContainer.posInRow = selectedContainer.posInRow + selectedContainer.children.Count;
                selectedContainer.children.Add(newContainer);
                                
                if (selectedContainer.children.Count > 1)
                {
                    UpdatePosInRowFollowingContainers(newContainer.row, newContainer.posInRow, 1, true);
                    UpdateParentsButtonSize(newContainer, 1);
                }

                if (database.storage.Count < newContainer.row + 1)  //  row is empty
                    database.storage.Add(new RowHolder(new List<DataBaseContainer> { newContainer }));
                else  //  row has containers
                    database.storage[newContainer.row].containers.Add(newContainer);

                // sort them by their pos in row
                database.storage[newContainer.row].containers = database.storage[newContainer.row].containers.OrderBy(c => c.posInRow).ToList();
                EditorUtility.SetDirty(selectedContainer);    
            }
            
            if (database.containerNameAlwaysFitsButton)
            {
                Vector2 size = containerStyle.CalcSize(new GUIContent(newContainerName));

                if (size.x > database.containerButtonWidth)
                    database.containerButtonWidth = (int)size.x;
            }
            database.usedNames.Add(newContainerName);
            newName = string.Empty;
            EditorUtility.SetDirty(newContainer);
            EditorUtility.SetDirty(database);

            if (database.selectContainerWithParents)
                SelectContainer(newContainer, true);
            else
                SelectContainer(newContainer);

            GUIUtility.keyboardControl = 0; // avoid search field getting focus bug 
        }

        /// <summary>
        /// update button size for every parent of container
        /// </summary>
        void UpdateParentsButtonSize(DataBaseContainer container, int update)
        {
            for (int i = 0; i < container.parents.Count; i++)
            {
                DataBaseContainer parent = container.parents[i];
                // Debug.Log($"UpdateButtonSize {parent.name} {update}");
                parent.buttonSize += update; 
                UpdatePosInRowFollowingContainers(parent, update);
                EditorUtility.SetDirty(parent);

                if (parent.buttonSize < 1)
                    Debug.LogError($"{parent.name} button Size < 1");
            }
        }

        /// <summary>
        /// update pos in row for every container in row after parameter container
        /// </summary>
        void UpdatePosInRowFollowingContainers(DataBaseContainer container, int update, bool updateChildren = false)
        {
            // Debug.Log($"UpdatePosInRowNew container {container.name}");
            List<DataBaseContainer> row = database.storage[container.row].containers;
            int cIndex = row.IndexOf(container);

            for (int i = cIndex + 1; i < row.Count; i++)
            {
                DataBaseContainer c = row[i];
                // Debug.Log($"UpdatePosInRowFollowingContainers {c.name} {c.posInRow}");

                if (updateChildren)
                    UpdatePosInRowWithChildren (c , update);
                else
                    c.posInRow += update; 

                EditorUtility.SetDirty(c);

                if (c.posInRow < 0)
                    Debug.LogError($"{c.name} pos < 0");
            }
        }

        /// <summary>
        /// update pos in row for every container in row 
        /// </summary>
        void UpdatePosInRowFollowingContainers(int rowNum, int startPos, int update, bool updateChildren = false)
        {
            // Debug.Log($"UpdatePosInRowFollowingContainers rowNum {rowNum} startPos {startPos}");
            List<DataBaseContainer> row = database.storage[rowNum].containers;

            for (int i = 0; i < row.Count; i++)
            {
                DataBaseContainer c = row[i];

                if (c.posInRow >= startPos)
                {
                    // Debug.Log($"UpdatePosInRowFollowingContainers {c.name} {c.posInRow}");

                    if (updateChildren)
                        UpdatePosInRowWithChildren (c , update);
                    else
                        c.posInRow += update; 

                    EditorUtility.SetDirty(c);

                    if (c.posInRow < 0)
                        Debug.LogError($"{c.name} pos < 0");
                }
            }
        }

        /// <summary>
        /// update pos in row for container and all its children
        /// </summary>
        void UpdatePosInRowWithChildren(DataBaseContainer container, int update)
        {
            // Debug.Log($"UpdatePosInRowWithChildren {container.name} {container.posInRow}");
            container.posInRow += update; 
            EditorUtility.SetDirty(container);

            if (container.posInRow < 0)
                Debug.LogError($"{container.name} pos < 0");

            for (int i = 0; i < container.children.Count; i++)
                UpdatePosInRowWithChildren( container.children[i], update);
        }

        void StopSearching()
        {
            searching = false;
            entryNameToFind = string.Empty;
            enteredEntryNameToFind = string.Empty;
            searchFilterEntryType_ = 0;
            searchFilterEntryType = 0;
            GUIUtility.keyboardControl = 0;
        }

        void SelectRoot()
        {
            StopSearching();
            selectedEntryType = 0;
            renamingEntry = false;
            importingEntry = false;
            importingType = false;
            selectedContainer = null;
            selectedEntry = null;
            newName = string.Empty;
            selectedContainers.Clear();
            DatabaseUtility.StoreEntryTypes();
            // GUIUtility.keyboardControl = 0;
        }

        void SelectContainer(DataBaseContainer container , bool selectParents = false)
        {
            // Debug.Log($"selectedContainer {container.name}");	
            StopSearching();
            selectedEntryType = 0;
            renamingEntry = false;
            importingEntry = false;
            importingType = false;
            selectedEntry = null;
            newName = string.Empty;
            selectedContainers.Clear();

            selectedContainer = container;
            selectedContainers.Add(container);

            if (selectParents)
                selectedContainers.AddRange(container.parents);

            if (container.children.Count == 0)
            {
                if (container.entries.Count == 0)
                    DatabaseUtility.StoreEntryTypes();
                else if (container.entries.Count > 0)
                {
                    DatabaseUtility.StoreEntryTypes(container.entries[0].GetType());  

                    if (container.entries[0].GetType() == typeof(GameEvent))
                    {               
                        GetEventListeners();
                        GetEventRaisers();
                    }
                }
            }
        }

        void SelectEntry(DatabaseEntry entry)
        {
            // Debug.Log($"entry type {entry.GetType()}");	
            renamingEntry = false;
            importingEntry = false;
            importingType = false;
            GUIUtility.keyboardControl = 0;
            newName = string.Empty;
            selectedEntry = entry;
            entryEditor = Editor.CreateEditor(entry);
        }

        /// <summary>
        /// Create new database entry
        /// </summary>
        void AddEntry(Type entryType, string name)
        {
            name = name.Trim(); 

            // dEBDatabaseUtility.entryTypes[selectedEntryType], newName);
            if (!DatabaseUtility.IsNameOK(newName))
                return;

            database.usedNames.Add(name);
            string path = AssetDatabase.GetAssetPath(selectedContainer);
            path = path.Substring(0, path.Length - 6);
            DatabaseUtility.CreateDirectoryIfNotExists(path);
            path = path + Path.DirectorySeparatorChar + name + ".asset";
            DatabaseEntry newEntry = (DatabaseEntry)ScriptableObject.CreateInstance(entryType);
            AssetDatabase.CreateAsset(newEntry, path);
            selectedContainer.entries.Add(newEntry);
            newEntry.parent = selectedContainer;
            newName = string.Empty;
            EditorUtility.SetDirty(selectedContainer);
            EditorUtility.SetDirty(newEntry);
            EditorUtility.SetDirty(database);
            SelectEntry(newEntry);
            GUIUtility.keyboardControl = 0; // remove focus from new name text field
        }
		
        private void SetContainerButtonWidthToLongestName()
        {
            database.containerButtonWidth = 0;

            for (int rowIndex = 0; rowIndex < database.storage.Count; rowIndex++)
            {
                List<DataBaseContainer> row = database.storage[rowIndex].containers;

                for (int containerIndex = 0; containerIndex < row.Count; containerIndex++)
                {
                    DataBaseContainer container = row[containerIndex];
                    Vector2 size = containerStyle.CalcSize(new GUIContent(container.name));

                    if (size.x > database.containerButtonWidth)
                    {
                        database.containerButtonWidth = (int)size.x;
                        // EditorUtility.SetDirty(database);
                    }
                }
            }
        }

		void DrawPreviewIcon()
		{
			FieldInfo[] objectFields = selectedEntry.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);

            for (int i = 0; i < objectFields.Length; i++)
            {
                DatabaseAssetPreview previewAttribute = Attribute.GetCustomAttribute(objectFields[i], typeof(DatabaseAssetPreview)) as DatabaseAssetPreview;

                if (previewAttribute != null && previewAttribute.width > 0 && previewAttribute.height > 0)
                { 
                    UnityEngine.Object previewObject = (UnityEngine.Object)selectedEntry.GetType().GetField(objectFields[i].Name).GetValue(selectedEntry); 

                    if (previewObject != null) 
                        GUILayout.Label(AssetPreview.GetAssetPreview(previewObject), GUILayout.Width(previewAttribute.width), GUILayout.Height(previewAttribute.height));
                   
                    break;
                }
            }
		}

        partial void CustomStuffBeforeEntry();
		
		partial void CustomStuffAfterEntry();

        [MenuItem("Tools/Database/About", priority = 3)]
        public static void About()
        {
            EditorUtility.DisplayDialog("About", "Unity Native Database\nVersion: 1.0", "Close");
        }
        
 // delete this region if you wanna delete example folder
#region Example

        private void GetEventListeners()
        {
			EventListenersPrefabs.Clear();
			string[] allGUIDs = AssetDatabase.FindAssets("t:gameObject", new[] { "Assets" }); // 
            

            for (int i = 0; i < allGUIDs.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(allGUIDs[i]);
				GameObject go = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));

                GameEventListener listener = go.GetComponent<GameEventListener>();

                if (listener && listener.eventToListen)
                {
                    // Debug.Log($"GetEventListeners {go.name}");	
                    if (!EventListenersPrefabs.ContainsKey(listener.eventToListen))
                        EventListenersPrefabs.Add(listener.eventToListen, new List<string>{go.name});
                    else
                    {
                        List<string> names = EventListenersPrefabs[listener.eventToListen];
                        names.Add(go.name);
                        EventListenersPrefabs[listener.eventToListen] = names;
                    }
                }
            }
            EventListenersScene.Clear();
            GameEventListener[] listeners = FindObjectsOfType<GameEventListener>();

            foreach (GameEventListener listener in listeners)
            {
				if (listener && listener.eventToListen)
                {
                    if (!EventListenersScene.ContainsKey(listener.eventToListen))
                        EventListenersScene.Add(listener.eventToListen, new List<string>{listener.gameObject.name});
                    else
                    {
                        List<string> names = EventListenersScene[listener.eventToListen];
                        names.Add(listener.gameObject.name);
                        EventListenersScene[listener.eventToListen] = names;
                    }
                }
            }
        }

        private void GetEventRaisers()
        {
			EventRaisersPrefabs.Clear();
			string[] allGUIDs = AssetDatabase.FindAssets("t:gameObject", new[] { "Assets" });

            for (int i = 0; i < allGUIDs.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(allGUIDs[i]);
				GameObject go = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));

                GameEventRaiserBase raiser = go.GetComponent<GameEventRaiserBase>();

                if (raiser && raiser.eventToRaise)
                {
                    if (!EventRaisersPrefabs.ContainsKey(raiser.eventToRaise))
                        EventRaisersPrefabs.Add(raiser.eventToRaise, new List<string>{go.name});
                    else
                    {
                        List<string> names = EventRaisersPrefabs[raiser.eventToRaise];
                        names.Add(go.name);
                        EventRaisersPrefabs[raiser.eventToRaise] = names;
                    }
                }
            }

            EventRaisersScene.Clear();
            GameEventRaiserBase[] raisers = FindObjectsOfType<GameEventRaiserBase>();

            foreach (GameEventRaiserBase raiser in raisers)
            {
				if (raiser && raiser.eventToRaise)
                {
                    if (!EventRaisersScene.ContainsKey(raiser.eventToRaise))
                        EventRaisersScene.Add(raiser.eventToRaise, new List<string>{raiser.gameObject.name});
                    else
                    {
                        List<string> names = EventRaisersScene[raiser.eventToRaise];
                        names.Add(raiser.gameObject.name);
                        EventRaisersScene[raiser.eventToRaise] = names;
                    }
                }
            }
        }

        private void EventCustomInspector()
        {
			ListEventListeners();
			ListEventRaisers();

            if (EditorApplication.isPlaying)
            {
                GameEvent selectedEvent = (GameEvent)selectedEntry;
                //If the application is playing - raise/trigger the event

                if (selectedEvent && GUILayout.Button("Raise Event"))
                {
                    selectedEvent.Raise();
                }
            }
        }

        private void ListEventListeners()
        {
			if (selectedEntry?.GetType() != typeof(GameEvent))
                return;

            EditorGUILayout.Space();

            if (EventListenersScene.ContainsKey((GameEvent)selectedEntry))
            {
                showEventListenersScene = EditorGUILayout.Foldout(showEventListenersScene, "EVENT LISTENERS IN SCENE", true, EditorStyles.foldout);

                if (showEventListenersScene)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(17);
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox); // min width is limited

                    foreach (var listener in EventListenersScene[(GameEvent)selectedEntry])
                        EditorGUILayout.LabelField(listener);

                    EditorGUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.Space();
            // Debug.Log($"EventListenersPrefabs {EventListenersPrefabs.Count}");	
            if ( EventListenersPrefabs.ContainsKey((GameEvent)selectedEntry))
            {
                showEventListenersPrefabs = EditorGUILayout.Foldout(showEventListenersPrefabs, "EVENT LISTENERS IN PREFABS", true, EditorStyles.foldout);

                if (showEventListenersPrefabs)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(17);
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox); // min width is limited

                    foreach (var listener in EventListenersPrefabs[(GameEvent)selectedEntry])
                        EditorGUILayout.LabelField(listener);

                    EditorGUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        
        private void ListEventRaisers()
        {
			if (selectedEntry?.GetType() != typeof(GameEvent))
                return;

            EditorGUILayout.Space();

            if (EventRaisersScene.ContainsKey((GameEvent)selectedEntry))
            {
                showEventRaisersScene = EditorGUILayout.Foldout(showEventRaisersScene, "EVENT RAISERS IN SCENE", true, EditorStyles.foldout);

                if (showEventRaisersScene)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(17);
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox); // min width is limited

                    foreach (var raiser in EventRaisersScene[(GameEvent)selectedEntry])
                        EditorGUILayout.LabelField(raiser);

                    EditorGUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.Space();
            // Debug.Log($"EventListenersPrefabs {EventListenersPrefabs.Count}");	
            if (EventRaisersPrefabs.ContainsKey((GameEvent)selectedEntry))
            {
                showEventRaisersPrefabs = EditorGUILayout.Foldout(showEventRaisersPrefabs, "EVENT RAISERS IN PREFABS", true, EditorStyles.foldout);

                if (showEventRaisersPrefabs)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(17);
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox); // min width is limited

                    foreach (var raiser in EventRaisersPrefabs[(GameEvent)selectedEntry])
                        EditorGUILayout.LabelField(raiser);

                    EditorGUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

#endregion

        // void AddRandomEntry(int numEntries)
        // {
        //     for (int i = 0; i < numEntries; i++)
        //     {
        //         AddEntry(typeof(DatabaseEntry), DatabaseUtility.GenerateRandomName(3, 11));
        //     }
        // }

        // void Test()
        // {   
            // Debug.Log($"  left Window Height   {GetLeftWindowContentHeight()}  {leftWindowHeight}");
            // AddRandomEntries(3);
        // } 
                      
        //  public void ResetDatabase()
        // {
        //     SelectRoot();
        //     database.storage = new List<RowHolder>();
        //     database.usedNames = new List<string>();
        // }

        //void OnInspectorUpdate()
        //{
        //    Repaint();
        //}

    }
}
#endif