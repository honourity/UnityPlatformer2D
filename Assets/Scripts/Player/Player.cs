using Assets.Scripts;
using UnityEngine;

public class Player : Unit
{
	public override void TakeAction()
	{
		// Check input and take actions
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
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			UnitState |= Enums.UnitStateEnum.Moving;
			rigidBody.velocity = new Vector2(RunSpeed, rigidBody.velocity.y);
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			UnitState |= Enums.UnitStateEnum.Moving;
			rigidBody.velocity = new Vector2(-RunSpeed, rigidBody.velocity.y);
		}

		if (Input.GetKey(KeyCode.F))
		{
			gameObject.GetComponentInChildren<HorizontalKickTrigger>().TryAttack = true;
		}
	}
}