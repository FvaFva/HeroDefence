using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CombatUnit
{
    private CombatUnit _target = null;
    private float _attackSpeed;
    private float _attackSpeedCoefficient;
    private float _targetpointsToAttak = 1000;
    private float _currentpointsToAttak = 1000;

    public float HitPointsMax { get; protected set; }
    public float HitPointsCurrent { get; protected set; }
    public float Armor { get; protected set; }
    public float Damage { get; protected set; }
    public bool IsDead { get; protected set; }
    public UnityEvent Died = new UnityEvent();
    public UnityEvent Attacked = new UnityEvent();
    public UnityEvent OnAttack = new UnityEvent();
    public UnityEvent<CombatUnit> ChagedTarget = new UnityEvent<CombatUnit>();
    public UnityEvent<EffectImpact> ImpactingEffect = new UnityEvent<EffectImpact>();

    public  CombatUnit(float hitPoints, float armor, float damage, float attackSpeed)
    {       
        HitPointsMax = hitPoints;
        Armor = armor;
        Damage = damage;
        IsDead = false;
        _attackSpeed = attackSpeed;
        HitPointsCurrent= hitPoints;
    }

    public void TakeDamage(float damage)
    {
        if(damage<=0) 
            return;

        Attacked?.Invoke();
        float realDamage = damage * Armor / 100;

        HitPointsCurrent -= realDamage;

        if (HitPointsCurrent <= 0)
            Deing();
    }

    public void Attack()
    {
        if (_target == null || _target.IsDead || _currentpointsToAttak< _targetpointsToAttak)
            return;

        OnAttack.Invoke();
        _target.TakeDamage(Damage);
        _currentpointsToAttak = 0;
    }

    public void AddEffect(EffectLogic effect, Anima caster, float duration)
    {
        ImpactingEffect.Invoke(new EffectImpact(effect, caster, duration));
    }

    public IEnumerator Resting()
    {
        while(true)
        {
            if (_currentpointsToAttak < _targetpointsToAttak)
                _currentpointsToAttak += _attackSpeed * _attackSpeedCoefficient * Time.deltaTime;

            yield return null;
        }
    }

    private void Deing()
    {
        IsDead = true;
        Died?.Invoke();
    }

    public void SetTarget(CombatUnit newTarget)
    {
        _target = newTarget;
        ChagedTarget.Invoke(newTarget);
    }
}
