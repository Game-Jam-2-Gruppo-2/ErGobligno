using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
	[SerializeField] public Rigidbody Rb;
	[SerializeField] public float MaxSpeed, AccelerationSpeed, DecelerationSpeed;
	[SerializeField] public float MaxJumpHight;
	[SerializeField] public LayerMask ClimableLayers;
	[SerializeField] public AnimationCurve AccelerationCurve; // how fast you XLR8

	[Tooltip("how you stop moving based on decelerationSpeed")]
	[SerializeField] public AnimationCurve DecelerationCurve;

	[HideInInspector] public float MomentumCounter; // whare you are inside one of the curves
	[SerializeField] public MovementStates CurrentState;


	void Start()
	{
		Rb = GetComponent<Rigidbody>();
		CurrentState = new WalkState();
	}

	void Update()
	{
		CurrentState.Tick(this);
	}

	public void ChangeState(MovementStates newState)
	{
		CurrentState = newState;
	}
}