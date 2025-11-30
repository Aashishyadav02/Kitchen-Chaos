using System;
using TMPro;
using UnityEngine;

public class GameStartCountDownUI : MonoBehaviour
{
    private const string NUMBER_POPUP= "NumberPopup";
    [SerializeField] private TextMeshProUGUI countDownText;

    private Animator animator;
    private int previousCountDownNumber;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_onStateChanged;
        Hide();
    }

    private void KitchenGameManager_onStateChanged(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountDownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        int countDownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountDownToStartTimer());
        countDownText.text = countDownNumber.ToString();

        if (previousCountDownNumber != countDownNumber)
        {
            previousCountDownNumber = countDownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountDownSound();
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
