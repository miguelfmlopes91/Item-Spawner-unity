using System;
using System.Collections;
using System.Collections.Generic;
using Types;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;

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

    private static MageData _mageData;
    private static WarriorData _warriorData;
    private static RogueData _rogueData;

    public static MageData MageInfo => _mageData;
    public static WarriorData WarriorInfo => _warriorData;
    public static RogueData RogueInfo => _rogueData;
    

    [MenuItem("Window/Enemy Designer")]
    private static void OpenWindow()
    {
        var window = GetWindow<EnemyDesignWindow>();
        window.minSize = new Vector2(600, 300);
        window.Show();
    }
    
    private void OnEnable()
    {
        InitTexture();
        InitData();
    }

    public static void InitData()
    {
        _mageData = CreateInstance<MageData>();
        _warriorData = CreateInstance<WarriorData>();
        _rogueData = CreateInstance<RogueData>();
    }
    private void InitTexture()
    {
        headerSectionTexture = new Texture2D(1, 1);
        headerSectionTexture.SetPixel(0,0,headSectionColor);
        headerSectionTexture.Apply();

        mageSectionTexture = Resources.Load<Texture2D>("icons/editor_mage_gradient");
        warriorSectionTexture = Resources.Load<Texture2D>("icons/editor_warrior_gradient");
        rogueSectionTexture = Resources.Load<Texture2D>("icons/editor_rogue_gradient");
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
        headerSection.width = position.width;
        headerSection.height = 50;
        
        mageSection.x = 0;
        mageSection.y = 50;
        mageSection.width = position.width / 3f;
        mageSection.height = position.width - 50f;
        
        warriorSection.x = position.width / 3f;
        warriorSection.y = 50;
        warriorSection.width = position.width / 3f;
        warriorSection.height = position.width - 50f;

        rogueSection.x = 2 * position.width / 3f ;
        rogueSection.y = 50;
        rogueSection.width = position.width / 3f;
        rogueSection.height = position.width - 50f;
        
        GUI.DrawTexture(headerSection,headerSectionTexture);
        GUI.DrawTexture(mageSection, mageSectionTexture);
        GUI.DrawTexture(warriorSection, warriorSectionTexture);
        GUI.DrawTexture(rogueSection, rogueSectionTexture);
    }
    
    private void DrawHeader()
    {
        GUILayout.BeginArea(headerSection);
        
        GUILayout.Label("Enemy Designer");
        
        GUILayout.EndArea();
    }
    
    private void DrawMageSettings()
    {
        GUILayout.BeginArea(mageSection);
        GUILayout.Label("Mage");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Damage Type");
        _mageData.dmgType = (MageDamageType)EditorGUILayout.EnumPopup(_mageData.dmgType);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Weapon Type");
        _mageData.wpnType = (MageWeaponType)EditorGUILayout.EnumPopup(_mageData.wpnType);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Create!", GUILayout.Height(40)))
        {
            GeneralSettings.OpenWindow(GeneralSettings.SettingsType.MAGE);
        }
        
        GUILayout.EndArea();
    }
    
    private void DrawWarriorSettings()
    {
        GUILayout.BeginArea(warriorSection);
        
        GUILayout.Label("Warrior");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Strategy Type");
        _warriorData.StrategyType = (WarriorStrategyType)EditorGUILayout.EnumPopup(_warriorData.StrategyType);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Weapon Type");
        _warriorData.WeaponType = (WarriorWeaponType)EditorGUILayout.EnumPopup(_warriorData.WeaponType);
        EditorGUILayout.EndHorizontal();
        
        if (GUILayout.Button("Create!", GUILayout.Height(40)))
        {
            GeneralSettings.OpenWindow(GeneralSettings.SettingsType.WARRIOR);
        }
        
        GUILayout.EndArea();
    }
    
    private void DrawRogueSettings()
    {
        GUILayout.BeginArea(rogueSection);
        
        GUILayout.Label("Rogue");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Damage Type");
        _rogueData.ClassType = (RogueClassType)EditorGUILayout.EnumPopup(_rogueData.ClassType);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Weapon Type");
        _rogueData.WeaponType = (RogueWeaponType)EditorGUILayout.EnumPopup(_rogueData.WeaponType);
        EditorGUILayout.EndHorizontal();
        
        if (GUILayout.Button("Create!", GUILayout.Height(40)))
        {
            GeneralSettings.OpenWindow(GeneralSettings.SettingsType.ROGUE);
        }
        
        GUILayout.EndArea();
    }
}

public class GeneralSettings : EditorWindow
{
    public enum SettingsType
    {
        MAGE,
        WARRIOR,
        ROGUE
    }

