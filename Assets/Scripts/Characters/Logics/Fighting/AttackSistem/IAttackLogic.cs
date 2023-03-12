using System;

public interface IAttackLogic
{
    public WeaponType Type { get; }
    public event Action<IFightable, float, bool> DamageDealed;

    public void AttackEnemy(IFightable attacker, IFightable enemy, float damage, bool isPercTrigered = true);
}
