using System;
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
    private CharacterAttackLogic _attackLogic;

    public event Action Died;
    public event Action<float> HitPointsChanged;

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
    
    public void SetNewAttackLogic(CharacterAttackLogic newLogic)
    {
        if (newLogic == null || newLogic == _attackLogic)
            return;        

        _attackLogic = newLogic;
    }

    public bool TryApplyDamage(ref float damage, Character attacker)
    {
        damage = GetRealDamage(damage);

        if (damage <= 0)
            return false;

        _hitPointsCurrent -= damage;

        if (_hitPointsCurrent <= 0)
            Deing();

        HitPointsChanged?.Invoke(_hitPointsCurrent / _hitPointsMax);

        return true;
    }

    public void ApplyHeal(float heal)
    {
        _hitPointsCurrent += heal;
        _hitPointsCurrent = (_hitPointsMax < _hitPointsCurrent) ? _hitPointsMax : _hitPointsCurrent;
        HitPointsChanged.Invoke(_hitPointsCurrent / _hitPointsMax);
    }

    public void Attack(Character attacker, Character enemy)
    {
        if (enemy == null || _currentPointsToAttak < GameSettings.Character.StaminaPointsToAtack)
            return;
       
        _attackLogic.AttackEnemy(attacker, enemy, _damage);
        _currentPointsToAttak -= GameSettings.Character.StaminaPointsToAtack;
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
