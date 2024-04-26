using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioManager))]
public class Director : MonoBehaviour
{
    //Singeton
    [HideInInspector] public static Director Instance;

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
    }
}
