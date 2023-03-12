using System;
using System.Collections;
using UnityEngine;

public class CharacterFightLogic : IReachLogic
{
    private const float StaminaToAttack = GameSettings.Character.StaminaPointsToAtack;

    private float _attackSpeed;
    private float _currentStamina;
    private float _hitPointsMax;
    private float _hitPointsCurrent;
    private float _armor;
    private float _damage;

    private IFightable _enemy;
    private IFightable _attacker;
    private IAttackLogic _attackLogic;

    public float HitPointsCoefficient => _hitPointsCurrent / _hitPointsMax;
    public event Action Died;
    public event Action HitPointsChanged;

    public event Action<Target> Reached;

    public CharacterFightLogic(Fighter�haracteristics �haracteristics, IFightable attacker)
    {               
        _hitPointsCurrent= �haracteristics.HitPoints;
        _currentStamina = StaminaToAttack;
        ApplyNewCharacteristics(�haracteristics);
        _attacker = attacker;
    }
    
    public void ApplyNewCharacteristics(Fighter�haracteristics �haracteristics)
    {
        _hitPointsMax = �haracteristics.HitPoints;
        _hitPointsCurrent = �haracteristics.HitPoints * HitPointsCoefficient;
        _armor = �haracteristics.Armor;
        _damage = �haracteristics.Damage;
        _attackSpeed = �haracteristics.AttackSpeed;
    }

    public void SetNewAttackLogic(IAttackLogic newLogic)
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
        if (_currentStamina >= StaminaToAttack)
        {
            _attackLogic.AttackEnemy(_attacker, _enemy, _damage);
            _currentStamina -= StaminaToAttack;
        }        
    }

    public IEnumerator Resting()
    {
        while(true)
        {
            if (_currentStamina < StaminaToAttack)
                _currentStamina += _attackSpeed * GameSettings.Character.SecondsDelay;

            yield return GameSettings.Character.OptimizationDelay;
        }
    }

    private float GetRealDamage(float damage)
    {        
        float armorImpact = Mathf.Pow(GameSettings.Character.ArmorUnitImpact, _armor);
        return armorImpact * damage;
    }

    public IEnumerator ReachTarget()
    {
        yield return GameSettings.Character.OptimizationDelay;

        while (_enemy != null)
        {
            Attack();
            yield return GameSettings.Character.OptimizationDelay;
        }

        Reached!.Invoke(new Target(Vector3.zero, _enemy));
    }

    public void SetTarget(Target target)
    {
        target.TryGetFightebel(out _enemy);
    }    
}
