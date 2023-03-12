using System;

public class MeleeAttackLogic : IAttackLogic
{
    public event Action<IFightable, float, bool> DamageDealed;

    public WeaponType Type => WeaponType.Melee;

    public void AttackEnemy(IFightable attacker, IFightable enemy, float damage, bool isPercTrigered = true)
    {
        if (enemy.TryApplyDamage(attacker, ref damage, isPercTrigered))
            DamageDealed?.Invoke(enemy, damage, isPercTrigered);
    }
}
