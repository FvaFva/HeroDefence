using System;
using UnityEngine;

public interface IFightebel
{
    public event Action Died;
    public event Action<IFightebel, float, bool> TakenDamage;
    
    public abstract Vector3 CurrentPosition { get;}

    public void ApplyHeal(float heal);
    public void ApplyDamage(IFightebel attacker, float damage, bool isPercTrigered = true);
    public bool CheckFriendly(Team verifiableTeam);
}
