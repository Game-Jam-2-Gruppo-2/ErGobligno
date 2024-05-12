using UnityEngine;

[System.Serializable]
public class WindowData
{
    [SerializeField] public Sprite SilhouetteSprite;
    [SerializeField] public float Speed = 1f;
    [SerializeField] public float Hz = 5f;
    [SerializeField, Range(0f, 1f)] public float SpawnProbability = 0.1f;
}
