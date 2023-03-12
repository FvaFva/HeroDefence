public struct Fighter—haracteristics
{
    public float AttackSpeed;
    public float HitPoints;
    public float Armor;
    public float Damage;
    public float Speed;
    public float ManaPoints;
    public float ManaRegen;

    public Fighter—haracteristics(float attackSpeed, float damage, float armor, float hitPointsMax, float speed,
                                  float manaRegen, float manaPoints)
    {
        AttackSpeed = attackSpeed;
        Damage = damage;
        Armor = armor;
        HitPoints = hitPointsMax;
        Speed = speed;
        ManaPoints = manaRegen;
        ManaRegen = manaPoints;
    }

    public void ApplyCharacteristics(Fighter—haracteristics Òharacteristics)
    {
        AttackSpeed += Òharacteristics.AttackSpeed;
        Damage += Òharacteristics.Damage;
        Armor += Òharacteristics.Armor;
        HitPoints += Òharacteristics.HitPoints;
        Speed += Òharacteristics.Speed;
        ManaPoints += Òharacteristics.ManaRegen;
        ManaRegen += Òharacteristics.ManaPoints;
    }
}
