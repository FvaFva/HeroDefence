using System;
using UnityEngine;

public interface IFightable
{
    public event Action Died;
    public event Action<IFightable, float, bool> TakenDamage;
    
    public abstract Vector3 CurrentPosition { get;}

    public void ApplyHeal(float heal);
    public void ApplyDamage(IFightable attacker, float damage, bool isPercTrigered = true);
    public bool IsFriendly(Team verifiableTeam);
    public bool IsFriendly(IFightable verifiableIFightebel);
}
