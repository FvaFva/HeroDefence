public struct FighterĐharacteristics
{
    public float AttackSpeed;
    public float HitPoints;
    public float Armor;
    public float Damage;
    public float Speed;
    public float ManaPoints;
    public float ManaRegen;

    public FighterĐharacteristics(float attackSpeed, float damage, float armor, float hitPointsMax, float speed,
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

    public void ApplyCharacteristics(FighterĐharacteristics ˝haracteristics)
    {
        AttackSpeed += ˝haracteristics.AttackSpeed;
        Damage += ˝haracteristics.Damage;
        Armor += ˝haracteristics.Armor;
        HitPoints += ˝haracteristics.HitPoints;
        Speed += ˝haracteristics.Speed;
        ManaPoints += ˝haracteristics.ManaRegen;
        ManaRegen += ˝haracteristics.ManaPoints;
    }
}
