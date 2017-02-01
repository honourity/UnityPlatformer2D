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
	private AudioClip[] audioClips;
	private List<Collider2D> colliders;
	private Attack previousAttack;
	private float attackTimer;

	public void ClearAttackQueue()
	{
		unit.AttackQueue.Clear();

		previousAttack = unit.CurrentAttack;
		unit.CurrentAttack = null;
	}

	public bool PreviousAttackInCooldown()
	{
		if (unit.CurrentAttack == null && previousAttack != null && attackTimer < previousAttack.Cooldown)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private void Awake()
	{
		unit = gameObject.transform.parent.gameObject.GetComponent<Unit>();
		audioSource = gameObject.transform.parent.gameObject.GetComponent<AudioSource>();
		colliders = gameObject.GetComponents<Collider2D>().ToList();

		//load audioclips
		audioClips = Resources.LoadAll<AudioClip>("Audio");

		//disable all attack colliders
		colliders.ForEach(collider => collider.enabled = false);
	}

	private void Update()
	{
		attackTimer += Time.deltaTime;

		#region Process TryAttack

		if (TryAttack != null)
		{
			//cant queue/chain root attacks
			if ((unit.CurrentAttack == null || unit.AttackQueue.Count > 0) || !AttackGraph.RootAttacks.Contains(TryAttack))
			{
				if (unit.AttackQueue.Count >= AttackQueueLength)
				{
					unit.AttackQueue[unit.AttackQueue.Count - 1] = TryAttack;
				}
				else
				{
					unit.AttackQueue.Add(TryAttack);
				}
			}

			TryAttack = null;
		}


		#endregion

		#region Process existing attack state

		//need to figure out if attack is part of a chain,
		// in which case use animationlength instead of cooldown to decide when to start next attack
		if ((previousAttack == null || attackTimer > previousAttack.AnimationLength) && unit.AttackQueue.Count > 0 && !AttackGraph.RootAttacks.Contains(unit.AttackQueue.FirstOrDefault()))
		{
			BeginNewAttack();
		}
		else if ((previousAttack == null || attackTimer > previousAttack.Cooldown) && unit.AttackQueue.Count > 0)
		{
			BeginNewAttack();
		}
		else if (unit.CurrentAttack != null && attackTimer > unit.CurrentAttack.AnimationLength)
		{
			previousAttack = unit.CurrentAttack;
			unit.CurrentAttack = null;
		}

		#endregion

		#region Collider management

		colliders.ForEach(collider => collider.enabled = false);

		if (unit.CurrentAttack != null)
		{
			if (attackTimer < unit.CurrentAttack.AnimationLength)
			{
				if (attackTimer >= unit.CurrentAttack.DeadlyRangeStart && attackTimer < unit.CurrentAttack.DeadlyRangeEnd)
				{
					colliders.FirstOrDefault(collider => collider.sharedMaterial.name == unit.CurrentAttack.Name.ToString()).enabled = true;
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

		var audioClip = audioClips.FirstOrDefault(clip => clip.name == unit.CurrentAttack.Name.ToString());
		audioSource.PlayOneShot(audioClip);

		previousAttack = unit.CurrentAttack;
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
