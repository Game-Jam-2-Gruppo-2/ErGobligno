using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Noise Score Settings", menuName = "Settings/Other/Noise Score")]
public class NoiseScore_Settings: ScriptableObject
{
    [SerializeField] public List<Sprite> ExpressionsList = new List<Sprite>();
    [SerializeField] public List<Color> ColorList = new List<Color>();
}
