
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

// Tagging a class with the EditorTool attribute and no target type registers a global tool. Global tools are valid for any selection, and are accessible through the top left toolbar in the editor.
[EditorTool("Platform Tool")]
class AnotherExample : EditorTool
{
    // Serialize this value to set a default value in the Inspector.
    [SerializeField] private Texture2D m_ToolIcon;
    private GUIContent m_IconContent;
    private Vector2 scrollPosition;
    private static string testModeTargetScene = string.Empty;
    private static Scene testModePreviousActiveScene;
    private static AnotherExample toolInstance;
    
    private Rect toolbarRect;
    private Rect toolboxRect;
    private int toolboxIconSize;
    private int maxGridLength;
    

    public static AnotherExample Instance => toolInstance;
    public override GUIContent toolbarIcon => EditorGUIUtility.TrTextContentWithIcon("Platform Tool", "Platform Tool");


    
    void OnEnable()
    {
        if (ToolManager.IsActiveTool(this))
        {
            SetupTool();
            toolInstance = this;
        }

        ToolManager.activeToolChanging += EditorToolsOnActiveToolChanging;
        ToolManager.activeToolChanged += EditorToolsOnActiveToolChanged;

        EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
    }

    private void EditorApplicationOnplayModeStateChanged(PlayModeStateChange stateChange)
    {
        // If no target scene for test mode has been set then don't do anything
        if (string.IsNullOrWhiteSpace(testModeTargetScene))
        {
            return;
        }

        string currentScenePath = SceneManager.GetActiveScene().path;
        bool isInTargetRunScene = currentScenePath.Equals(testModeTargetScene);

        // Leaving edit mode to enter play mode, load the test scene before we actually leave the edit mode
        if (!isInTargetRunScene && stateChange == PlayModeStateChange.ExitingEditMode)
        {
            testModePreviousActiveScene = SceneManager.GetActiveScene();

            foreach (var camera in FindObjectsOfType<Camera>())
            {
                camera.gameObject.SetActive(false);
            }

            var targetScene = EditorSceneManager.OpenScene(testModeTargetScene, OpenSceneMode.Additive);
            if (targetScene.IsValid())
            {
                SceneManager.SetActiveScene(targetScene);
            }
        }

        // Returning to edit mode after stopping playing, remove the test scene if we automatically added it
        if (stateChange == PlayModeStateChange.EnteredEditMode && testModePreviousActiveScene.path != testModeTargetScene)
        {
            var loadedScene = SceneManager.GetSceneByPath(testModeTargetScene);
            if (loadedScene.IsValid())
            {
                EditorSceneManager.CloseScene(loadedScene, true);
            }
            if (testModePreviousActiveScene.IsValid())
            {
                SceneManager.SetActiveScene(testModePreviousActiveScene);
            }
            testModeTargetScene = string.Empty;
        }
    }
    
    protected virtual void EditorToolsOnActiveToolChanged()
    {
        if (ToolManager.IsActiveTool(this))
        {
            SetupTool();
        }
    }

    private void EditorToolsOnActiveToolChanging()
    {
        if (ToolManager.IsActiveTool(this))
        {
            TeardownTool();
        }
    }

    private void TeardownTool()
    {
        SceneView.duringSceneGui -= OnSceneGui;
        Selection.selectionChanged -= SelectionChanged;
    }

    private void OnSceneGui(SceneView sceneView)
    {
        // Don't run during play mode
        if (Application.isPlaying)
        {
            UnityEditor.Tools.current = Tool.Move;
            return;
        }
        
        // Hijack scroll requests, have to do this here because otherwise the scene gui eats it first
        if (Event.current.type == EventType.ScrollWheel && toolboxRect.Contains(Event.current.mousePosition))
        {
            scrollPosition += Event.current.delta * 50.0f;
            Event.current.Use();
        }

        if (Event.current.type == EventType.MouseDown)
        {
        }

        if (Event.current.isKey && Event.current.keyCode == KeyCode.F)
        {
            if (Selection.activeGameObject == null)
            {
            }
        }
    }
    
    private void SelectionChanged()
    {
        
    }

    private void SetupTool()
    {
        SceneView.duringSceneGui -= OnSceneGui;
        SceneView.duringSceneGui += OnSceneGui;
        
        Selection.selectionChanged += SelectionChanged;
    }


    // This is called for each window that your tool is active in. Put the functionality of your tool here.
    public override void OnToolGUI(EditorWindow window)
    {
        EditorGUI.BeginChangeCheck();

        Vector3 position = Tools.handlePosition;

        using (new Handles.DrawingScope(Color.green))
        {
            position = Handles.Slider(position, Vector3.right);
        }

        if (EditorGUI.EndChangeCheck())
        {
            Vector3 delta = position - Tools.handlePosition;

            Undo.RecordObjects(Selection.transforms, "Move Platform");

            foreach (var transform in Selection.transforms)
                transform.position += delta;
        }
    }
}