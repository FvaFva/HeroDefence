using System;

public interface IAttackLogic
{
    public event Action<IFightable, float, bool> DamageDealt;

    public WeaponType Type { get; }

    public void AttackEnemy(IFightable attacker, IFightable enemy, float damage, bool isPercTriggered = true);
}
