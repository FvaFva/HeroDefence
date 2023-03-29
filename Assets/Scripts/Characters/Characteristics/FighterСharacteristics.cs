public struct FighterCharacteristics
{
    public float Damage;
    public float AttackSpeed;
    public float Armor;
    public float HitPoints;
    public float Speed;
    public float ManaPoints;
    public float ManaRegen;

    public FighterCharacteristics(float attackSpeed, float damage, float armor, float hitPointsMax, float speed,
                                  float manaRegen, float manaPoints)
    {
        Damage = damage;
        AttackSpeed = attackSpeed;
        Armor = armor;
        HitPoints = hitPointsMax;
        Speed = speed;
        ManaPoints = manaRegen;
        ManaRegen = manaPoints;
    }

    public void ApplyCharacteristics(FighterCharacteristics characteristics)
    {
        AttackSpeed += characteristics.AttackSpeed;
        Damage += characteristics.Damage;
        Armor += characteristics.Armor;
        HitPoints += characteristics.HitPoints;
        Speed += characteristics.Speed;
        ManaPoints += characteristics.ManaRegen;
        ManaRegen += characteristics.ManaPoints;
    }
}
