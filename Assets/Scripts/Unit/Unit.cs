﻿using System;
using Assets.Scripts;
using UnityEngine;
using System.Collections.Generic;

public abstract class Unit : MonoBehaviour
{
	public int HealthMax = 10;
	public int HealthCurrent = 10;
	public int StaminaMax = 100;
	public int StaminaCurrent = 100;

	public float RunSpeed = 6.1875f;
	public float JumpHeight = 12f;
	public float MoveDampingRate = 0.5f;
	public float FlightDampingRate = 0.1f;
	public Enums.UnitStateEnum UnitState = Enums.UnitStateEnum.Grounded;

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
		UnitState = UnitState.NAND(Enums.UnitStateEnum.Moving);

		// Apply landing
		if (UnitState.HasFlag(Enums.UnitStateEnum.Landing))
		{
			UnitState |= Enums.UnitStateEnum.Grounded;
			UnitState = UnitState.NAND(Enums.UnitStateEnum.Landing);
		}

		// Zero vertical velocity when we're on the ground
		if (UnitState.HasFlag(Enums.UnitStateEnum.Grounded))
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
		}

		TakeAction();

		MotionDamping();

		UpdateUnitState();

		var scale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		if (UnitState.HasFlag(Enums.UnitStateEnum.FacingLeft)) scale.x *= -1;
		transform.localScale = scale;

		UpdateAnimations();
	}

	private void UpdateUnitState()
	{
		if (rigidBody.velocity.y < 0)
		{
			UnitState |= Enums.UnitStateEnum.Falling;
			UnitState = UnitState.NAND(Enums.UnitStateEnum.Jumping);
		}
		else if (rigidBody.velocity.y >= 0)
		{
			UnitState |= Enums.UnitStateEnum.Jumping;
			UnitState = UnitState.NAND(Enums.UnitStateEnum.Falling);
		}

		if (UnitState.HasFlag(Enums.UnitStateEnum.Grounded) && !UnitState.HasFlag(Enums.UnitStateEnum.Falling))
		{
			UnitState = UnitState.NAND(Enums.UnitStateEnum.Jumping);
			UnitState = UnitState.NAND(Enums.UnitStateEnum.DoubleJumping);
		}

		if (rigidBody.velocity.x > 0 && UnitState.HasFlag(Enums.UnitStateEnum.Moving))
		{
			UnitState |= Enums.UnitStateEnum.FacingRight;
			UnitState = UnitState.NAND(Enums.UnitStateEnum.FacingLeft);
		}
		else if (rigidBody.velocity.x < 0 && UnitState.HasFlag(Enums.UnitStateEnum.Moving))
		{
			UnitState |= Enums.UnitStateEnum.FacingLeft;
			UnitState = UnitState.NAND(Enums.UnitStateEnum.FacingRight);
		}
	}

	private void UpdateAnimations()
	{
		//update animator with latest data
		animator.SetBool("Grounded", UnitState.HasFlag(Enums.UnitStateEnum.Grounded));
		animator.SetBool("Running", UnitState.HasFlag(Enums.UnitStateEnum.Moving));
		animator.SetBool("Jumping", UnitState.HasFlag(Enums.UnitStateEnum.Jumping));
		animator.SetBool("Falling", UnitState.HasFlag(Enums.UnitStateEnum.Falling));
		//animator.SetBool("Attacking", UnitState.HasFlag(Enums.UnitStateEnum.Attacking));

		animator.SetBool("LightPunch", false);
		animator.SetBool("LightKick", false);
		animator.SetBool("HeavyPunch", false);

		if (CurrentAttack != null)
		{
			//starts/continues current attack animation
			animator.SetBool(CurrentAttack.Name.ToString(), true);
		}
	}

	private void MotionDamping()
	{
		if (Math.Abs(rigidBody.velocity.x) > 0
			&& !UnitState.HasFlag(Enums.UnitStateEnum.Moving)
			&& UnitState.HasFlag(Enums.UnitStateEnum.Grounded))
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x / MoveDampingRate, rigidBody.velocity.y);
		}
		else if (Math.Abs(rigidBody.velocity.x) > 0
			&& !UnitState.HasFlag(Enums.UnitStateEnum.Moving)
			&& !UnitState.HasFlag(Enums.UnitStateEnum.Grounded))
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x / FlightDampingRate, rigidBody.velocity.y);
		}
	}
}