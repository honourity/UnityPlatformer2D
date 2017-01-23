using System;
using Assets.Scripts;
using UnityEngine;
using System.Collections.Generic;

public abstract class Unit : MonoBehaviour
{
	public int HealthMax = 10;
	public int HealthCurrent = 10;

	public float RunSpeed = 6.1875f;
	public float JumpHeight = 12f;
	public float MoveDampingRate = 0.5f;
	public float FlightDampingRate = 0.1f;
	public Enums.UnitStateEnun UnitState = Enums.UnitStateEnun.Grounded;

	public IList<Attack> AttackTypes = new List<Attack>();
	public IList<Attack> AttackQueue = new List<Attack>();
	public Attack CurrentAttack;

	internal Rigidbody2D rigidBody;
	internal Animator animator;

	//placeholder for checking inputs or running AI decisions go here as an override
	public abstract void TakeAction();

	// Use this for initialization
	void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		MoveDampingRate += 1;
		FlightDampingRate += 1;
	}

	// Update is called once per frame
	void Update()
	{
		UnitState = UnitState.NAND(Enums.UnitStateEnun.Moving);

		// Apply landing
		if (UnitState.HasFlag(Enums.UnitStateEnun.Landing))
		{
			UnitState |= Enums.UnitStateEnun.Grounded;
			UnitState = UnitState.NAND(Enums.UnitStateEnun.Landing);
		}

		// Zero vertical velocity when we're on the ground
		if (UnitState.HasFlag(Enums.UnitStateEnun.Grounded))
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
		}

		TakeAction();

		MotionDamping();

		if (rigidBody.velocity.y < 0)
		{
			UnitState |= Enums.UnitStateEnun.Falling;
			UnitState = UnitState.NAND(Enums.UnitStateEnun.Jumping);
		}
		else if (rigidBody.velocity.y >= 0)
		{
			UnitState |= Enums.UnitStateEnun.Jumping;
			UnitState = UnitState.NAND(Enums.UnitStateEnun.Falling);
		}

		if (UnitState.HasFlag(Enums.UnitStateEnun.Grounded) && !UnitState.HasFlag(Enums.UnitStateEnun.Falling))
		{
			UnitState = UnitState.NAND(Enums.UnitStateEnun.Jumping);
			UnitState = UnitState.NAND(Enums.UnitStateEnun.DoubleJumping);
		}

		if (rigidBody.velocity.x > 0 && UnitState.HasFlag(Enums.UnitStateEnun.Moving))
		{
			UnitState |= Enums.UnitStateEnun.FacingRight;
			UnitState = UnitState.NAND(Enums.UnitStateEnun.FacingLeft);
		}
		else if (rigidBody.velocity.x < 0 && UnitState.HasFlag(Enums.UnitStateEnun.Moving))
		{
			UnitState |= Enums.UnitStateEnun.FacingLeft;
			UnitState = UnitState.NAND(Enums.UnitStateEnun.FacingRight);
		}

		var scale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		if (UnitState.HasFlag(Enums.UnitStateEnun.FacingLeft)) scale.x *= -1;
		transform.localScale = scale;

		//update animator with latest data
		animator.SetBool("Grounded", UnitState.HasFlag(Enums.UnitStateEnun.Grounded));
		animator.SetBool("Running", UnitState.HasFlag(Enums.UnitStateEnun.Moving));
		animator.SetBool("Jumping", UnitState.HasFlag(Enums.UnitStateEnun.Jumping));
		animator.SetBool("Falling", UnitState.HasFlag(Enums.UnitStateEnun.Falling));
		animator.SetBool("Attacking", UnitState.HasFlag(Enums.UnitStateEnun.Attacking));


		////hopefully will catch null reference exception in the case Enum doesnt match CurrentAttack property
		//if (UnitState.HasFlag(Enums.UnitStateEnun.Attacking))
		//{
		//	//starts current attack animation
		//	animator.SetBool(CurrentAttack.AttackAnimationKey, true);
		//}
		//else
		//{
		//	//stops all attack animations
		//	foreach(var attack in AttackTypes)
		//	{
		//		animator.SetBool(attack.AttackAnimationKey, false);
		//	}
		//}		
	}

	private void MotionDamping()
	{
		if (Math.Abs(rigidBody.velocity.x) > 0
			&& !UnitState.HasFlag(Enums.UnitStateEnun.Moving)
			&& UnitState.HasFlag(Enums.UnitStateEnun.Grounded))
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x / MoveDampingRate, rigidBody.velocity.y);
		}
		else if (Math.Abs(rigidBody.velocity.x) > 0
			&& !UnitState.HasFlag(Enums.UnitStateEnun.Moving)
			&& !UnitState.HasFlag(Enums.UnitStateEnun.Grounded))
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x / FlightDampingRate, rigidBody.velocity.y);
		}
	}
}