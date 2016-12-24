using System;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class HorizontalAttackTrigger : MonoBehaviour
{
    public float Attack1DeadlyRangeStart = 0;
    public float Attack1DeadlyRangeEnd = 0.5f;
    public float Attack1Cooldown = 1f;
    public float Attack1VelocityBoost = 5f;
    public float Attack1AnimationLength = 0.5f;
    public int Attack1Damage = 1;
    public AudioClip Attack1Sound;

    public float Attack2DeadlyRangeStart = 0;
    public float Attack2DeadlyRangeEnd = 0.5f;
    public float Attack2Cooldown = 1f;
    public float Attack2VelocityBoost = 5f;
    public float Attack2AnimationLength = 0.5f;
    public int Attack2Damage = 2;
    public AudioClip Attack2Sound;

    public float Attack3DeadlyRangeStart = 0;
    public float Attack3DeadlyRangeEnd = 0.5f;
    public float Attack3Cooldown = 1f;
    public float Attack3VelocityBoost = 5f;
    public float Attack3AnimationLength = 0.5f;
    public int Attack3Damage = 5;
    public AudioClip Attack3Sound;

    public bool TryAttack = false;

    private Collider2D attack1Collider;
    private Collider2D attack2Collider;
    private Collider2D attack3Collider;
    private Unit unit;

    private float attackTimer;
    private AudioSource audioSource;

    private void Awake()
    {
        unit = gameObject.transform.parent.gameObject.GetComponent<Unit>();

        var colliders = gameObject.GetComponents<Collider2D>();
        attack1Collider = colliders.FirstOrDefault(collider => collider.sharedMaterial.name == "Attack1Collider");
        attack2Collider = colliders.FirstOrDefault(collider => collider.sharedMaterial.name == "Attack2Collider");
        attack3Collider = colliders.FirstOrDefault(collider => collider.sharedMaterial.name == "Attack3Collider");

        attack1Collider.enabled = false;
        attack2Collider.enabled = false;
        attack3Collider.enabled = false;

        //start large because 0 timer means attack just happened
        attackTimer = 0;

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;

        #region Process existing attack state

        if (unit.UnitState.HasFlag(Enums.UnitStateEnum.AttackingH1))
        {
            if (attackTimer > Attack1Cooldown)
            {
                unit.UnitState = unit.UnitState.NAND(Enums.UnitStateEnum.Attacking);
                unit.UnitState = unit.UnitState.NAND(Enums.UnitStateEnum.AttackingH1);
            }
            else if (attackTimer > Attack1AnimationLength)
            {
                unit.UnitState = unit.UnitState.NAND(Enums.UnitStateEnum.Attacking);
            }
        }
        else if (unit.UnitState.HasFlag(Enums.UnitStateEnum.AttackingH2))
        {
            if (attackTimer > Attack2Cooldown)
            {
                unit.UnitState = unit.UnitState.NAND(Enums.UnitStateEnum.Attacking);
                unit.UnitState = unit.UnitState.NAND(Enums.UnitStateEnum.AttackingH2);
            }
            else if (attackTimer > Attack2AnimationLength)
            {
                unit.UnitState = unit.UnitState.NAND(Enums.UnitStateEnum.Attacking);
            }
        }
        else if (unit.UnitState.HasFlag(Enums.UnitStateEnum.AttackingH3))
        {
            if (attackTimer > Attack3Cooldown)
            {
                unit.UnitState = unit.UnitState.NAND(Enums.UnitStateEnum.Attacking);
                unit.UnitState = unit.UnitState.NAND(Enums.UnitStateEnum.AttackingH3);
            }
            else if (attackTimer > Attack3AnimationLength)
            {
                unit.UnitState = unit.UnitState.NAND(Enums.UnitStateEnum.Attacking);
            }
        }

        #endregion

        #region Try attack

        if (TryAttack)
        {
            if (!(unit.UnitState.HasFlag(Enums.UnitStateEnum.AttackingH1)
                || unit.UnitState.HasFlag(Enums.UnitStateEnum.AttackingH2)
                || unit.UnitState.HasFlag(Enums.UnitStateEnum.AttackingH3)))
            {
                unit.UnitState |= Enums.UnitStateEnum.Attacking;
                unit.UnitState |= Enums.UnitStateEnum.AttackingH1;
                DoVelocityBoost(Attack1VelocityBoost);
                attackTimer = 0;

                audioSource.PlayOneShot(Attack1Sound);
            }
            else if (unit.UnitState.HasFlag(Enums.UnitStateEnum.AttackingH1))
            {
                if (attackTimer > Attack1AnimationLength && attackTimer < Attack1Cooldown)
                {
                    unit.UnitState = unit.UnitState.NAND(Enums.UnitStateEnum.AttackingH1);
                    unit.UnitState |= Enums.UnitStateEnum.Attacking;
                    unit.UnitState |= Enums.UnitStateEnum.AttackingH2;
                    DoVelocityBoost(Attack2VelocityBoost);
                    attackTimer = 0;

                    audioSource.PlayOneShot(Attack2Sound);
                }
            }
            else if (unit.UnitState.HasFlag(Enums.UnitStateEnum.AttackingH2))
            {
                if (attackTimer > Attack2AnimationLength && attackTimer < Attack2Cooldown)
                {
                    unit.UnitState = unit.UnitState.NAND(Enums.UnitStateEnum.AttackingH2);
                    unit.UnitState |= Enums.UnitStateEnum.Attacking;
                    unit.UnitState |= Enums.UnitStateEnum.AttackingH3;
                    DoVelocityBoost(Attack3VelocityBoost);
                    attackTimer = 0;

                    audioSource.PlayOneShot(Attack3Sound);
                }
            }

            TryAttack = false;
        }

        #endregion

        #region Collider management

        if (unit.UnitState.HasFlag(Enums.UnitStateEnum.AttackingH1))
        {
            if (attackTimer < Attack1AnimationLength)
            {
                if (attackTimer >= Attack1DeadlyRangeStart && attackTimer < Attack1DeadlyRangeEnd)
                {
                    attack1Collider.enabled = true;
                }
                else
                {
                    attack1Collider.enabled = false;
                }
            }
            else
            {
                attack1Collider.enabled = false;
            }
        }
        else if (unit.UnitState.HasFlag(Enums.UnitStateEnum.AttackingH2))
        {
            if (attackTimer < Attack2AnimationLength)
            {
                if (attackTimer >= Attack2DeadlyRangeStart && attackTimer < Attack2DeadlyRangeEnd)
                {
                    attack2Collider.enabled = true;
                }
                else
                {
                    attack2Collider.enabled = false;
                }
            }
            else
            {
                attack2Collider.enabled = false;
            }
        }
        else if (unit.UnitState.HasFlag(Enums.UnitStateEnum.AttackingH3))
        {
            if (attackTimer < Attack3AnimationLength)
            {
                if (attackTimer >= Attack3DeadlyRangeStart && attackTimer < Attack3DeadlyRangeEnd)
                {
                    attack3Collider.enabled = true;
                }
                else
                {
                    attack3Collider.enabled = false;
                }
            }
        }

        #endregion
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (unit.UnitState.HasFlag(Enums.UnitStateEnum.AttackingH1))
            {
                enemy.Attacked(Attack1Damage);
            }
            else if (unit.UnitState.HasFlag(Enums.UnitStateEnum.AttackingH2))
            {
                enemy.Attacked(Attack2Damage);
            }
            else if (unit.UnitState.HasFlag(Enums.UnitStateEnum.AttackingH3))
            {
                enemy.Attacked(Attack3Damage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

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
