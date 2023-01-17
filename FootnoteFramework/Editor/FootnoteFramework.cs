using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class FootnoteFramework
{
    [MenuItem("Tools/Generate Footnote Framework")]
    static void GenerateFramework()
    {
        var frameworkPluginPath =
            Application.dataPath + "/Plugins/FootnoteUtilities/FootnoteFramework";

        if (!Directory.Exists(frameworkPluginPath))
        {
            Debug.LogError(
                frameworkPluginPath
                    + " not found. Please make sure to install the plugin the correct location."
                    + "If you prefer to install it in a different location, submit a Pull Request for handling this error!"
            );
            return;
        }

        CopyPrototypes(frameworkPluginPath + "/.Prototype/", Application.dataPath + "/");
        AssetDatabase.Refresh();

        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
        var utilitiesPluginPath = "Assets/Plugins/FootnoteUtilities/FootnoteUtilities";
        scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/MainMenu.unity", true));
        scenes.Add(
            new EditorBuildSettingsScene(
                utilitiesPluginPath + "/Transition/Transitions/Scenes/SimpleFade.unity",
                true
            )
        );
        scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/Game.unity", true));

        EditorBuildSettings.scenes = scenes.ToArray();

        EditorSettings.enterPlayModeOptionsEnabled = true;
        EditorSettings.enterPlayModeOptions =
            EnterPlayModeOptions.DisableSceneReload | EnterPlayModeOptions.DisableDomainReload;
    }

    static void CopyPrototypes(string sourceDir, string targetDir)
    {
        Directory.CreateDirectory(targetDir);

        foreach (var file in Directory.GetFiles(sourceDir))
            File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));

        foreach (var directory in Directory.GetDirectories(sourceDir))
            CopyPrototypes(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
    }
}
