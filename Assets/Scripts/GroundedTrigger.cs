using Assets.Scripts;
using UnityEngine;

public class GroundedTrigger : MonoBehaviour
{

	private Player player;

	// Use this for initialization
	void Start()
	{
		player = gameObject.GetComponentInParent<Player>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((collision.sharedMaterial != null)
			&& collision.sharedMaterial.name == "Ground"
			|| collision.sharedMaterial.name == "Unit"
			|| collision.sharedMaterial.name == "Player")
		{
			player.UnitState |= Enums.UnitStateEnum.Landing;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if ((collision.sharedMaterial != null)
			&& collision.sharedMaterial.name == "Ground"
			|| collision.sharedMaterial.name == "Unit"
			|| collision.sharedMaterial.name == "Player")
		{
			player.UnitState = player.UnitState.NAND(Enums.UnitStateEnum.Grounded);
		}
	}
}
