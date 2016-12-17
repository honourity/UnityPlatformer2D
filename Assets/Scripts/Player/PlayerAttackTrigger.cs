using UnityEngine;
using System.Collections;

public class PlayerAttackTrigger : MonoBehaviour {
    public float AttackDeadlyRangeStart = 0;
    public float AttackDeadlyRangeEnd = 0.5f;
    public float AttackCooldown = 1.0f;
    public float AnimationLength = 0.5f;
    public bool Attacking;

    //private Animator animator;
    private Collider2D attackCollider;
    private float attackTimer = 0;

    private void Awake()
    {
        attackCollider = gameObject.GetComponent<Collider2D>();
        attackCollider.enabled = false;
        Attacking = false;
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;

        if (Input.GetKey(KeyCode.F) && (attackTimer == 0 || attackTimer > AttackCooldown))
        {
            Attacking = true;
            attackTimer = 0;
        }

        if (Attacking)
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
                Attacking = false;
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
