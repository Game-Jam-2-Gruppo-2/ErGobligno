using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UINoiseIndicator
{
    [SerializeField] private Image m_NoiseBar;
    [SerializeField] private Image m_FaceImage;
    [SerializeField] private NoiseScore_Settings m_Settings;
    
    public void UpdateIndicator(float percentage, int noiseValue)
    {
        //Update noise Bar
        if (m_NoiseBar.fillAmount != percentage)
            m_NoiseBar.fillAmount = percentage;
        
        //Update color
        if (noiseValue < m_Settings.ColorList.Count)
            m_NoiseBar.color = m_Settings.ColorList[noiseValue];

        //Update Sprite
        if (noiseValue < m_Settings.ExpressionsList.Count)
            m_FaceImage.sprite = m_Settings.ExpressionsList[noiseValue];
    }
}
