using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager: MonoBehaviour
{
    private static ScoreManager Instance;

    [SerializeField] private ScoreManager_Settings m_Settings;

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
    //Saved Scores
    private static List<RecordData> Records;
    private static List<float> TimeRecords;
    private static List<float> CoinRecords;

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

    public static float GetGameTimeValue()
    {
        return GameTime;
    }

    //public static List<float> GetRecords()
    //{

    //}

    private void OnEnable()
    {
        GameManager.OnNewGame += StartGame;
    }

    private void OnDisable()
    {
        GameManager.OnNewGame -= StartGame;
        OnCoinChanged -= OnCoinChanged;
        OnNoiseChanged -= OnNoiseChanged;
        OnGameTimeChanged -= OnGameTimeChanged;
    }
}
