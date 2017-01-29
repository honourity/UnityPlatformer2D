using System;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using System.Collections.Generic;

public class AttackTrigger : MonoBehaviour
{
	public int AttackQueueLength = 1;
	public Attack TryAttack;

	private Unit unit;
	private AudioSource audioSource;
	private List<Collider> colliders;
	private float attackTimer;
	private float previousAttackCooldown;

	private void Awake()
	{
		unit = gameObject.transform.parent.gameObject.GetComponent<Unit>();
		audioSource = gameObject.transform.parent.gameObject.GetComponent<AudioSource>();
		colliders = gameObject.GetComponents<Collider>().ToList();

		//disable all attack colliders
		colliders.ForEach(collider => collider.enabled = false);
	}

	private void Update()
	{
		attackTimer += Time.deltaTime;

		#region Process TryAttack

		if (TryAttack != null)
		{
			if (unit.AttackQueue.Count >= AttackQueueLength)
			{
				unit.AttackQueue[unit.AttackQueue.Count - 1] = TryAttack;
			}
			else
			{
				unit.AttackQueue.Add(TryAttack);
			}

			TryAttack = null;
		}
		

		#endregion

		#region Process existing attack state

		if (unit.CurrentAttack != null)
		{
			if (attackTimer > previousAttackCooldown && unit.AttackQueue.Count > 0)
			{
				BeginNewAttack();
			}
			else if (attackTimer > unit.CurrentAttack.AnimationLength)
			{
				unit.CurrentAttack = null;
			}
		}
		else
		{
			if (attackTimer > previousAttackCooldown && unit.AttackQueue.Count > 0)
			{
				BeginNewAttack();
			}
		}

		#endregion

		#region Collider management

		if (unit.CurrentAttack != null)
		{
			if (attackTimer < unit.CurrentAttack.AnimationLength)
			{
				if (attackTimer >= unit.CurrentAttack.DeadlyRangeStart && attackTimer < unit.CurrentAttack.DeadlyRangeEnd)
				{
					colliders.Where(collider => collider.sharedMaterial.name == unit.CurrentAttack.Name.ToString()).ToList().ForEach(collider => collider.enabled = true);
					colliders.Where(collider => collider.sharedMaterial.name != unit.CurrentAttack.Name.ToString()).ToList().ForEach(collider => collider.enabled = false);
				}
			}
		}

		#endregion
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var enemy = collision.GetComponent<Enemy>();

		if (enemy != null && unit.CurrentAttack != null)
		{
			enemy.Attacked(unit.CurrentAttack.Damage);
		}
	}

	//private void OnTriggerExit2D(Collider2D collision)
	//{
	//
	//}

	private void BeginNewAttack()
	{
		unit.CurrentAttack = unit.AttackQueue.First();
		unit.AttackQueue.Remove(unit.CurrentAttack);

		audioSource.PlayOneShot(unit.CurrentAttack.Name.ToString());

		previousAttackCooldown = unit.CurrentAttack.Cooldown;
		attackTimer = 0;
	}

	private void DoVelocityBoost(float boost)
	{
		if (!unit.UnitState.HasFlag(Enums.UnitStateEnum.Moving))
		{
			if (Math.Abs(unit.rigidBody.velocity.x) - boost > 0)
			{
				boost -= Math.Abs(unit.rigidBody.velocity.x);
			}

			if ((unit.UnitState.HasFlag(Enums.UnitStateEnum.FacingLeft)))
			{
				boost = -boost;
			}

			unit.rigidBody.velocity = new Vector2(unit.rigidBody.velocity.x + boost, unit.rigidBody.velocity.y);
		}
	}
}
