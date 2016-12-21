using Assets.Scripts;
using System;
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
            if (UnitState.HasFlag(Enums.UnitStateEnum.FacingLeft)) SpawnDashPuff();

            UnitState |= Enums.UnitStateEnum.Moving;
            rigidBody.velocity = new Vector2(RunSpeed, rigidBody.velocity.y);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !(UnitState.HasFlag(Enums.UnitStateEnum.FacingRight) && UnitState.HasFlag(Enums.UnitStateEnum.Attacking)))
        {
            if (UnitState.HasFlag(Enums.UnitStateEnum.FacingRight)) SpawnDashPuff();

            UnitState |= Enums.UnitStateEnum.Moving;
            rigidBody.velocity = new Vector2(-RunSpeed, rigidBody.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            horizontalAttackTrigger.TryAttack = true;
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