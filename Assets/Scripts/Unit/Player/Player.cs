using Assets.Scripts;
using System;
using System.Linq;
using UnityEngine;

public class Player : Unit
{
	public Transform DashPuff;

	private AttackTrigger attackTrigger;

	void Awake()
	{
		attackTrigger = gameObject.GetComponentInChildren<AttackTrigger>();
	}

	public override void TakeAction()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			if (!UnitState.HasFlag(Enums.UnitStateEnum.DoubleJumping))
			{
				UnitState = UnitState.NAND(Enums.UnitStateEnum.Grounded);

				rigidBody.velocity = new Vector2(rigidBody.velocity.x, JumpHeight);

				if (UnitState.HasFlag(Enums.UnitStateEnum.Jumping) || UnitState.HasFlag(Enums.UnitStateEnum.Falling))
				{
					UnitState |= Enums.UnitStateEnum.DoubleJumping;
				}
				else
				{
					UnitState |= Enums.UnitStateEnum.Jumping;
				}
			}
		}
		else if (Input.GetKey(KeyCode.D) && !(UnitState.HasFlag(Enums.UnitStateEnum.FacingLeft) && CurrentAttack != null))
		{
			if (UnitState.HasFlag(Enums.UnitStateEnum.FacingLeft)) SpawnDashPuff();

			UnitState |= Enums.UnitStateEnum.Moving;
			rigidBody.velocity = new Vector2(RunSpeed, rigidBody.velocity.y);

			attackTrigger.ClearAttackQueue();
		}
		else if (Input.GetKey(KeyCode.A) && !(UnitState.HasFlag(Enums.UnitStateEnum.FacingRight) && CurrentAttack != null))
		{
			if (UnitState.HasFlag(Enums.UnitStateEnum.FacingRight)) SpawnDashPuff();

			UnitState |= Enums.UnitStateEnum.Moving;
			rigidBody.velocity = new Vector2(-RunSpeed, rigidBody.velocity.y);

			attackTrigger.ClearAttackQueue();
		}

		if (Input.GetKeyDown((KeyCode)Enums.AttackKeyCode.LightAttack))
		{
			//when animation is finished, but cooldown is still active,
			// we dont want to try a root attack
			if (CurrentAttack == null && AttackQueue.Count == 0 && !attackTrigger.PreviousAttackInCooldown())
			{
				var attack = AttackGraph.RootAttacks.FirstOrDefault(a => a.Type == Enums.AttackKeyCode.LightAttack);
				if (attack != null)
				{
					attackTrigger.TryAttack = attack;
				}
			}
			else if (CurrentAttack != null)
			{
				var attack = CurrentAttack.FollowupAttacks.FirstOrDefault(a => a.Type == Enums.AttackKeyCode.LightAttack);
				if (attack != null)
				{
					attackTrigger.TryAttack = attack;
				}
			}
		}
	}

	private void SpawnDashPuff()
	{
		if (UnitState.HasFlag(Enums.UnitStateEnum.Grounded) && Math.Abs(rigidBody.velocity.x) > 5)
		{
			var puff = (Transform)Instantiate(DashPuff, transform.position, transform.rotation);
			var destructionScript = puff.gameObject.GetComponent<TimedDestruction>();
			destructionScript.FlipDirection = UnitState.HasFlag(Enums.UnitStateEnum.FacingRight);
		}

	}
}
