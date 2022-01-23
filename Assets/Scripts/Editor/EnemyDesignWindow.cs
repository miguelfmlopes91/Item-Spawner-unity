using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyDesignWindow : EditorWindow
{
    private Texture2D headerSectionTexture;
    private Texture2D mageSectionTexture;
    private Texture2D warriorSectionTexture;
    private Texture2D rogueSectionTexture;

    private Color headSectionColor = new Color(12f / 25f, 32f / 255f, 44f / 255f, 1f);

    private Rect headerSection;
    private Rect mageSection;
    private Rect warriorSection;
    private Rect rogueSection;
    

    [MenuItem("Window/Enemy Designer")]
    private static void OpenWindow()
    {
        EnemyDesignWindow window = (EnemyDesignWindow)GetWindow(typeof(EnemyDesignWindow));
        window.minSize = new Vector2(600, 300);
        window.Show();
    }
    
    private void OnEnable()
    {
        InitTexture();
    }

    private void InitTexture()
    {
        headerSectionTexture = new Texture2D(1, 1);
        headerSectionTexture.SetPixel(0,0,headSectionColor);
        headerSectionTexture.Apply();
    }
    
    private void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        DrawMageSettings();
        DrawRogueSettings();
        DrawWarriorSettings();
    }

    private void DrawLayouts()
    {
        headerSection.x = 0;
        headerSection.y = 0;
        headerSection.width = Screen.width;
        headerSection.height = 50;
        
        GUI.DrawTexture(headerSection,headerSectionTexture);
    }
    
    private void DrawHeader()
    {
        
    }
    
    private void DrawMageSettings()
    {
        
    }
    
    private void DrawWarriorSettings()
    {
        
    }
    
    private void DrawRogueSettings()
    {
        
    }
    
    
}
 