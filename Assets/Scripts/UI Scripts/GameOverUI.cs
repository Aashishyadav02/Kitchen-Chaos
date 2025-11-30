using System;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeDeliveredText;
    
    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_onStateChanged;
        Hide();
    }

    private void KitchenGameManager_onStateChanged(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            Show();
            recipeDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
        }
        else
        {
            Hide();
        }
    }

    void Show()
    {
        gameObject.SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
