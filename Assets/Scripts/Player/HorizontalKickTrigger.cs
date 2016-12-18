using Assets.Scripts;
using UnityEngine;

public class HorizontalKickTrigger : MonoBehaviour
{
	public float AttackDeadlyRangeStart = 0;
	public float AttackDeadlyRangeEnd = 0.5f;
	public float AttackCooldown = 1f;
	public float AttackVelocityBoost = 5f;
	public float AnimationLength = 0.5f;

	public bool TryAttack = false;

	private Collider2D attackCollider;
	private Unit unit;

	private float attackTimer = 0;

	private void Awake()
	{
		attackCollider = gameObject.GetComponent<Collider2D>();
		attackCollider.enabled = false;
		unit = gameObject.transform.parent.gameObject.GetComponent<Unit>();
	}

	private void Update()
	{
		attackTimer += Time.deltaTime;

		if (TryAttack && (attackTimer == 0 || attackTimer > AttackCooldown))
		{
			unit.UnitState |= Enums.UnitStateEnum.Attacking;

			var velocityBoost = AttackVelocityBoost;

			if ((unit.UnitState.HasFlag(Enums.UnitStateEnum.FacingLeft)))
			{
				velocityBoost = -velocityBoost;
			}

			unit.rigidBody.velocity = new Vector2(unit.rigidBody.velocity.x + velocityBoost, unit.rigidBody.velocity.y);

			attackTimer = 0;
		}

		if (unit.UnitState.HasFlag(Enums.UnitStateEnum.Attacking))
		{
			if (attackTimer < AnimationLength)
			{
				if (attackTimer >= AttackDeadlyRangeStart && attackTimer < AttackDeadlyRangeEnd)
				{
					attackCollider.enabled = true;
				}
				else
				{
					attackCollider.enabled = false;
				}
			}
			else
			{
				unit.UnitState = unit.UnitState.NAND(Enums.UnitStateEnum.Attacking);
				attackCollider.enabled = false;
			}
		}

		TryAttack = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{

	}

	private void OnTriggerExit2D(Collider2D collision)
	{

	}
}
