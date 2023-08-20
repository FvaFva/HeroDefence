using System;
using UnityEngine;

public interface IFightable
{
    public event Action Died;

    public abstract Vector3 CurrentPosition { get; }

    public void ApplyHeal(float heal);

    public void ApplyStamina(int count);

    public void ApplyEffect(EffectLogic effect);

    public bool TryApplyDamage(IFightable attacker, ref float damage, bool isPercTriggered = true);

    public bool IsFriendly(Team verifiableTeam);

    public bool IsFriendly(IFightable verifiableIFightable);

    public void ShowColoredEffectImpact(Color color);
}
