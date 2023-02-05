using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterAttackLogic : ScriptableObject
{
    [SerializeField] private float _attackDistance;
    
    public float AttackDistance => _attackDistance;
  
    public virtual void AttackEnemy(IFightebel attacker, IFightebel enemy, float damage, bool isPercTrigered = true)
    {        
        enemy.ApplyDamage(attacker, damage, isPercTrigered);
    }
}
