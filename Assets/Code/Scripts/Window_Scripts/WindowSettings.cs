using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Window Settings", menuName = "Settings/Env/Window")]
public class WindowSettings : ScriptableObject
{
    [SerializeField] private List<WindowData> m_WindowData = new List<WindowData>();

    private int m_CurrentIndex = 0;

    public WindowData GetRandomWindowData()
    {
        float[] probabilityArray = new float[m_WindowData.Count];
        //Load Probability Values
        probabilityArray[0] = m_WindowData[0].SpawnProbability;
        for (int i = 1; i < probabilityArray.Length; i++)
            probabilityArray[i] = m_WindowData[i].SpawnProbability + probabilityArray[i-1];

        //Get Random Value
        float randomValue = UnityEngine.Random.value;
        for (int i = 0; i < probabilityArray.Length; i++)
        {
            if (randomValue < probabilityArray[i])
            {
                m_CurrentIndex = i;
                return m_WindowData[m_CurrentIndex];
            }
        }
        return m_WindowData[m_CurrentIndex];
    }
}
