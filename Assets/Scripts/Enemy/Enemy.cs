using System;
using Assets.Scripts;
using UnityEngine;

public class Enemy : Unit
{
	public override void TakeAction()
	{
		//UnitState |= Enums.UnitStateEnum.FacingRight;
		//rigidBody.velocity = new Vector2(RunSpeed, rigidBody.velocity.y);
		//UnitState |= Enums.UnitStateEnum.Moving;
	}

	internal void Attacked(int incomingDamage)
	{
		HealthCurrent -= incomingDamage;

		if (HealthCurrent <= 0)
		{
			Destroy(gameObject);
		}
	}
}
