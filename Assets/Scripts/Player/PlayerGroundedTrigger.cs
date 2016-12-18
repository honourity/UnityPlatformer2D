using UnityEngine;

public class PlayerGroundedTrigger : MonoBehaviour {

    private Player player;

    // Use this for initialization
    void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.sharedMaterial.name == "Ground")
        {
            player.PlayerState |= Player.PlayerStateEnum.Landing;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.sharedMaterial.name == "Ground")
        {
            player.PlayerState = player.PlayerState.NAND(Player.PlayerStateEnum.Grounded);
        }
    }
}