    private static SettingsType _dataSettings;
    private static GeneralSettings _window;

    public static void OpenWindow(SettingsType settings)
    {
        _dataSettings = settings;
        _window = GetWindow<GeneralSettings>();
        _window.minSize = new Vector2(250, 200);
        _window.Show();
    }

    private void OnGUI()
    {
        switch (_dataSettings)
        {
            case SettingsType.MAGE:
                DrawSettings(EnemyDesignWindow.MageInfo);
                break;
            case SettingsType.WARRIOR: 
                DrawSettings(EnemyDesignWindow.WarriorInfo);
                break;
            case SettingsType.ROGUE: 
                DrawSettings(EnemyDesignWindow.RogueInfo);
                break;
            default:
                break;
        }
    }

    public void DrawSettings(CharacterData charData)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Prefab");
        charData.prefab = (GameObject)EditorGUILayout.ObjectField(charData.prefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Max health");
        charData.maxHealth = EditorGUILayout.FloatField(charData.maxHealth);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Max Energy");
        charData.maxEnergy = EditorGUILayout.FloatField(charData.maxEnergy);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Power");
        charData.power = EditorGUILayout.Slider(charData.power, 0,100);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("% Crit Chance");
        charData.critChance = EditorGUILayout.Slider(charData.critChance, 0,charData.power);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name");
        charData.charName = EditorGUILayout.TextField(charData.charName);
        EditorGUILayout.EndHorizontal();

        if (charData.prefab == null)
        {
            EditorGUILayout.HelpBox("This enemy needs a [Prefab] before it can be created.", MessageType.Warning);
        }
        else if(string.IsNullOrEmpty(charData.charName) )
        {
            EditorGUILayout.HelpBox("This enemy needs a [name] before it can be created.", MessageType.Warning);
        }
        else if (GUILayout.Button("Finish and Save", GUILayout.Height(30)))
        {
            SaveCharacterData();
            _window.Close();
        }
        
    }

    private void SaveCharacterData()
    {
        string prefabPath;
        string newPrefabPath = "Assets/Prefabs/Characters/";
        string dataPath = "Assets/Resources/CharacterData/data/";

        switch (_dataSettings)
        {
            case SettingsType.MAGE:
                //create .asset file
                dataPath += "mage/" + EnemyDesignWindow.MageInfo.charName + ".asset";
                AssetDatabase.CreateAsset(EnemyDesignWindow.MageInfo, dataPath);

                newPrefabPath += "mage/" + EnemyDesignWindow.MageInfo.charName + ".prefab";
                prefabPath = AssetDatabase.GetAssetPath(EnemyDesignWindow.MageInfo.prefab);
                
                AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                GameObject magePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(newPrefabPath);
                if (!magePrefab.GetComponent<Mage>())
                    magePrefab.AddComponent<Mage>();

                magePrefab.GetComponent<Mage>().MageData = EnemyDesignWindow.MageInfo;
                
                break;
            
            case SettingsType.ROGUE:
                //create .asset file
                dataPath += "mage/" + EnemyDesignWindow.RogueInfo.charName + ".asset";
                AssetDatabase.CreateAsset(EnemyDesignWindow.RogueInfo, dataPath);

                newPrefabPath += "mage/" + EnemyDesignWindow.RogueInfo.charName + ".prefab";
                prefabPath = AssetDatabase.GetAssetPath(EnemyDesignWindow.RogueInfo.prefab);
                
                AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                GameObject roguePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(newPrefabPath);
                if (!roguePrefab.GetComponent<Rogue>())
                    roguePrefab.AddComponent<Rogue>();

                roguePrefab.GetComponent<Rogue>().RogueData = EnemyDesignWindow.RogueInfo;
                break;
            case SettingsType.WARRIOR:
                //create .asset file
                dataPath += "mage/" + EnemyDesignWindow.WarriorInfo.charName + ".asset";
                AssetDatabase.CreateAsset(EnemyDesignWindow.WarriorInfo, dataPath);

                newPrefabPath += "mage/" + EnemyDesignWindow.WarriorInfo.charName + ".prefab";
                prefabPath = AssetDatabase.GetAssetPath(EnemyDesignWindow.WarriorInfo.prefab);
                
                AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                GameObject warriorPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(newPrefabPath);
                if (!warriorPrefab.GetComponent<Warrior>())
                    warriorPrefab.AddComponent<Warrior>();

                warriorPrefab.GetComponent<Warrior>().WarriorData = EnemyDesignWindow.WarriorInfo;
                break;
        }
    }
}
 