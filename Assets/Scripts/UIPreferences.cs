using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIPreferences : MonoBehaviour
{

    private const string DarkMode = "DarkMode";

    // Start is called before the first frame update

    public static void SaveDarkMode(bool on)
    {
        PlayerPrefs.SetInt(DarkMode, on? 1: 0);
    }

    public static bool IsDarkMode()
    {
        return PlayerPrefs.GetInt(DarkMode, 0) == 1;
    }

}
