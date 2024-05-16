using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHands : MonoBehaviour
{
    [SerializeField] private Vector2 m_FinalPosition = new Vector2(0f, 500f);
    [SerializeField] private float m_MovementTime = 0.25f;
    [Header("Hand")]
    [SerializeField] private Image m_SpriteRenderer;
    [SerializeField] private List<Sprite> m_Hand_Sprites = new List<Sprite>();

    private RectTransform m_RectTransform;

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    public void StartAnimation()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        //Reset Sprites
        m_SpriteRenderer.sprite = m_Hand_Sprites[0];

        float progress = 0f;
        Vector2 startPos = m_RectTransform.anchoredPosition;

        //Move hands up
        while (progress < 1f)
        {
            m_RectTransform.anchoredPosition = Vector2.Lerp(startPos, startPos + m_FinalPosition, progress);
            progress += Time.deltaTime * (1 / m_MovementTime);
            yield return null;
        }
        m_RectTransform.anchoredPosition = m_FinalPosition;
        //Close hand
        for (int i = 0; i < m_Hand_Sprites.Count; i++)
        {
            m_SpriteRenderer.sprite = m_Hand_Sprites[i];
            yield return new WaitForSeconds(0.02f);
        }
        //Move hands down
        progress = 0f;
        while (progress < 1f)
        {
            m_RectTransform.anchoredPosition = Vector2.Lerp(startPos + m_FinalPosition, startPos, progress);
            progress += Time.deltaTime * (1 / m_MovementTime);
            yield return null;
        }
        m_RectTransform.anchoredPosition = startPos;
    }
}
