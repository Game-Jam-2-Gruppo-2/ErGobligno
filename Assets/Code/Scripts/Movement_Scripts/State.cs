using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
	[HideInInspector] protected Rigidbody Rb;
	[HideInInspector] protected Collider MyCollider;
	[HideInInspector] protected MovementController controller;

	protected virtual void Awake()
	{
		Rb = GetComponent<Rigidbody>();
		MyCollider = GetComponent<Collider>();
		controller = GetComponent<MovementController>();
	}

}

