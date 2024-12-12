using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathSystem))]
public class PointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathSystem pathSystem = (PathSystem)target;

        if (GUILayout.Button("Next Sphere"))
        {
            pathSystem.GenerateNextSphere();
        }
        if (GUILayout.Button("Previous Sphere"))
        {
            pathSystem.GeneratePreviousSphere();
        }
    }
}
