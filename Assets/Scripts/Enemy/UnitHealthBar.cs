using Assets.Scripts;
using UnityEngine;

public class UnitHealthBar : MonoBehaviour
{
	private Unit parentUnit;

	// Use this for initialization
	void Start()
	{
		parentUnit = GetComponentInParent<Unit>();
	}

	// Update is called once per frame
	void Update()
	{
		transform.localScale = new Vector3(parentUnit.HealthCurrent / parentUnit.HealthMax, transform.localScale.y, transform.localScale.z);

		////flip the same way Unit does, so when unit is flipped, this is flipped back to original direction
		//var scale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		//if (parentUnit.UnitState.HasFlag(Enums.UnitStateEnum.FacingRight))
		//{
		//	scale.x *= -1;
		//}
		//transform.localScale = scale;
	}
}