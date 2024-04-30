using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordData
{
    public float GameTime = 0f;
    public int CoinAmount = 0;
    
    public RecordData()
    {

    }

    /// <summary>
    /// Data of the record
    /// </summary>
    /// <param name="gameTime">Duration of game</param>
    /// <param name="coinAmount">Amount of coins collected during game</param>
    public RecordData(float gameTime, int coinAmount)
    {
        GameTime = gameTime;
        CoinAmount = coinAmount;
    }
}
