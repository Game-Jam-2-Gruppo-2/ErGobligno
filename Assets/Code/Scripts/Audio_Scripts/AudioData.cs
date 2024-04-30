using UnityEngine;

[System.Serializable]
public class AudioData
{    
    [SerializeField] public AudioChannel Type;
    [SerializeField][Range(0f, 1f)] public float DefaultVolume = 0.75f;
}
