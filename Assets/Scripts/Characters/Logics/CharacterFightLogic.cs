using System;
using System.Collections;
using UnityEngine;

public class CharacterFightLogic
{
    private const float PointsToAttack = GameSettings.Character.StaminaPointsToAtack;

    private float _attackSpeed;
    private float _attackSpeedCoefficient;
    private float _currentPointsToAttak;
    private float _hitPointsMax;
    private float _hitPointsCurrent;
    private float _armor;
    private float _damage;

    public float HiPointsCoefficient => _hitPointsCurrent / _hitPointsMax;

    private CharacterAttackLogic _attackLogic;

    public FighterÑharacteristics Ñharacteristics => new (_attackSpeed, _damage, _hitPointsCurrent, _hitPointsCurrent, _hitPointsMax);
    public event Action Died;
    public event Action HitPointsChanged;

    public CharacterFightLogic(float hitPoints, float armor, float damage, float attackSpeed)
    {       
        _hitPointsMax = hitPoints;
        _armor = armor;
        _damage = damage;
        _attackSpeed = attackSpeed;
        _hitPointsCurrent= hitPoints;
        _attackSpeedCoefficient = 1;
        _currentPointsToAttak = PointsToAttack;
    }
    
    public void SetNewAttackLogic(CharacterAttackLogic newLogic)
    {
        if (newLogic == null || newLogic == _attackLogic)
            return;        

        _attackLogic = newLogic;
    }

    public bool TryApplyDamage(ref float damage)
    {
        damage = GetRealDamage(damage);

        if (damage <= 0)
            return false;

        _hitPointsCurrent -= damage;

        if (_hitPointsCurrent <= 0)
            Died?.Invoke();

        HitPointsChanged?.Invoke();

        return true;
    }

    public void ApplyHeal(float heal)
    {
        _hitPointsCurrent += heal;
        _hitPointsCurrent = (_hitPointsMax < _hitPointsCurrent) ? _hitPointsMax : _hitPointsCurrent;
        HitPointsChanged.Invoke();
    }

    public void Attack(IFightebel attacker, IFightebel enemy)
    {
        if (enemy == null || _currentPointsToAttak < PointsToAttack)
            return;
       
        _attackLogic.AttackEnemy(attacker, enemy, _damage);
        _currentPointsToAttak -= PointsToAttack;
    }

    public IEnumerator Resting()
    {
        while(true)
        {
            if (_currentPointsToAttak < PointsToAttack)
                _currentPointsToAttak += _attackSpeed * _attackSpeedCoefficient * Time.deltaTime;

            yield return null;
        }
    }

    private float GetRealDamage(float damage)
    {        
        float armorImpact = Mathf.Pow(GameSettings.Character.ArmorUnitImpact, _armor);
        return armorImpact * damage;
    }
}
