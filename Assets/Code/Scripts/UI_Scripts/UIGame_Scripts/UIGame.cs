using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIGame : MonoBehaviour
{
    [SerializeField] private TMP_Text TimerText;
    [SerializeField] private TMP_Text CoinText;
    [SerializeField] private TMP_Text MaxCoinText;
    [SerializeField] private UINoiseIndicator NoiseIndicator = new UINoiseIndicator();
    [SerializeField] private UIHands HandsIndicator;

    private void Start()
    {
        UpdateNoise();
        UpdateCoin();
    }

    private void UpdateNoise()
    {
        NoiseIndicator.UpdateIndicator(ScoreManager.GetNoiseValue()/ScoreManager.GetMaxNoiseValue(), (int)ScoreManager.GetNoiseValue());
    }

    private void UpdateCoin()
    {
        CoinText.text = ScoreManager.GetCoinValue().ToString();
        MaxCoinText.text = ScoreManager.GetMaxCoinValue().ToString();
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

    private void AnimateClimb()
    {
        HandsIndicator.StartAnimation();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        ScoreManager.OnGameTimeChanged += UpdateTimer;
        ScoreManager.OnCoinChanged += UpdateCoin;
        ScoreManager.OnNoiseChanged += UpdateNoise;
        MovementController.OnClimb += AnimateClimb;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        ScoreManager.OnGameTimeChanged -= UpdateTimer;
        ScoreManager.OnCoinChanged -= UpdateCoin;
        ScoreManager.OnNoiseChanged -= UpdateNoise;
    }
}
