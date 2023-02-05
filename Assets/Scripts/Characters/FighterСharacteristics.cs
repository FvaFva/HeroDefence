using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Fighter—haracteristics
{
    public float AttackSpeed;
    public float HitPointsMax;
    public float HitPointsCurrent;
    public float Armor;
    public float Damage;
    public float HiPointsCoefficient => HitPointsCurrent / HitPointsMax;

    public Fighter—haracteristics(float attackSpeed, float damage, float armor, float hitPointsCurrent, float hitPointsMax)
    {
        AttackSpeed = attackSpeed;
        Damage = damage;
        Armor = armor;
        HitPointsMax = hitPointsMax;
        HitPointsCurrent = hitPointsCurrent;
    }
}
