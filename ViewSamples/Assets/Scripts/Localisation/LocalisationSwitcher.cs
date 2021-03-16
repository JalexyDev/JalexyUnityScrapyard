using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[ExecuteInEditMode]
public class LocalisationSwitcher : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("GameObject/Managers/Localisation Switcher")]
    public static void AddLinearProgressBar()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Localisation/LocalisationSwitcher"));
        if (Selection.activeGameObject != null)
        {
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
        }    
    }
#endif

    private const string LANGUAGE = "LANGUAGE";

    private static LocalisationSwitcher instance;
    public LocalisationSystem.Language Language;

    private List<TextLocaliserUI> textUIElements;

    private void Awake()
    {
        instance = this;

        //todo загрузить сохраненный в преференсах язык
        Language = (LocalisationSystem.Language) PlayerPrefs.GetInt(LANGUAGE);

        LocalisationSystem.selectedLanguage = Language;
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            if (Language != LocalisationSystem.selectedLanguage)
            {
                SwitchLanguage(Language);
            }
        }
    }

    public static void SubscribeOnLanguage(TextLocaliserUI textElement)
    {
        if (instance.textUIElements == null)
        {
            instance.textUIElements = new List<TextLocaliserUI>();
        }

        instance.textUIElements.Add(textElement);
    }

    public static void RemoveFromSubscribers(TextLocaliserUI textElement)
    {
        if (instance == null ||
            instance.textUIElements == null || 
            !instance.textUIElements.Contains(textElement)) { return; }

        instance.textUIElements.Remove(textElement);
    }

    public static void SwitchLanguage(LocalisationSystem.Language language)
    {
        //todo сохранить язык в преференсы
        PlayerPrefs.SetInt(LANGUAGE, (int)instance.Language);

        instance.Language = language;
        LocalisationSystem.selectedLanguage = instance.Language;

        List<TextLocaliserUI> uiElements = instance.textUIElements;

        if (uiElements == null) { return; }

        foreach (TextLocaliserUI element in uiElements)
        {
            element?.UpdateUIText();
        }
    }
}
