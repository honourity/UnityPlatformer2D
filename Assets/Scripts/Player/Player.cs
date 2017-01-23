using Assets.Scripts;
using System;
using System.Linq;
using UnityEngine;

public class Player : Unit
{
	public Transform DashPuff;

	private HorizontalAttackTrigger horizontalAttackTrigger;

	void Awake()
	{
		horizontalAttackTrigger = gameObject.GetComponentInChildren<HorizontalAttackTrigger>();
	}

	public override void TakeAction()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (!UnitState.HasFlag(Enums.UnitStateEnun.DoubleJumping))
			{
				UnitState = UnitState.NAND(Enums.UnitStateEnun.Grounded);

				rigidBody.velocity = new Vector2(rigidBody.velocity.x, JumpHeight);

				if (UnitState.HasFlag(Enums.UnitStateEnun.Jumping) || UnitState.HasFlag(Enums.UnitStateEnun.Falling))
				{
					UnitState |= Enums.UnitStateEnun.DoubleJumping;
				}
				else
				{
					UnitState |= Enums.UnitStateEnun.Jumping;
				}
			}
		}
		else if (Input.GetKey(KeyCode.RightArrow) && !(UnitState.HasFlag(Enums.UnitStateEnun.FacingLeft) && UnitState.HasFlag(Enums.UnitStateEnun.Attacking)))
		{
			if (UnitState.HasFlag(Enums.UnitStateEnun.FacingLeft)) SpawnDashPuff();

			UnitState |= Enums.UnitStateEnun.Moving;
			rigidBody.velocity = new Vector2(RunSpeed, rigidBody.velocity.y);
		}
		else if (Input.GetKey(KeyCode.LeftArrow) && !(UnitState.HasFlag(Enums.UnitStateEnun.FacingRight) && UnitState.HasFlag(Enums.UnitStateEnun.Attacking)))
		{
			if (UnitState.HasFlag(Enums.UnitStateEnun.FacingRight)) SpawnDashPuff();

			UnitState |= Enums.UnitStateEnun.Moving;
			rigidBody.velocity = new Vector2(-RunSpeed, rigidBody.velocity.y);
		}

		//if (Input.GetKeyDown(KeyCode.F))
		//{
		//	if (AttackQueue.Count < 3)
		//	{
		//		AttackTypes.FirstOrDefault(attack => attack == CurrentAttack);
		//		AttackQueue.Add();
		//	}

		//	horizontalAttackTrigger.TryAttack = true;
		//}
	}

	private void SpawnDashPuff()
	{
		if (UnitState.HasFlag(Enums.UnitStateEnun.Grounded) && Math.Abs(rigidBody.velocity.x) > 5)
		{
			var puff = (Transform)Instantiate(DashPuff, transform.position, transform.rotation);
			var destructionScript = puff.gameObject.GetComponent<TimedDestruction>();
			destructionScript.FlipDirection = UnitState.HasFlag(Enums.UnitStateEnun.FacingRight);
		}

	}
}

//attacks have types, and also combo orders
