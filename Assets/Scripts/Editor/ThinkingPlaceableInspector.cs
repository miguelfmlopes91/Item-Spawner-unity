using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EditorProgramming
{
    [CustomEditor(typeof(ThinkingPlaceable))]
    public class ThinkingPlaceableInspector : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement container = new VisualElement();
            SerializedProperty sp = serializedObject.GetIterator();
            sp.Next(true);

            while (sp.Next(false))
            {
                PropertyField prop = new PropertyField();
                prop.SetEnabled(sp.name != "m_Script");
                container.Add(prop);
            }

            return container;
        }
    }
}
