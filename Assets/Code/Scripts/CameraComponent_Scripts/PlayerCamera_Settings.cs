using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Camera Settings", menuName = "Settings/Other/Camera")]
public class PlayerCamera_Settings : ScriptableObject
{
    [SerializeField] public float SmootTime = 0.5f;
    [SerializeField] public float MaxAngleX = 70f;
}
