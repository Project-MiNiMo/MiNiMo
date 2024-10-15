using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

class BuildScript
{
    static string[] SCENES = FindEnabledEditorScenes();

    [MenuItem("CustomBuild/Perform Build")]
    static void PerformBuildWindow()
    {
        // build for mac
        BuildPipeline.BuildPlayer(FindEnabledEditorScenes(), "Builds/Test", BuildTarget.StandaloneOSX, BuildOptions.None);
    }
    
    [MenuItem("CustomBuild/Perform Build iOS")]
    statiic void PerformBuildiOS()
    {
        BuildPipeline.BuildPlayer(FindEnabledEditorScenes(), "Builds/Test", BuildTarget.iOS, BuildOptions.None);
    }
    
    [MenuItem("CustomBuild/Perform Build Android")]
    static void PerformBuildAndroid()
    {
        BuildPipeline.BuildPlayer(FindEnabledEditorScenes(), "Builds/Test", BuildTarget.Android, BuildOptions.None);
    }
    
    
    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }
}