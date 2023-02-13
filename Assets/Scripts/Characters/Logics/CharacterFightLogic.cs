using System;
using System.Collections;
using UnityEngine;

public class CharacterFightLogic : IReachLogic
{
    private const float StaminaToAttack = GameSettings.Character.StaminaPointsToAtack;

    private float _attackSpeed;
    private float _attackSpeedCoefficient;
    private float _currentStamina;
    private float _hitPointsMax;
    private float _hitPointsCurrent;
    private float _armor;
    private float _damage;

    private IFightebel _enemy;
    private IFightebel _attacker;
    private CharacterAttackLogic _attackLogic;

    public float HiPointsCoefficient => _hitPointsCurrent / _hitPointsMax;

    public FighterÑharacteristics Ñharacteristics => new (_attackSpeed, _damage, _hitPointsCurrent, _hitPointsCurrent, _hitPointsMax);
    public event Action Died;
    public event Action HitPointsChanged;

    public event Action Reached;
    public event Action Failed;

    public CharacterFightLogic(float hitPoints, float armor, float damage, float attackSpeed)
    {       
        _hitPointsMax = hitPoints;
        _armor = armor;
        _damage = damage;
        _attackSpeed = attackSpeed;
        _hitPointsCurrent= hitPoints;
        _attackSpeedCoefficient = 1;
        _currentStamina = StaminaToAttack;
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

    private void Attack()
    {
        if (_enemy == null || _currentStamina < StaminaToAttack)
            return;
       
        _attackLogic.AttackEnemy(_attacker, _enemy, _damage);
        _currentStamina -= StaminaToAttack;
    }

    public IEnumerator Resting()
    {
        while(true)
        {
            if (_currentStamina < StaminaToAttack)
                _currentStamina += _attackSpeed * _attackSpeedCoefficient * Time.deltaTime;

            yield return null;
        }
    }

    private float GetRealDamage(float damage)
    {        
        float armorImpact = Mathf.Pow(GameSettings.Character.ArmorUnitImpact, _armor);
        return armorImpact * damage;
    }

    public IEnumerator ReachTarget()
    {
        while(_enemy != null)
        {
            Attack();
            yield return null;
        }
    }

    public void SetTarget(Target target)
    {
        target.TryGetFightebel(out _enemy);
    }
}
