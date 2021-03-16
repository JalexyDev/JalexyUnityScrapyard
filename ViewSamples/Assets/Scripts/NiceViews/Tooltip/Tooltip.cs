using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public int characterWrapLimit;
    private RectTransform tooltipTransform;
    public RectTransform canvasTransform;

    private void Awake()
    {
        tooltipTransform = GetComponent<RectTransform>();
    }

    public void SetText(string content, string header)
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;
    }

    private void Update()
    {
        Vector2 position = Input.mousePosition;

        if (position.x + tooltipTransform.rect.width > Screen.width)
        {
            position.x = Screen.width - tooltipTransform.rect.width;
        }

        if (position.y + tooltipTransform.rect.height > Screen.height)
        {
            position.y = Screen.height - tooltipTransform.rect.height;
        }

        tooltipTransform.position = position;
    }
}
