// Copyright © 2024 llllyyyynnnn. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.InputSystem;

public class LynnUI_Generate : ScriptableObject
{

    public static Texture2D PixelColorTexture(int width, int height, Color col)
    { // todo: one pixel, gradients
        
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        
        Texture2D result = new Texture2D(width, height);
        result.hideFlags = HideFlags.HideAndDontSave;
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    public static GUIStyle colorStyle(Color clr, string name = "")
    { // if untitled, refresh every frame (wip, todo)
        GUIStyle currentStyle = null;
        Texture2D background_color = PixelColorTexture(1, 1, new Color(clr.r, clr.g, clr.b, clr.a));
        currentStyle = new GUIStyle(GUI.skin.box);
        currentStyle.normal.background = background_color;
        Texture2D.Destroy(background_color);
        return currentStyle;
    }
}