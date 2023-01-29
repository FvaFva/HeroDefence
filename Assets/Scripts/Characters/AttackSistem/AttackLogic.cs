using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AttackLogic : ScriptableObject
{
    [SerializeField] private float _attackDistance;
    
    public float AttackDistance => _attackDistance;
  
    public abstract void AttackEnemy(Character enemy, float damage);
}
