using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioManager), typeof(VFXManager))]
public class GameManager : MonoBehaviour
{
    //Singeton
    [HideInInspector] public static GameManager Instance;

    /// <summary>
    /// Game states
    /// </summary>
    private enum GameState
    {

    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("MULTIPLE DIRECTORS FOUND");
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
