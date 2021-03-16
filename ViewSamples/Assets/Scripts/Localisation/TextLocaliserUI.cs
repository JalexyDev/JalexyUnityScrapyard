using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
[ExecuteInEditMode]
public class TextLocaliserUI : MonoBehaviour
{
    TextMeshProUGUI textField;
    public LocalisedString localisedString;

    private void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
        UpdateUIText();

        LocalisationSwitcher.SubscribeOnLanguage(this);
    }

    public void UpdateUIText()
    {
        textField.text = localisedString.value;
    }

    private void OnDestroy()
    {
        LocalisationSwitcher.RemoveFromSubscribers(this);
    }
}
