using Assets.Scripts;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
	public float AttackDeadlyRangeStart = 0;
	public float AttackDeadlyRangeEnd = 0.5f;
	public float AttackCooldown = 1f;
	public float AttackVelocityBoost = 5f;
	public float AnimationLength = 0.5f;
	public Player player;

	private Collider2D attackCollider;
	private float attackTimer = 0;

	private void Awake()
	{
		attackCollider = gameObject.GetComponent<Collider2D>();
		attackCollider.enabled = false;
	}

	private void Update()
	{
		attackTimer += Time.deltaTime;

		if (Input.GetKey(KeyCode.F) && (attackTimer == 0 || attackTimer > AttackCooldown))
		{
			player.UnitState |= Enums.UnitStateEnum.Attacking;

			var velocityBoost = AttackVelocityBoost;

			if ((player.UnitState.HasFlag(Enums.UnitStateEnum.FacingLeft)))
			{
				velocityBoost = -velocityBoost;
			}

			player.rigidBody.velocity = new Vector2(player.rigidBody.velocity.x + velocityBoost, player.rigidBody.velocity.y);

			attackTimer = 0;
		}

		if (player.UnitState.HasFlag(Enums.UnitStateEnum.Attacking))
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
				player.UnitState = player.UnitState.NAND(Enums.UnitStateEnum.Attacking);
				attackCollider.enabled = false;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{

	}

	private void OnTriggerExit2D(Collider2D collision)
	{

	}
}
