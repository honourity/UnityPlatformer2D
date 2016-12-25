using Assets.Scripts;
using System;
using UnityEngine;

public class UnitHealthBar : MonoBehaviour
{
    private Unit parentUnit;

    void Start()
    {
        parentUnit = GetComponentInParent<Unit>();
    }

    void Update()
    {
        //cast is NOT redundant
        transform.localScale = new Vector3((((float)parentUnit.HealthCurrent) / (float)parentUnit.HealthMax), transform.localScale.y, transform.localScale.z);

        ////flip the same way Unit does, so when unit is flipped, this is flipped back to original direction
        //var scale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        //if (parentUnit.UnitState.HasFlag(Enums.UnitStateEnum.FacingRight))
        //{
        //	scale.x *= -1;
        //}
        //transform.localScale = scale;
    }
}