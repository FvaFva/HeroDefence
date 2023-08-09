using System;

public class MeleeAttackLogic : IAttackLogic
{
    public event Action<IFightable, float, bool> DamageDealt;

    public WeaponType Type => WeaponType.Melee;

    public void AttackEnemy(IFightable attacker, IFightable enemy, float damage, bool isPercTrigered = true)
    {
        if (enemy.TryApplyDamage(attacker, ref damage, isPercTrigered))
            DamageDealt?.Invoke(enemy, damage, isPercTrigered);
    }
}
