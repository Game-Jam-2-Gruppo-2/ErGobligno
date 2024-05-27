using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    [SerializeField] private List<Canvas> m_Canvases = new List<Canvas>();
    [SerializeField] private string m_SceneToLoad = "none";
    private int currentIndex;

    private void Awake()
    {
        currentIndex = 0;
        for(int i=1; i<m_Canvases.Count; i++)
            m_Canvases[i].gameObject.SetActive(false);
    }

    public void NextTab()
    {
        m_Canvases[currentIndex].gameObject.SetActive(false);
        currentIndex++;
        if (currentIndex > m_Canvases.Count)
            SceneManager.LoadScene(m_SceneToLoad);
        m_Canvases[currentIndex].gameObject.SetActive(true);
    }
}
