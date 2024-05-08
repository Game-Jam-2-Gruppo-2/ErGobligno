using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [Header("Bottles")]
    [SerializeField] private bool Highlight_Bottle = false;
    [SerializeField] private float GizsmoScale_Bottle = 1.25f;
    [SerializeField] private Color HighlightColor_Bottle = Color.white;
    [SerializeField] private Bottle[] Bottles;

    [Header("Climbable")]
    [SerializeField] private bool Highlight_Climbable = false;
    [SerializeField] private float GizsmoScale_Climbable = 1.25f;
    [SerializeField] private Color HighlightColor_Climbable = Color.white;
    [SerializeField] private List<IClimbable> Climbable;

    private void OnValidate()
    {
        if(Highlight_Bottle)
            Bottles = FindObjectsOfType<Bottle>();

        if (Highlight_Climbable)
        {
            Climbable = FindObjectsOfType<IClimbable>().ToList();
        }
    }

    private void OnDrawGizmos()
    {
        if (Highlight_Bottle)
        {
            Gizmos.color = HighlightColor_Bottle;
            for (int i = 0; i < Bottles.Length; i++)
                Gizmos.DrawCube(Bottles[i].gameObject.transform.position, Bottles[i].gameObject.GetComponent<Collider>().bounds.extents * 2 * GizsmoScale_Climbable);
        }

        if (Highlight_Climbable)
        {
            Gizmos.color = HighlightColor_Climbable;
            for (int i = 0; i < Climbable.Count; i++)
                Gizmos.DrawCube(Climbable[i].gameObject.transform.position, Climbable[i].gameObject.GetComponent<Collider>().bounds.extents * 2 * GizsmoScale_Climbable);
        }
    }
}
