using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModeManager : MonoBehaviour
{
    private readonly Color darkGray = new Color(.1f, .1f, .1f, 1);
    private readonly Color uiYellow = new Color(.1f, .1f, .0f, 1);

    public static UIModeManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ResolveColorMode()
    {
        foreach (var text in GetComponentsInChildren<Text>(true))
        {
            if (HUDManager.instance.darkTextColor == HUDManager.DarkModeTextColor.Yellow)
            {
                text.color = text.color == Color.yellow ? darkGray : Color.yellow;
            }
            else
                text.color = text.color == Color.white ? darkGray : Color.white;
        }
        foreach (var image in GetComponentsInChildren<RawImage>(true))
        {
            image.color = image.color == Color.white ? darkGray : Color.white;
        }
        foreach (var image in GetComponentsInChildren<Image>(true))
        {
            image.color = image.color == Color.white ? darkGray : Color.white;
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
