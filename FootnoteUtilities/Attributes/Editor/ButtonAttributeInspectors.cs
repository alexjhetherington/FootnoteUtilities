using System.Reflection;
using UnityEditor;
using UnityEngine;

public partial class ObjectInspector : UnityEditor.Editor
{
    MethodInfo[] Methods =>
        target
            .GetType()
            .GetMethods(
                BindingFlags.Instance
                    | BindingFlags.Static
                    | BindingFlags.NonPublic
                    | BindingFlags.Public
            );

    void DrawMethods()
    {
        if (Methods.Length < 1)
            return;

        foreach (var method in Methods)
        {
            var buttonAttribute = (ButtonAttribute)method.GetCustomAttribute(
                typeof(ButtonAttribute)
            );

            if (buttonAttribute != null)
                DrawButton(buttonAttribute, method);
        }
    }

    public void DrawButton(ButtonAttribute buttonAttribute, MethodInfo method)
    {
        var label = buttonAttribute.Label ?? method.Name;

        if (GUILayout.Button(label))
            method.Invoke(target, null);
    }
}
