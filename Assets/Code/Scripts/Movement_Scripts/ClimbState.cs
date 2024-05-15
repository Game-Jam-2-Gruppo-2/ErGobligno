using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClimbState : State
{

	[Header("Climb Settings:")]
	[SerializeField] public float ClimbDuration;
	[SerializeField] public float CooldownAfterEnd;
	///<summary>the offset in y after you climed</summary>
	[Tooltip("the offset in y after you climed")]
	[SerializeField]
	public float ClimbOffsetY, ClimbOffsetZ;
	Vector3 startpos;
	Vector3 endPos = new();
	Vector3 lerpedPos;
	float timer;
	Collider HitCollider;
	private void Awake()
	{
		base.Awake();
	}
	private void OnEnable()
	{
		MyCollider.enabled = false;
		Rb.useGravity = false;

		if (controller.HitObject.IsUnityNull())
			return;

		HitCollider = controller.HitObject.collider;
		startpos = transform.position;
		endPos = startpos;
		endPos.y = HitCollider.bounds.max.y + MyCollider.bounds.extents.y + ClimbOffsetY; // + controller.ClimbOffset
		endPos += transform.forward * ClimbOffsetZ;
	}
	private void OnDisable()
	{
		//nada
	}
	void Update()
	{
		if (timer < ClimbDuration)
		{
			timer += Time.deltaTime;
			lerpedPos = Vector3.Lerp(startpos, endPos, timer / ClimbDuration);
			transform.position = lerpedPos;
		}
		else
		{
			transform.position = endPos;
			controller.ChangeState(States.IDLE);
		}
	}
}
