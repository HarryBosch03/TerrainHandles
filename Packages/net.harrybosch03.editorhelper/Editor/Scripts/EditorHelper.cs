using System;
using UnityEditor;
using UnityEngine;

public class EditorHelper
{
    public const string Actions = "Actions/";
    public const string Window = "Window/";
    public const string SubMenu = "Terrain Handles/";

    public static void Div()
    {
        var rect = EditorGUILayout.GetControlRect();
        rect.y += rect.height / 2.0f;
        rect.height = 1.0f;
        EditorGUI.DrawRect(rect, new Color(1.0f, 1.0f, 1.0f, 0.1f));
    }

    private bool GetFoldout(string reference)
    {
        return EditorPrefs.GetBool(GetType().Name + "." + reference, false);
    }

    private void SetFoldout(string reference, bool state)
    {
        EditorPrefs.SetBool(GetType().Name + "." + reference, state);
    }

    private bool Foldout(string reference)
    {
        var index = reference.IndexOf("##") - 1;
        var name = index > 0 ? reference[..(index)] : reference;

        var state = GetFoldout(reference);
        state = EditorGUILayout.Foldout(state, name, true);
        SetFoldout(reference, state);
        return state;
    }

    public void Section(string reference, Action contents)
    {
        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            if (!Foldout(reference)) return;
                
            using (new EditorGUI.IndentLevelScope())
            {
                contents();
            }
        }
    }
}