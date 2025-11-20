
using UnityEditor;
using System;
using System.IO;
using UnityEngine;

public static class Build
{
    public static void iOS()
    {
        // Get the current user's Downloads folder
        string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "iOSBuild");

        // Make sure the folder exists
        if (!Directory.Exists(downloadsPath))
        {
            Directory.CreateDirectory(downloadsPath);
        }

        // Set the build path
        string path = downloadsPath;

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
            locationPathName = path,
            target = BuildTarget.iOS,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(options);
        Debug.Log("iOS build saved to: " + path);
    }
}
