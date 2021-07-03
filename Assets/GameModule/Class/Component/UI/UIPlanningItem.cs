using System;
using GameModule.Class;
using GameModule.Class.Component;
using ItemModule.Class.Data;
using UnityEngine;
using UnityEngine.UI;

public class UIPlanningItem : MonoBehaviour
{
    public ItemType ItemType;
    public Text NameText;
    public Text DescriptionText;
    public Image Texture;
    public Button MinusButton;
    public Button AddButton;
    public Text CurrentCountText;
    public Text CostText;
    public int Cost;
    public int CurrentCount;
    
    public void OnMinusButtonClicked()
    {
        if (CurrentCount <= 0)
            return;
        var planningPanel = (PlanningPanel) GameManager.Instance.GetPanel();
        if (planningPanel == null)
            return;

        planningPanel.UpdateUsedPoints(-Cost);
        CurrentCountText.text = (--CurrentCount).ToString();
    }

    public void OnAddButtonClicked()
    {
        var planningPanel = (PlanningPanel) GameManager.Instance.GetPanel();
        if (planningPanel == null)
            return;

        if (planningPanel.GetPointsLeft() + (-Cost) < 0)
            return;
        
        planningPanel.UpdateUsedPoints(Cost);
        CurrentCountText.text = (++CurrentCount).ToString();
    }

}
