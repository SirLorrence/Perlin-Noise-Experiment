﻿using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextureCreator))]
public class TextureCreatorInspector : Editor
{
    private TextureCreator creator;
    
    public void OnEnable()
    {
        creator = target as TextureCreator;
        Undo.undoRedoPerformed += RefreshCreator;
    }
    public void OnDisable() => Undo.undoRedoPerformed -= RefreshCreator;
    
    void RefreshCreator()
    {
        if(Application.isPlaying){
            creator.FillTexture();
        }
    }
    
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck() && Application.isPlaying)
        {
            RefreshCreator();
        }
    
    }
}


 [CustomEditor(typeof(MeshGeneration))]
 public class SurfaceInspector : Editor
 {
     private MeshGeneration meshGeneration;
    
     public void OnEnable()
     {
         meshGeneration = target as MeshGeneration;
         Undo.undoRedoPerformed += RefreshCreator;
     }
     public void OnDisable() => Undo.undoRedoPerformed -= RefreshCreator;
    
     void RefreshCreator()
     {
         if(Application.isPlaying){
             meshGeneration.Rebuild();
         }
     }
    
     public override void OnInspectorGUI()
     {
         EditorGUI.BeginChangeCheck();
         DrawDefaultInspector();
         if (EditorGUI.EndChangeCheck() && Application.isPlaying)
         {
             RefreshCreator();
         }
    
     }
 }