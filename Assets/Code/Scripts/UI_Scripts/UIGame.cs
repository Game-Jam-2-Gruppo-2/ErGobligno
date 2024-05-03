using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGame : MonoBehaviour
{
    [SerializeField] private TMP_Text NoiseText;
    [SerializeField] private TMP_Text TimerText;
    [SerializeField] private TMP_Text CoinText;

    private void UpdateNoise()
    {
        NoiseText.text = ScoreManager.GetNoiseValue()+" db";
    }

    private void UpdateCoin()
    {
        CoinText.text = ScoreManager.GetCoinValue()+" €";
    }

    private void UpdateTimer()
    {
        float sec = ScoreManager.GetGameTimeValue();
        int min = (int)(sec / 60f);
        if (sec >= 60)
            sec -= min * 60;
        string result = string.Format("{0:N}", sec);
        TimerText.text = min+" : "+result;
    }

    private void OnEnable()
    {
        ScoreManager.OnGameTimeChanged += UpdateTimer;
        ScoreManager.OnCoinChanged += UpdateCoin;
        ScoreManager.OnNoiseChanged += UpdateNoise;
    }

    private void OnDisable()
    {
        ScoreManager.OnGameTimeChanged -= UpdateTimer;
        ScoreManager.OnCoinChanged -= UpdateCoin;
        ScoreManager.OnNoiseChanged -= UpdateNoise;
    }
}
