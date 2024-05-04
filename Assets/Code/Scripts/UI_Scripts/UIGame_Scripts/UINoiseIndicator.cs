using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UINoiseIndicator
{
    [SerializeField] private Image m_NoiseBar;
    [SerializeField] private Image m_FaceImage;
    [SerializeField] private NoiseScore_Settings m_Settings;

    private int m_Index = 0;

    public void SetNoiseBarValue(float value)
    {
        if (m_NoiseBar.fillAmount != value)
        {
            m_NoiseBar.fillAmount = value;
            //Update color
            if (m_Index < m_Settings.ColorList.Count)
                m_NoiseBar.color = m_Settings.ColorList[m_Index];
            //Update Sprite
            if (m_Index < m_Settings.ExpressionsList.Count)
                m_FaceImage.sprite = m_Settings.ExpressionsList[m_Index];
        }
        Debug.Log(m_Index);
    }

    public void SetIndexValue(int value)
    {
        m_Index = value;
    }
}
