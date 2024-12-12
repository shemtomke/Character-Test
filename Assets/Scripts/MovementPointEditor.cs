using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovementPoint))]
public class MovementPointEditor : Editor
{
    public override void OnInspectorGUI() {

        DrawDefaultInspector();

        MovementPoint myPoint = (MovementPoint)target;

        if(GUILayout.Button("Build Up")) {
            myPoint.BuildPathUp();
        }
        if(GUILayout.Button("Build Right")) {
            myPoint.BuildPathRight();
        }
        if(GUILayout.Button("Build Down")) {
            myPoint.BuildPathDown();
        }
        if(GUILayout.Button("Build Left")) {
            myPoint.BuildPathLeft();
        }
    }
}

