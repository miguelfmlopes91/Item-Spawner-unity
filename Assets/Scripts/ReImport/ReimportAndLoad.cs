using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ReimportAndLoad : EditorWindow
{
    // Start is called before the first frame update
    private static ReimportAndLoad _window;
    private bool _reimporting = false;
    private bool _working = false;
    private int _progress = -1;
    
    private string[] _scripts =
    {
        "Assets/Scripts/Reimport/HelpFiles/DummyScript1",
        "Assets/Scripts/Reimport/HelpFiles/DummyScript2",
        "Assets/Scripts/Reimport/HelpFiles/DummyScript3",
        "Assets/Scripts/Reimport/HelpFiles/DummyScript4"
    };


    [MenuItem("Editor Tools/Reimport")]
    public static void Open()
    {
        _window = GetWindow<ReimportAndLoad>();
        _window.Show();
    }

    private void OnEnable()
    {
        EditorApplication.update += update;
    }
    
    private void OnDisable()
    {
        EditorApplication.update += update;
    }

    private void update()
    {
        if (!_reimporting) return;
        for (int i = 0; i < _scripts.Length; i++)
        {
            if (_progress == i - 1 && !_working)
            {
                _progress = i;
                _working = true;
                AssetDatabase.ImportAsset(_scripts[_progress]);
                EditorUtility.DisplayProgressBar("Reimporting..", _scripts[_progress],
                    (float)_progress / (_scripts.Length - 1));
            }
        }

        if (!EditorApplication.isCompiling)
        {
            _working = false;
            if (_progress == _scripts.Length - 1)
            {
                _reimporting = false;
                EditorUtility.ClearProgressBar();
                _progress = -1;
            }
        }
    }

    private void OnGUI()
    {
        if (_reimporting) return;
        if (GUILayout.Button("Reimport"))
        {
            _reimporting = true;
        }
    }
}
