using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterFightLogic
{
    private float _attackSpeed;
    private float _attackSpeedCoefficient;
    private float _currentPointsToAttak;
    private float _hitPointsMax;
    private float _hitPointsCurrent;
    private float _armor;
    private float _damage;
    private AttackLogic _attackLogic;

    public UnityEvent Died = new UnityEvent();
    public UnityEvent Attacked = new UnityEvent();
    public UnityEvent OnAttack = new UnityEvent();
    public UnityEvent<EffectImpact> ImpactingEffect = new UnityEvent<EffectImpact>();
    public UnityEvent<float> HitPointsChanged = new UnityEvent<float>();

    public CharacterFightLogic(float hitPoints, float armor, float damage, float attackSpeed)
    {       
        _hitPointsMax = hitPoints;
        _armor = armor;
        _damage = damage;
        _attackSpeed = attackSpeed;
        _hitPointsCurrent= hitPoints;
        _attackSpeedCoefficient = 1;
        _currentPointsToAttak = GameSettings.Character.StaminaPointsToAtack;
    }
    
    public void SetNewAttackLogic(AttackLogic newLogic)
    {
        if (newLogic == null || newLogic == _attackLogic)
            return;        

        _attackLogic = newLogic;
    }

    public void ApplyDamage(float damage)
    {
        if (damage <= 0)
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
        if (enemy == null || _currentPointsToAttak < GameSettings.Character.StaminaPointsToAtack)
            return;

        OnAttack.Invoke();
        _attackLogic.AttackEnemy(enemy, _damage);
        _currentPointsToAttak -= GameSettings.Character.StaminaPointsToAtack;
    }

    public void AddEffect(EffectLogic effect, Anima caster, float duration)
    {
        ImpactingEffect.Invoke(new EffectImpact(effect, caster, duration));
    }

    public IEnumerator Resting()
    {
        while(true)
        {
            if (_currentPointsToAttak < GameSettings.Character.StaminaPointsToAtack)
                _currentPointsToAttak += _attackSpeed * _attackSpeedCoefficient * Time.deltaTime;

            yield return null;
        }
    }

    private float GetRealDamage(float damage)
    {        
        float armorImpact = Mathf.Pow(GameSettings.Character.ArmorUnitImpact, _armor);
        return armorImpact * damage;
    }

    private void Deing()
    {        
        Died?.Invoke();
    }
}
