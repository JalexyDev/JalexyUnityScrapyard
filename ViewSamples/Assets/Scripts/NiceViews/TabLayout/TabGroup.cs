using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public PanelGroup panelGroup;

    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;

    private List<TabButton> tabButtons;
    private TabButton selectedTab;

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        if (selectedTab != null && selectedTab != button)
        {
            selectedTab.Deselect();
        }

        selectedTab = button;
        selectedTab.Select();

        ResetTabs();
        button.background.sprite = tabActive;

        if (panelGroup != null)
        {
            panelGroup.SetPageIndex(button.transform.GetSiblingIndex());
        }
    }

    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }
            button.background.sprite = tabIdle;
        }
    }
}
