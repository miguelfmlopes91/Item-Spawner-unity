using UnityEngine;
using UnityEditor;

namespace EditorProgramming
{
    [CustomEditor(typeof(ShapeCreator))]
    public class ShapeEditor : UnityEditor.Editor
    {

        private ShapeCreator shapeCreator;
        private bool needsRepaint;


        private void OnSceneGUI()
        {
            Event guiEvent = Event.current;

            if (guiEvent.type == EventType.Repaint)
            {
                Draw();
            }
            else if (guiEvent.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            }
            else
            {
                HandleInput(guiEvent);
                if (needsRepaint)
                {
                    HandleUtility.Repaint();
                }
            }
        }

        private void HandleInput(Event guiEvent)
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
            float drawPlaneHeight = 0;
            float dstToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
            Vector3 mousePosition = mouseRay.GetPoint(dstToDrawPlane);

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 &&
                guiEvent.modifiers == EventModifiers.None)
            {
                Undo.RecordObject(shapeCreator, "Add point");
                shapeCreator.points.Add(mousePosition);
                needsRepaint = true;
            }

        }

        private void Draw()
        {
            for (int i = 0; i < shapeCreator.points.Count; i++)
            {
                Vector3 nextPoint = shapeCreator.points[(i + 1) % shapeCreator.points.Count];
                Handles.color = Color.black;
                Handles.DrawDottedLine(shapeCreator.points[i], nextPoint, 4);
                Handles.color = Color.white;
                Handles.DrawSolidDisc(shapeCreator.points[i], Vector3.up, .5f);
            }

            needsRepaint = false;
        }

        private void OnEnable()
        {
            shapeCreator = target as ShapeCreator;
        }

    }
}