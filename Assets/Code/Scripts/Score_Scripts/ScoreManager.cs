using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager: MonoBehaviour
{
    private static ScoreManager Instance;

    [SerializeField] private ScoreManager_Settings m_Settings;

    //Noise Action
    public static Action<float> IncreaseNoise;
    public static Action<int> IncreaseCoin;

    //Value Changed Event
    public delegate void ValueChanged();
    public static event ValueChanged OnCoinChanged;
    public static event ValueChanged OnNoiseChanged;
    public static event ValueChanged OnGameTimeChanged;

    public delegate void NewRecord();
    public static event NewRecord OnNewRecord;

    //Static variables
    private static float GameTime;
    private static int CoinAmount;
    private static float NoiseAmount;
    private static float MaxNoise;

    //Saved Scores
    private static List<RecordData> Records;

    //Coroutine
    private Coroutine TimerCoroutine;

    private void Awake()
    {
        //Singleton set up
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("MULTIPLE SCORE MANAGER FOUND");
            Destroy(this.gameObject);
            return;
        }

        ResetValues();

        ////Load All Records
        Records = new List<RecordData>();
        for (int i = 0; i < m_Settings.SavedScoreCount; i++)
        {
            RecordData record = new RecordData(0f, 0);

            //Time Records
            if (PlayerPrefs.HasKey("TimeRecord_" + i))
                record.GameTime = PlayerPrefs.GetFloat("TimeRecord_" + i);
            else
            {
                record.GameTime = 0;
                PlayerPrefs.SetFloat("TimeRecord_" + i, 0f);
            }
            //Coin Record
            if (PlayerPrefs.HasKey("ScoreRecord_" + i))
                record.CoinAmount = (int)PlayerPrefs.GetFloat("ScoreRecord_" + i);
            else
            {
                record.CoinAmount = 0;
                PlayerPrefs.SetFloat("ScoreRecord_" + i, 0f);
            }

            Records.Add(record);
        }

        MaxNoise = m_Settings.MaxNoise;
        TimerCoroutine = null;
    }

    private void ResetValues()
    {
        //Reset Values
        GameTime = 0f;
        CoinAmount = 0;
        NoiseAmount = 0f;
        //Call Update Value Events
        OnGameTimeChanged?.Invoke();
        OnCoinChanged?.Invoke();
        OnNoiseChanged?.Invoke();
    }

    /// <summary>
    /// Play time Enumerator
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimerEnumerator()
    {
        while (true)
        {
            GameTime += Time.deltaTime;
            OnGameTimeChanged?.Invoke();
            yield return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    private void IncreaseNoiseAmount(float amount)
    {
        NoiseAmount += amount;
        OnNoiseChanged?.Invoke();
    }

    private void IncreaseCoinAmount(int amount)
    {
        CoinAmount += amount;
        OnCoinChanged?.Invoke();
    }

    private void StartGame()
    {
        ResetValues();
        TimerCoroutine = StartCoroutine(TimerEnumerator());
    }

    private void EndGame()
    {
        //TODO: Save High Score
        ResetValues();
    }

    //Public static getter
    public static float GetCoinValue()
    {
        return CoinAmount;
    }

    public static float GetNoiseValue()
    {
        return NoiseAmount;
    }

    public static float GetMaxNoiseValue()
    {
        return MaxNoise;
    }

    public static float GetGameTimeValue()
    {
        return GameTime;
    }

    public static bool IsNoiseOnMax()
    {
        return NoiseAmount > MaxNoise;
    }

    //public static List<float> GetRecords()
    //{

    //}

    private void OnEnable()
    {
        GameManager.OnNewGame += StartGame;
        IncreaseNoise += IncreaseNoiseAmount;
        IncreaseCoin += IncreaseCoinAmount;
    }

    private void OnDisable()
    {
        GameManager.OnNewGame -= StartGame;
        IncreaseNoise -= IncreaseNoiseAmount;
        IncreaseCoin -= IncreaseCoinAmount;
        OnCoinChanged -= OnCoinChanged;
        OnNoiseChanged -= OnNoiseChanged;
        OnGameTimeChanged -= OnGameTimeChanged;
    }
}
