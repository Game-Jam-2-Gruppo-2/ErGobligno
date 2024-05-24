using System.Collections;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    [SerializeField] private WindowSettings m_Settings;
    [Header("Transform Refs")]
    [SerializeField] private Transform m_SilhouetteTransform;
    [SerializeField] private Transform m_StartPos;
    [SerializeField] private Transform m_EndPos;
    [Header("Renderer Refs")]
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private SpriteRenderer m_SpriteRendererShadowCaster;

    private void Awake()
    {
        m_SilhouetteTransform.position = m_StartPos.position;
        StartCoroutine(MoveSilhouette());
    }

    private IEnumerator MoveSilhouette()
    {
        //Window values
        WindowData newData = m_Settings.GetRandomWindowData();
        //Update silhouette
        m_SpriteRenderer.flipX = true;
        m_SpriteRendererShadowCaster.flipX = true;
        m_SpriteRenderer.sprite = newData.SilhouetteSprite;
        m_SpriteRendererShadowCaster.sprite = m_SpriteRenderer.sprite;
        //Animation values
        float progress = 0f;
        Vector3 startPos = m_StartPos.position;
        Vector3 endPos = m_EndPos.position;

        if(UnityEngine.Random.value > 0.5f) //Invert Start and End
        {
            Vector3 x;
            x = startPos;
            startPos = endPos;
            endPos = x;
            m_SpriteRenderer.flipX = false;
            m_SpriteRendererShadowCaster.flipX = false;
        }
        //Move sprite
        while(progress<1f)
        {
            m_SilhouetteTransform.position = Vector3.Lerp(startPos, endPos, progress);
            progress += Time.deltaTime * newData.Speed;
            yield return null;
        }
        //Loop
        yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
        StartCoroutine(MoveSilhouette());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if(m_StartPos!=null)
            Gizmos.DrawSphere(m_StartPos.position, 0.5f);
        if (m_StartPos != null)
            Gizmos.DrawSphere(m_EndPos.position, 0.5f);
    }
}

