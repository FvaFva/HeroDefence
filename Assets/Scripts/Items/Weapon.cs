public class Weapon : Item
{
    public Weapon(ItemPreset preset, IAttackLogic attackLogic, float attackDistance, Perc perc)
        : base(preset, perc)
    {
        AttackDistance = attackDistance;
        AttackLogic = attackLogic;
    }

    public float AttackDistance { get; private set; }

    public IAttackLogic AttackLogic { get; private set; }
}
