using Assets.Scripts;
using UnityEngine;

public class GroundedTrigger : MonoBehaviour
{

    private Unit unit;

    // Use this for initialization
    void Start()
    {
        unit = gameObject.GetComponentInParent<Unit>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.sharedMaterial != null)
            && collision.sharedMaterial.name == "Ground"
            || collision.sharedMaterial.name == "Unit"
            || collision.sharedMaterial.name == "Player")
        {
            unit.UnitState |= Enums.UnitStateEnun.Landing;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.sharedMaterial != null)
            && collision.sharedMaterial.name == "Ground"
            || collision.sharedMaterial.name == "Unit"
            || collision.sharedMaterial.name == "Player")
        {
            unit.UnitState = unit.UnitState.NAND(Enums.UnitStateEnun.Grounded);
        }
    }
}
