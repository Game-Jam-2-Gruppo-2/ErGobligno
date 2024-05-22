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
        m_SpriteRenderer.sprite = newData.SilhouetteSprite;
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
        }
        //Move sprite
        while(progress<1f)
        {
            m_SilhouetteTransform.position = Vector3.Lerp(startPos, endPos, progress);
            //float y = (float)(0.5 * Mathf.Sin(Time.time * newData.Hz) + 0.5);
            //if (y > 0)
            //    m_SilhouetteTransform.position = new Vector3(m_SilhouetteTransform.position.x, m_SilhouetteTransform.position.y + y, m_SilhouetteTransform.position.z);
            progress += Time.deltaTime * newData.Speed;
            yield return null;
        }
        //Loop
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

