using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : State
{
	[Header("Jump Settings:")]
	[SerializeField] public float JumpForce;
	private void Awake()
	{
		base.Awake();
	}
	private void OnEnable()
	{
		Rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
	}
	private void OnDisable()
	{

	}
	private void Update()
	{
		if (controller.CheckClimb())
		{
			controller.ChangeState(States.CLIMB);
		}
	}
	private void OnCollisionEnter(Collision other)
	{
		if (other.contacts[0].normal.y > 0)
		{
			controller.ChangeState(States.IDLE);
		}
	}
}
