using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModeElement : MonoBehaviour
{
    // Start is called before the first frame update

    public Texture2D LightModeSprite;
    public Texture2D DarkModeSprite;

    public void ResolveImage()
    {
        if (LightModeSprite)
        {
            GetComponent<RawImage>().texture = UIPreferences.IsDarkMode() ? DarkModeSprite : LightModeSprite;
        }
    }

    public void ResolveTextMode()
    {
        foreach (var text in GetComponentsInChildren<Text>(true))
        {
            text.fontStyle = text.fontStyle == FontStyle.Normal ? FontStyle.Bold : FontStyle.Normal;
        }

    }



}
