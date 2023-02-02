using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterAttackLogic : ScriptableObject
{
    [SerializeField] private float _attackDistance;
    
    public float AttackDistance => _attackDistance;
  
    public virtual void AttackEnemy(Character attacker, Character enemy, float damage, bool isPercTrigered = false)
    {        
        enemy.TakeDamage(attacker, damage, isPercTrigered);
    }
}
