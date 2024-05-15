using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
	[HideInInspector] public Rigidbody Rb;
	[HideInInspector] public Collider MyCollider;
	[HideInInspector] public MovementController controller;

	protected void Awake()
	{
		this.Rb = GetComponent<Rigidbody>();
		this.MyCollider = GetComponent<Collider>();
		this.controller = GetComponent<MovementController>();
	}

}

