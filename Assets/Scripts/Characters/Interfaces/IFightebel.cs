using System;
using UnityEngine;

public interface IFightable
{
    public event Action Died;
    
    public abstract Vector3 CurrentPosition { get;}

    public void ApplyHeal(float heal);
    public bool TryApplyDamage(IFightable attacker,ref float damage, bool isPercTrigered = true);
    public bool IsFriendly(Team verifiableTeam);
    public bool IsFriendly(IFightable verifiableIFightebel);
}
