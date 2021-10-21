﻿using GameTroopers.UI;
using UnityEngine;
using UnityEngine.UI;

public class PopupMenu : Menu, IOnMenuLoaded
{
    public void OnMenuLoaded()
    {
        closeButton.onClick.AddListener(OnCloseButton);
    }

    private void OnCloseButton()
    {
        GameManager.Instance.BackEvent();
    }

    [SerializeField] Button closeButton;
}
