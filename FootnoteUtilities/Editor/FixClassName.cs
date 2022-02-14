// FixClassName V 0.1#29092017
// By Pellegrino ~thp~ Principe
// A little script to automatically fix class FixClassName when the name
// of the relative script file has changed

using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace com.pellegrinoprincipe
{
    public class FixClassName : Editor
    {
        private static string errorMessage = "Ops, something has gone wrong: {0}.";
        private static string refactoringMessage = "The class has been successfully renamed.";
        private static string noRefactoringMessage =
            "The class name is the same of the file name. No renaming performed.";
        private static bool scriptSelected;
        private static MonoScript[] scripts;

        // hotkey to activate the item: ALT + SHIFT + f
        [MenuItem("Assets/Fix Class Name... &#f")]
        public static void Fix()
        {
            if (scripts != null && scripts.Length != 0)
            {
                MonoScript script = scripts[0];
                int selectedScript = script.GetInstanceID();

                string fsAssetPath = Application.dataPath;
                string unityAssetPath = AssetDatabase.GetAssetPath(selectedScript);
                string currentClassName = script.name;

                replaceClassName(currentClassName, AssetDatabase.GetAssetPath(selectedScript));
            }
        }

        [MenuItem("Assets/Fix Class Name... &#f", true)]
        public static bool CheckIfScriptFile()
        {
            // enable the menu item only if a script file is selected
            // get only the scripts selected...
            scripts = Selection.GetFiltered<MonoScript>(SelectionMode.Assets);

            // ... but we only use the first
            scriptSelected = scripts.Length != 0 && scripts.Length < 2;

            return scriptSelected;
        }

        private static void replaceClassName(string className, string scriptPath)
        {
            try
            {
                String[] fileText = File.ReadAllLines(scriptPath);

                for (int i = 0; i < fileText.Length; i++)
                {
                    // make the refactoring only if the class name is different
                    if (Regex.IsMatch(fileText[i], @"\bclass\b"))
                    {
                        if (Regex.IsMatch(fileText[i], "\\b" + className + "\\b"))
                        {
                            Debug.Log(noRefactoringMessage);
                            return; // skip if the name is the same
                        }

                        // match the identifier of a class so it can be replaced by 'className'
                        // we use a Positive Lookbehind...
                        String regexPattern = @"(?<=class )\w+";
                        fileText[i] = Regex.Replace(fileText[i], regexPattern, className);
                        File.WriteAllLines(scriptPath, fileText);

                        Debug.Log(refactoringMessage);
                    }
                    else if (Regex.IsMatch(fileText[i], @"\binterface\b"))
                    {
                        if (Regex.IsMatch(fileText[i], "\\b" + className + "\\b"))
                        {
                            Debug.Log(noRefactoringMessage);
                            return; // skip if the name is the same
                        }

                        // match the identifier of a class so it can be replaced by 'className'
                        // we use a Positive Lookbehind...
                        String regexPattern = @"(?<=interface )\w+";
                        fileText[i] = Regex.Replace(fileText[i], regexPattern, className);
                        File.WriteAllLines(scriptPath, fileText);

                        Debug.Log(refactoringMessage);
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.Log(String.Format(errorMessage, exc.Message));
            }
        }
    }
}
