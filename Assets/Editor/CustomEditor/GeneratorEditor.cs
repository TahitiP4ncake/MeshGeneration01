using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Generator))]
public class GeneratorEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        Generator generator = target as Generator;

        base.OnInspectorGUI();

        //filter parameters depending on mesh type
        
        if (GUILayout.Button("Generate Mesh"))
        {
            generator.GenerateMesh();
        }
    }
}

