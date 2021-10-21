using GameTroopers.UI;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : Menu, IOnMenuLoaded
{
    public void OnMenuLoaded()
    {
        closeButton.onClick.AddListener(OnCloseButton);
    }

    private void OnCloseButton()
    {
        GameManager.Instance.BackEvent();
    }

    protected override void HandleGoBack()
    {
        GameManager.Instance.ShowMainMenu();
    }
    
    [SerializeField] Button closeButton;
}
