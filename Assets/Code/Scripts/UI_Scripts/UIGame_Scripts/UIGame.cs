using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIGame : MonoBehaviour
{
    [SerializeField] private TMP_Text TimerText;
    [SerializeField] private TMP_Text CoinText;
    [SerializeField] private UINoiseIndicator NoiseIndicator = new UINoiseIndicator();

    private void UpdateNoise()
    {
        NoiseIndicator.SetNoiseBarValue(ScoreManager.GetNoiseValue()/ScoreManager.GetMaxNoiseValue());
        NoiseIndicator.SetIndexValue((int)ScoreManager.GetNoiseValue());
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
        Cursor.lockState = CursorLockMode.Locked;
        ScoreManager.OnGameTimeChanged += UpdateTimer;
        ScoreManager.OnCoinChanged += UpdateCoin;
        ScoreManager.OnNoiseChanged += UpdateNoise;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        ScoreManager.OnGameTimeChanged -= UpdateTimer;
        ScoreManager.OnCoinChanged -= UpdateCoin;
        ScoreManager.OnNoiseChanged -= UpdateNoise;
    }
}
