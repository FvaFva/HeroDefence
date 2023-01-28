using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CombatUnit
{
    private float _attackSpeed;
    private float _attackSpeedCoefficient = 1;
    private float _targetpointsToAttak = 1000;
    private float _currentpointsToAttak = 1000;
    private float _hitPointsMax;
    private float _hitPointsCurrent;
    private float _armor;
    private float _damage;
    public UnityEvent Died = new UnityEvent();
    public UnityEvent Attacked = new UnityEvent();
    public UnityEvent OnAttack = new UnityEvent();
    public UnityEvent<CombatUnit> ChagedTarget = new UnityEvent<CombatUnit>();
    public UnityEvent<EffectImpact> ImpactingEffect = new UnityEvent<EffectImpact>();
    public UnityEvent<float> HitPointsChanged = new UnityEvent<float>();

    public CombatUnit(float hitPoints, float armor, float damage, float attackSpeed)
    {       
        _hitPointsMax = hitPoints;
        _armor = armor;
        _damage = damage;
        _attackSpeed = attackSpeed;
        _hitPointsCurrent= hitPoints;
    }
    
    public void ApplyDamage(float damage)
    {
        if(damage<=0) 
            return;

        Attacked?.Invoke();        
        _hitPointsCurrent -= GetRealDamage(damage);

        if (_hitPointsCurrent <= 0)
            Deing();

        HitPointsChanged.Invoke(_hitPointsCurrent / _hitPointsMax);
    }

    public void ApplyHeal(float heal)
    {
        _hitPointsCurrent += heal;
        _hitPointsCurrent = (_hitPointsMax < _hitPointsCurrent) ? _hitPointsMax : _hitPointsCurrent;
        HitPointsChanged.Invoke(_hitPointsCurrent / _hitPointsMax);
    }

    public void Attack(Character enemy)
    {
        if (enemy == null || _currentpointsToAttak < _targetpointsToAttak)
            return;

        OnAttack.Invoke();
        enemy.ApplyDamage(_damage);
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

    private float GetRealDamage(float damage)
    {
        float armorImpactPerUnit = 0.97f;
        float armorImpact = Mathf.Pow(armorImpactPerUnit, _armor);
        return armorImpact * damage;
    }

    private void Deing()
    {        
        Died?.Invoke();
    }
}
