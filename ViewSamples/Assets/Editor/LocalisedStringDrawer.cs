using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LocalisedString))]
public class LocalisedStringDrawer : PropertyDrawer
{
    Rect keyPosition;
    bool dropdown;
    float height;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (dropdown)
        {
            return height + 25;
        }

        return 20;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        position.width -= 34;
        position.height = 18;

        Rect valueRect = new Rect(position);
        valueRect.x += 15;
        valueRect.width -= 15;

        Rect foldButtonRect = new Rect(position);
        foldButtonRect.width = 15;

        dropdown = EditorGUI.Foldout(foldButtonRect, dropdown, "");

        position.x += 15;
        position.width -= 15;

        SerializedProperty key = property.FindPropertyRelative("key");
        keyPosition = position;
        string old = key.stringValue;

        key.stringValue = EditorGUI.TextField(keyPosition, key.stringValue);

        Debug.Log("old = " + old + " new = " + key.stringValue);

        position.x += position.width + 2;
        position.width = 17;
        position.height = 17;

        //todo добавить иконку в ресурсы для кнопки поиска
        Texture searchIcon = (Texture)Resources.Load("search");
        GUIContent searchContent = new GUIContent(searchIcon);

        if (GUI.Button(position, searchContent))
        {
            //todo разобраться с этим...
            TextLoacaliserSearchWindow.Open((string keyFromWindow) => {
                Debug.Log("hren = " + key.stringValue);
                SerializedProperty newKey = property.FindPropertyRelative("key");
                newKey.stringValue = EditorGUI.TextField(keyPosition, keyFromWindow);
            });
        }

        position.x += position.width + 2;

        //todo добавить иконку в ресурсы для кнопки поиска
        Texture storeIcon = (Texture)Resources.Load("store");
        GUIContent storeContent = new GUIContent(storeIcon);

        if (GUI.Button(position, storeContent))
        {
            TextLocaliserEditWindow.Open(key.stringValue);
        }

        if (dropdown)
        {
            var value = LocalisationSystem.GetLocalisedValue(key.stringValue);
            GUIStyle style = GUI.skin.box;
            height = style.CalcHeight(new GUIContent(value), valueRect.width);

            valueRect.height = height;
            valueRect.y += 21;
            EditorGUI.LabelField(valueRect, value, EditorStyles.wordWrappedLabel);
        }

        EditorGUI.EndProperty();
    }
}
