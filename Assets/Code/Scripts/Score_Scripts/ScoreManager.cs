using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager: MonoBehaviour
{
    public static ScoreManager Instance;

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
    private static int CoinLeft;
    private static int MaxCoin;
    private static float NoiseAmount;
    private static float MaxNoise;

    //Saved Scores
    private static List<RecordData> Records;

    //Coroutine
    private Coroutine TimerCoroutine;
    private Stack<IEnumerator> DecreaseTimerStack;

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

        MaxNoise = m_Settings.MaxNoise;
        TimerCoroutine = null;
        DecreaseTimerStack = new Stack<IEnumerator>();
    }

    private void ResetValues()
    {
        //Reset Values
        GameTime = m_Settings.MaxTime;
        CoinAmount = 0;
        NoiseAmount = 0f;
    }

    private IEnumerator TimerEnumerator()
    {
        while (true)
        {
            GameTime -= Time.deltaTime;
            OnGameTimeChanged?.Invoke();
            yield return null;
        }
    }

    private void DecreaseNoiseProcedure()
    {

        if (NoiseAmount >= MaxNoise)
            return;
        if (NoiseAmount <= 0)
        {
            NoiseAmount = 0;
            return;
        }

        //Stop First Coroutine
        if(DecreaseTimerStack.Count > 0)
            StopCoroutine(DecreaseTimerStack.Peek());
        //Add a elemt inside the Stack
        DecreaseTimerStack.Push(DecreaseNoise(m_Settings.DecreaseTime[(int)NoiseAmount - 1]));
        //Start Stack coroutine
        StartCoroutine(DecreaseTimerStack.Peek());
    }

    private IEnumerator DecreaseNoise(float time)
    {
        //Wait and decrease
        yield return new WaitForSeconds(time);
        NoiseAmount -= 1;
        OnNoiseChanged?.Invoke();
        
        //Remove Current Coroutine
        DecreaseTimerStack.Pop();

        //Start Following element of Stack
        DecreaseNoiseProcedure();
    }

    private void IncreaseNoiseAmount(float amount)
    {
        NoiseAmount += amount;
        OnNoiseChanged?.Invoke();
        DecreaseNoiseProcedure();
    }

    private void IncreaseCoinScore(int amount)
    {
        CoinAmount += amount;
        CoinLeft--;
        OnCoinChanged?.Invoke();
    }

    private void StartGame()
    {
        ResetValues();
        TimerCoroutine = StartCoroutine(TimerEnumerator());
        MaxCoin = FindObjectsOfType<Collectible>().Length;
        CoinLeft = MaxCoin;
        //Call Update Value Events
        OnGameTimeChanged?.Invoke();
        OnCoinChanged?.Invoke();
        OnNoiseChanged?.Invoke();
    }

    private void EndGame()
    {
        //TODO: Save High Score
        ResetValues();
        OnGameTimeChanged?.Invoke();
        OnCoinChanged?.Invoke();
        OnNoiseChanged?.Invoke();
    }

    //Public static getter
    public static float GetCoinValue(){ return CoinAmount; }

    public static int GetMaxCoinValue(){ return MaxCoin; }

    public static int GetCoinLeft(){ return CoinLeft; }

    public static float GetNoiseValue(){ return NoiseAmount; }

    public static float GetMaxNoiseValue(){ return MaxNoise; }

    public static float GetGameTimeValue(){ return GameTime; }

    public static bool IsNoiseOnMax(){ return NoiseAmount > MaxNoise; }

    public List<RecordData> GetRecords()
    {
        //Load All Records
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
        return Records;
    }

    private void OnEnable()
    {
        GameManager.OnNewGame += StartGame;
        IncreaseNoise += IncreaseNoiseAmount;
        IncreaseCoin += IncreaseCoinScore;
    }

    private void OnDisable()
    {
        GameManager.OnNewGame -= StartGame;
        IncreaseNoise -= IncreaseNoiseAmount;
        IncreaseCoin -= IncreaseCoinScore;
        OnCoinChanged -= OnCoinChanged;
        OnNoiseChanged -= OnNoiseChanged;
        OnGameTimeChanged -= OnGameTimeChanged;
    }
}
