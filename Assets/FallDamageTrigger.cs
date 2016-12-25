using UnityEngine;
using System;

public class FallDamageTrigger : MonoBehaviour
{
    public int DamageScale = 1;
    public float VelocityDamageStart = 20;

    private Unit unit;

    void Start()
    {
        unit = gameObject.GetComponentInParent<Unit>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var currentVelocity = Math.Abs(unit.rigidBody.velocity.y);

        if (currentVelocity >= VelocityDamageStart)
        {
            unit.HealthCurrent--;
        }
    }
}