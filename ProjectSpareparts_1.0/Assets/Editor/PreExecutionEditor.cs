using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PreExecution))]
public class PreExecutionEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        PreExecution preExe = (PreExecution)target;
        if (GUILayout.Button("Save Start Positions")) {
            preExe.SetStartPositionOfSpareParts();
        }
        if (GUILayout.Button("Save End Positions")) {
            preExe.SetEndPositionOfSpareParts();
        }
        if (GUILayout.Button("Return Objects")) {
            preExe.ReturnObjects();
        }
    }
}
