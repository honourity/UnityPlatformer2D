using Assets.Scripts;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
	public float AttackDeadlyRangeStart = 0;
	public float AttackDeadlyRangeEnd = 0.5f;
	public float AttackCooldown = 1.0f;
	public float AnimationLength = 0.5f;

	private Collider2D attackCollider;
	private float attackTimer = 0;
	public Player player;

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
