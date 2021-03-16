using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelGroup : MonoBehaviour
{
    public GameObject[] panels;
    public int panelIndex;

    private void Awake()
    {
        ShowCurrentPanel();
    }

    public void SetPageIndex(int index)
    {
        panelIndex = index;
        ShowCurrentPanel();
    }

    private void ShowCurrentPanel()
    {
        for (int i = 0; i < panels.Length; i++ )
        {
            panels[i].gameObject.SetActive(i == panelIndex);
        }
    }
}
