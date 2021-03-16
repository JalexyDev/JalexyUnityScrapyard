using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using ICSharpCode.NRefactory.Ast;

public class TextLocaliserEditWindow : EditorWindow
{
    public string key;
    public string value;

    public static void Open(string key)
    {
        TextLocaliserEditWindow window = CreateInstance<TextLocaliserEditWindow>();
        window.titleContent = new GUIContent("Localiser Window");
        window.ShowUtility();
        window.key = key;
    }

    private void OnGUI()
    {
        key = EditorGUILayout.TextField("Key: ", key);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Value: ", GUILayout.MaxWidth(50));

        EditorStyles.textArea.wordWrap = true;
        value = EditorGUILayout.TextArea(value, EditorStyles.textArea, GUILayout.Height(100), GUILayout.Width(400));
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Add"))
        {
            if (LocalisationSystem.GetLocalisedValue(key) != string.Empty)
            {
                LocalisationSystem.Replace(key, value);
            }
            else
            {
                LocalisationSystem.Add(key, value);
            }
        }

        minSize = new Vector2(460, 250);
        maxSize = minSize;
    }
}

public class TextLoacaliserSearchWindow : EditorWindow
{
    public string value = "";
    public Vector2 scroll;
    public Dictionary<string, string> dictionary;

    private static Action<string> windowCallBack;

    //todo добавить параметр с полем в которое будет установлен Key
    //todo добавить кнопку для установки Key в это поле
    public static void Open(Action<string> callBack)
    {
        windowCallBack = callBack;

        TextLoacaliserSearchWindow window = CreateInstance<TextLoacaliserSearchWindow>();
        window.titleContent = new GUIContent("Localisation Search");

        Vector2 mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        Rect r = new Rect(mouse.x - 450, mouse.y + 10, 10, 10);
        window.ShowAsDropDown(r, new Vector2(500, 300));
    }

    private void OnEnable()
    {
        dictionary = LocalisationSystem.GetDictionaryForEditor();
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal("Box");
        EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel);
        value = EditorGUILayout.TextField(value);
        EditorGUILayout.EndHorizontal();

        GetSearchResults();
    }

    private void GetSearchResults()
    {
        if (value == null) { return; }

        EditorGUILayout.BeginVertical();
        scroll = EditorGUILayout.BeginScrollView(scroll);
        foreach(KeyValuePair<string, string> element in dictionary)
        {
            if (element.Key.ToLower().Contains(value.ToLower()) ||
                element.Value.ToLower().Contains(value.ToLower()))
            {
                EditorGUILayout.BeginHorizontal("box");

                //todo Добавить иконку в ресурсы
                Texture closeIcon = (Texture)Resources.Load("close");

                GUIContent content = new GUIContent(closeIcon);

                if (GUILayout.Button(content, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                {
                    if (EditorUtility.DisplayDialog("Remove Key " + element.Key + "?", "This will remove the element from localisation, are you sure?", "Do it"))
                    {
                        LocalisationSystem.Remove(element.Key);
                        AssetDatabase.Refresh();
                        LocalisationSystem.Init();
                        dictionary = LocalisationSystem.GetDictionaryForEditor();
                    }
                }

                EditorGUILayout.TextField(element.Key);
                EditorGUILayout.LabelField(element.Value);


                //todo Добавить иконку в ресурсы
                Texture setKeyIcon = (Texture)Resources.Load("setKey");

                GUIContent setKeyContent = new GUIContent(setKeyIcon);

                if (GUILayout.Button(setKeyContent, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                {
                    windowCallBack?.Invoke(element.Key);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}
