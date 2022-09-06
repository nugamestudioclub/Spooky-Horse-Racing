#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LineRendererSmoother))]

public class LineSmootherDrawer : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        LineRendererSmoother lrs = (LineRendererSmoother)target;

        if (GUILayout.Button("Smooth Line"))
        {
            lrs.Smooth();
        }
        if (GUILayout.Button("Generate Collider"))
        {
            lrs.GenerateEdgeCollider();
        }
        if (GUILayout.Button("Simplify Line"))
        {

            lrs.SimplifyLine();
        }
        //EditorGUI.EndProperty();
    }
}
#endif