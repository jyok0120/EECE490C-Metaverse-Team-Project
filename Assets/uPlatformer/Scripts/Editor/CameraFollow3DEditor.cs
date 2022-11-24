using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CameraFollow3D))]
public class CameraFollow3DEditor : Editor {

    CameraFollow3D myTarget;

    private void OnEnable() {
        myTarget = (CameraFollow3D)target;
    }
    
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
    }

    private void OnSceneGUI() {
        if (Event.current.type == EventType.Repaint) {
            Handles.color = Color.yellow;
            Handles.DrawWireCube(myTarget.focusArea.center, myTarget.focusAreaSize);
        }            
    }
}
