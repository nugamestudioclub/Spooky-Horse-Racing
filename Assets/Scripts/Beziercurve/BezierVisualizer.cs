using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class BezierVisualizer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer renderer;
    [SerializeField]
    private EdgeCollider2D bezier;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        renderer.positionCount = bezier.edgeCount;
        for (int i = 0; i < bezier.edgeCount; i++)
        {
            renderer.SetPosition(i, bezier.points[i]);
        }
    }
}
