using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public partial class ObjectInspector : UnityEditor.Editor
{
    FieldInfo[] _resolveableFields;
    FieldInfo[] ResolvableFields
    {
        get
        {
            if (_resolveableFields == null)
            {
                List<FieldInfo> allFields = new List<FieldInfo>();

                List<FieldInfo> nonPublicFields = new List<FieldInfo>(
                    target.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                );

                List<FieldInfo> publicFields = new List<FieldInfo>(
                    target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
                );

                foreach (FieldInfo nonPublicField in nonPublicFields)
                {
                    if (
                        hasAttr(nonPublicField)
                        && isVisible(nonPublicField)
                        && isType(nonPublicField)
                    )
                        allFields.Add(nonPublicField);
                }

                foreach (FieldInfo publicField in publicFields)
                {
                    if (hasAttr(publicField) && isType(publicField))
                        allFields.Add(publicField);
                }

                _resolveableFields = allFields.ToArray();
            }
            return _resolveableFields;
        }
    }

    bool hasAttr(FieldInfo field)
    {
        return field.GetCustomAttribute(typeof(ResolveAttribute)) != null;
    }

    bool isVisible(FieldInfo field)
    {
        return field.GetCustomAttribute(typeof(SerializeField)) != null;
    }

    bool isType(FieldInfo field)
    {
        return typeof(Object).IsAssignableFrom(field.FieldType)
            || typeof(Object).IsAssignableFrom(field.FieldType.GetElementType());
    }

    void DrawResolveButton()
    {
        if (ResolvableFields.Length < 1)
            return;

        GUILayout.Space(15);
        GUILayout.Label("Resolver");
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("From Children"))
        {
            Undo.RegisterCompleteObjectUndo(target, "Auto Resolve References");

            MonoBehaviour mb = (MonoBehaviour)target;
            foreach (FieldInfo fieldInfo in ResolvableFields)
            {
                if (!fieldInfo.FieldType.IsArray && ((Object)fieldInfo.GetValue(mb)) == null)
                {
                    fieldInfo.SetValue(mb, mb.GetComponentInChildren(fieldInfo.FieldType));
                }
                else if (fieldInfo.FieldType.IsArray)
                {
                    var comps = mb.GetComponentsInChildren(fieldInfo.FieldType.GetElementType());
                    var correctlyTypedArray = System.Array.CreateInstance(
                        fieldInfo.FieldType.GetElementType(),
                        comps.Length
                    );
                    comps.CopyTo(correctlyTypedArray, 0);

                    fieldInfo.SetValue(mb, correctlyTypedArray);
                }
            }
        }

        if (GUILayout.Button("From Scene"))
        {
            Undo.RegisterCompleteObjectUndo(target, "Auto Resolve References");

            MonoBehaviour mb = (MonoBehaviour)target;

            foreach (FieldInfo fieldInfo in ResolvableFields)
            {
                //Deprecated with Unity 2020 - but we support Unity 2019
                //When deprecated - unify approach with above
                System.Type type = fieldInfo.FieldType.IsArray
                    ? fieldInfo.FieldType.GetElementType()
                    : fieldInfo.FieldType;
                var comps = FootnoteHack.FindObjectsOfTypeIncludingDisabled(type);
                var correctlyTypedArray = System.Array.CreateInstance(type, comps.Length);
                comps.CopyTo(correctlyTypedArray, 0);

                if (correctlyTypedArray.Length == 0)
                    continue;

                if (!fieldInfo.FieldType.IsArray && ((Object)fieldInfo.GetValue(mb)) == null)
                {
                    fieldInfo.SetValue(mb, correctlyTypedArray.GetValue(0));
                }
                else if (fieldInfo.FieldType.IsArray)
                {
                    fieldInfo.SetValue(mb, correctlyTypedArray);
                }
            }
        }

        if (GUILayout.Button("Clear Resolved"))
        {
            Undo.RegisterCompleteObjectUndo(target, "Clear Resolved References");

            MonoBehaviour mb = (MonoBehaviour)target;
            foreach (FieldInfo fieldInfo in ResolvableFields)
            {
                fieldInfo.SetValue(mb, null);
            }
        }
        GUILayout.EndHorizontal();
    }
}
