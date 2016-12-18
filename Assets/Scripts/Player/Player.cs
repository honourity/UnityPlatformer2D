using UnityEngine;
using System;

public static class ExtensionMethods
{
    public static Boolean HasFlag(this Player.PlayerStateEnum self, Player.PlayerStateEnum flag)
    {
        return (self & flag) == flag;
    }

    public static Player.PlayerStateEnum NAND(this Player.PlayerStateEnum self, Player.PlayerStateEnum flag)
    {
        return (self | flag) ^ flag;
    }

    public static void LogState(this Player.PlayerStateEnum self)
    {
        foreach (var value in Enum.GetValues(typeof(Player.PlayerStateEnum)))
        {
            Debug.Log(String.Format("{0}: {1}", Enum.GetName(typeof(Player.PlayerStateEnum), value), self.HasFlag((Player.PlayerStateEnum)value)));
        }
    }
}
public class Player : MonoBehaviour
{
    [Flags]
    public enum PlayerStateEnum
    {
        Grounded      = 0x0001,
        Moving        = 0x0002,
        FacingRight   = 0x0004,
        FacingLeft    = 0x0008,
        Attacking     = 0x0010,
        Jumping       = 0x0020,
        DoubleJumping = 0x0040,
        Falling       = 0x0080,
        Landing       = 0x0100,
    }

    public float MoveSpeed = 5;
    public float JumpHeight = 20;
    public float JumpDampening = 0.6f;
    public PlayerStateEnum PlayerState;

    private Animator animator;
    private Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerState = PlayerState.NAND(PlayerStateEnum.Moving);

        // Apply landing
        if (PlayerState.HasFlag(PlayerStateEnum.Landing))
        {
            PlayerState |= Player.PlayerStateEnum.Grounded;
            PlayerState = PlayerState.NAND(Player.PlayerStateEnum.Landing);
        }

        // Zero vertical velocity when we're on the ground
        if (PlayerState.HasFlag(PlayerStateEnum.Grounded))
        {
            body.velocity = new Vector2(body.velocity.x, 0);
        }

        // Check input and take actions
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayerState |= PlayerState.NAND(PlayerStateEnum.Grounded);

            if (!PlayerState.HasFlag(PlayerStateEnum.DoubleJumping))
            {
                PlayerState = PlayerState.NAND(Player.PlayerStateEnum.Grounded);

                body.velocity = new Vector2(body.velocity.x, JumpHeight);

                if (PlayerState.HasFlag(PlayerStateEnum.Jumping))
                {
                    PlayerState |= PlayerStateEnum.DoubleJumping;
                }
                else
                {
                    PlayerState |= PlayerStateEnum.Jumping;
                }
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            PlayerState |= PlayerStateEnum.Moving;
            body.velocity = new Vector2(MoveSpeed, body.velocity.y);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            PlayerState |= PlayerStateEnum.Moving;
            body.velocity = new Vector2(-MoveSpeed, body.velocity.y);
        }

        //jump horizontal movement dampening
        if (body.velocity.x != 0 &&
            !PlayerState.HasFlag(PlayerStateEnum.Moving) &&
            !PlayerState.HasFlag(PlayerStateEnum.Attacking))
        {
            body.velocity = new Vector2(body.velocity.x * JumpDampening, body.velocity.y);
        }
        
        if (body.velocity.y < 0)
        {
            PlayerState |= PlayerStateEnum.Falling;
        } else if (body.velocity.y >= 0)
        {
            PlayerState = PlayerState.NAND(PlayerStateEnum.Falling);
        }
    
        if (PlayerState.HasFlag(PlayerStateEnum.Grounded) && !PlayerState.HasFlag(PlayerStateEnum.Falling))
        {
            PlayerState = PlayerState.NAND(PlayerStateEnum.Jumping);
            PlayerState = PlayerState.NAND(PlayerStateEnum.DoubleJumping);
        }

        if (body.velocity.x > 0 && PlayerState.HasFlag(PlayerStateEnum.Moving))
        {
            PlayerState |= PlayerStateEnum.FacingRight;
            PlayerState = PlayerState.NAND(PlayerStateEnum.FacingLeft);
        }
        else if (body.velocity.x < 0 && PlayerState.HasFlag(PlayerStateEnum.Moving))
        {
            PlayerState |= PlayerStateEnum.FacingLeft;
            PlayerState = PlayerState.NAND(PlayerStateEnum.FacingRight);
        }

        var scale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        if (PlayerState.HasFlag(PlayerStateEnum.FacingLeft)) scale.x *= -1;
        transform.localScale = scale;

        //update animator with latest data
        animator.SetBool("Grounded", PlayerState.HasFlag(PlayerStateEnum.Grounded));
        animator.SetFloat("SpeedHorizontal", Math.Abs(body.velocity.x));
        animator.SetFloat("SpeedVertical", body.velocity.y);
        animator.SetBool("Attacking", PlayerState.HasFlag(PlayerStateEnum.Attacking));

        PlayerState.LogState();
    }
}
