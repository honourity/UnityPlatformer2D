using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public float MoveSpeed = 5;
    public float JumpHeight = 20;
    public bool Grounded;
    public bool Attacking
    {
        get
        {
            return attackTrigger.Attacking;
        }
        set
        {
            attackTrigger.Attacking = value;
        }
    }

    private PlayerAttackTrigger attackTrigger;
    private Animator animator;
    private Rigidbody2D body;
    private bool movingLeft;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackTrigger = gameObject.GetComponentInChildren<PlayerAttackTrigger>();
    }

    private void Update()
    {
        

        //update animator with latest data
        animator.SetBool("Grounded", Grounded);
        animator.SetFloat("SpeedHorizontal", Math.Abs(body.velocity.x));
        animator.SetFloat("SpeedVertical", body.velocity.y);
        animator.SetBool("Attacking", Attacking);
    }

    void FixedUpdate()
    {
        //stop all movement instantly if no input and no current actions happening
        if (Grounded && !Attacking)
        {
            body.velocity = new Vector2(0, 0);
        }

        //check input and take actions
        if (Input.GetKey(KeyCode.UpArrow) && Grounded)
        {
            body.velocity = new Vector2(body.velocity.x, JumpHeight);
            //Attacking = false;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            body.velocity = new Vector2(MoveSpeed, body.velocity.y);
            //Attacking = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            body.velocity = new Vector2(-MoveSpeed, body.velocity.y);
            //Attacking = false;
        }

        //flip sprite based on movement direction
        if (body.velocity.x < 0)
        {
            if (!movingLeft)
            {
                var scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }

            movingLeft = true;
        }
        else if (body.velocity.x > 0)
        {
            if (movingLeft)
            {
                var scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }

            movingLeft = false;
        }
    }
}
