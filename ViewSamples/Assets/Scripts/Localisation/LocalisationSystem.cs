using System.Collections.Generic;

public class LocalisationSystem 
{
    public enum Language
    {
        English, Russian
    }

    public static Language selectedLanguage = Language.Russian;

    private static Dictionary<string, string> localisedEN;
    private static Dictionary<string, string> localisedRU;

    public static bool isInit;

    public static CSVLoader cSVLoader;

    public static void Init()
    {
        cSVLoader = new CSVLoader();
        cSVLoader.LoadCSV();

        UpdateDictionaries();

        isInit = true;
    }

    public static void UpdateDictionaries()
    {
        localisedEN = cSVLoader.GetDictionaryValues("en");
        localisedRU = cSVLoader.GetDictionaryValues("ru");
    }
    
    public static Dictionary<string, string> GetDictionaryForEditor()
    {
        if (!isInit) { Init(); }
        return localisedEN;
    }

    public static string GetLocalisedValue(string key)
    {
        if (!isInit) { Init(); }
        string value = key;

        switch (selectedLanguage)
        {
            case Language.English:
                localisedEN.TryGetValue(key, out value);
                break;
            case Language.Russian:
                localisedRU.TryGetValue(key, out value);
                break;
        }

        return value;
    }

    public static void Add(string key, string value)
    {
        if (value.Contains("\""))
        {
            value.Replace('"', '\"');
        }

        if (cSVLoader == null)
        {
            cSVLoader = new CSVLoader();
        }

        cSVLoader.LoadCSV();
        cSVLoader.Add(key, value);
        cSVLoader.LoadCSV();

        UpdateDictionaries();
    }

    public static void Replace(string key, string value)
    {
        if (value.Contains("\""))
        {
            value.Replace('"', '\"');
        }

        if (cSVLoader == null)
        {
            cSVLoader = new CSVLoader();
        }

        cSVLoader.LoadCSV();
        cSVLoader.Edit(key, value);
        cSVLoader.LoadCSV();

        UpdateDictionaries();
    }

    public static void Remove(string key)
    {
        if (cSVLoader == null)
        {
            cSVLoader = new CSVLoader();
        }

        cSVLoader.LoadCSV();
        cSVLoader.Remove(key);
        cSVLoader.LoadCSV();

        UpdateDictionaries();
    }
}
