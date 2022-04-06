using System.Collections.Generic;
using UnityEngine;

namespace EditorProgramming
{
    public class ShapeCreator : MonoBehaviour
    {
        [HideInInspector]
        public List<Vector3> points = new List<Vector3>();
    }
}
