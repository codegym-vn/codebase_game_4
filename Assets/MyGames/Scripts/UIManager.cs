using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[AddComponentMenu("DangSon/UIManager")]
public class UIManager : MonoBehaviour
{

    [Header("Coin UI")]
    public TextMeshProUGUI coinText;
    public float updateDuration = 0.1f;

    public Slider slider;
    private void Start()
    {
        Invoke(nameof(StartDelay),updateDuration);
    } 
    void StartDelay()
    {
            //UpdateCoin(GameManager.Instance.GetCoin());
            UpdateCoin(CoinManager.Instance.LoadCoins());
            GameEvent.eventCoinsCompleted.AddListener(UpdateCoin);
            GameEvent.eventHealth.AddListener(UpdateHealth);
    }
    private void UpdateCoin(int coin)
    {
        coinText.text = coin.ToString(); 
    }
    private void UpdateHealth(int health)
    {
       slider.value = health;
    }
}
