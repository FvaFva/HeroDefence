public class Weapon:Item
{
    public float AttackDistance { get; private set; }
    public IAttackLogic AttackLogic { get; private set; }

    public Weapon(ItemPreset preset, float attackDistance, WeaponType attackLogic) : base(preset)
    {
        AttackDistance = attackDistance;

        switch (attackLogic)
        {
            case WeaponType.Melee:
                AttackLogic = new MeleeAttackLogic();
            break;
        }
    }
}
