using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputActionData : ScriptableObject
{
	/// <summary>
	/// used when you need a Vector3 as input
	/// </summary>
	public Action<Vector3> OnV3Input;

	/// <summary>
	/// used when all you need is a button pressed
	/// </summary>
	public Action OnPress;

}
