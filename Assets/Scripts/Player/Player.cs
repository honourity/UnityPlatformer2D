using Assets.Scripts;
using UnityEngine;

public class Player : Unit
{
	private HorizontalAttackTrigger horizontalAttackTrigger;

	void Awake()
	{
		horizontalAttackTrigger = gameObject.GetComponentInChildren<HorizontalAttackTrigger>();
	}

	public override void TakeAction()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
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
		else if (Input.GetKey(KeyCode.RightArrow) && !(UnitState.HasFlag(Enums.UnitStateEnum.FacingLeft) && UnitState.HasFlag(Enums.UnitStateEnum.Attacking)))
		{
			UnitState |= Enums.UnitStateEnum.Moving;
			rigidBody.velocity = new Vector2(RunSpeed, rigidBody.velocity.y);
		}
		else if (Input.GetKey(KeyCode.LeftArrow) && !(UnitState.HasFlag(Enums.UnitStateEnum.FacingRight) && UnitState.HasFlag(Enums.UnitStateEnum.Attacking)))
		{
			UnitState |= Enums.UnitStateEnum.Moving;
			rigidBody.velocity = new Vector2(-RunSpeed, rigidBody.velocity.y);
		}

		if (Input.GetKeyDown(KeyCode.F))
		{
			horizontalAttackTrigger.TryAttack = true;
		}
	}
}