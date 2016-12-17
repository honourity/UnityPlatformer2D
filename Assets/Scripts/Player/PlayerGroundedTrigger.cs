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
            player.Grounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.sharedMaterial.name == "Ground")
        {
            player.Grounded = false;
        }
    }
}
