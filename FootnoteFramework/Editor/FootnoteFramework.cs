using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class FootnoteFramework : EditorWindow
{
    [MenuItem("Tools/Footnote Framework")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(FootnoteFramework));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Generate Framework"))
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
        }
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
