using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("GameObject/UI/Linear Progress Bar")]
    public static void AddLinearProgressBar()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("UI/Linear Progress Bar"));
        obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }

    [MenuItem("GameObject/UI/Radial Progress Bar")]
    public static void AddRadialProgressBar()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("UI/Radial Progress Bar"));
        obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }
#endif

    [SerializeField] private int minimum;
    [SerializeField] private int maximum;
    [SerializeField] private int current;
    [SerializeField] private Image mask;
    [SerializeField] private Image fill;
    [SerializeField] private Color color;


    private void Update()
    {
        if (Application.isEditor)
        {
            GetCurrentFill();
            UpdateColor();
        } 
    }


    public void SetBorders(int min, int max)
    {
        minimum = min;
        maximum = max;
        GetCurrentFill();
    }

    public void SetCurrentFill(int currentCount)
    {
        current = currentCount;
        GetCurrentFill();
    }

    private void GetCurrentFill()
    {
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;
        float fillAmount = currentOffset / (float)maximumOffset;
        mask.fillAmount = fillAmount;
    }

    private void UpdateColor()
    {
        fill.color = color;
    }
}
