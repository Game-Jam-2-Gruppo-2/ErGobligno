using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGame : MonoBehaviour
{
    [SerializeField] private TMP_Text TimerText;
    [SerializeField] private TMP_Text CoinText;

    private void UpdateCoin()
    {
        CoinText.text = ScoreManager.GetCoinValue() + " ";
    }

    private void UpdateTimer()
    {
        TimerText.text = ScoreManager.GetGameTimeValue() + " s";
    }

    private void OnEnable()
    {
        ScoreManager.OnGameTimeChanged += UpdateTimer;
        ScoreManager.OnCoinChanged += UpdateCoin;
    }

    private void OnDisable()
    {
        ScoreManager.OnGameTimeChanged -= UpdateTimer;
        ScoreManager.OnCoinChanged -= UpdateCoin;
    }
}
